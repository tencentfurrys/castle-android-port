using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Activates/deactivates children of a gameObject. Can optionnaly define depth of recursion to only affects so many levels of children")]
	[ActionCategory(ActionCategory.GameObject)]
	public class ActivateChildren : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to activate/deactivate.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Check to activate, uncheck to deactivate Game Object.")]
		[RequiredField]
		public FsmBool activate;

		[Tooltip("Recursively activate/deactivate all children given their depth. 0 direct children, 1 means grand children, -1 means all")]
		public FsmInt recursiveDepth;

		[Tooltip("Reset the game objects when exiting this state. Useful if you want an object to be active only while this state is active.\nNote: Only applies to the last Game Object activated/deactivated (won't work if Game Object changes).")]
		public bool resetOnExit;

		private GameObject parent;

		public override void Reset()
		{
			gameObject = null;
			activate = true;
			recursiveDepth = 0;
			resetOnExit = false;
		}

		public override void OnEnter()
		{
			parent = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (parent != null)
			{
				foreach (Transform item in parent.transform)
				{
					SetActiveRecursively(item.gameObject, activate.Value, 0);
				}
			}
			Finish();
		}

		public override void OnExit()
		{
			if (parent == null || !resetOnExit)
			{
				return;
			}
			foreach (Transform item in parent.transform)
			{
				SetActiveRecursively(item.gameObject, !activate.Value, 0);
			}
		}

		public void SetActiveRecursively(GameObject go, bool state, int currentDepth)
		{
			go.SetActive(state);
			if (recursiveDepth.Value >= 0 && currentDepth >= recursiveDepth.Value)
			{
				return;
			}
			int currentDepth2 = currentDepth + 1;
			foreach (Transform item in go.transform)
			{
				SetActiveRecursively(item.gameObject, state, currentDepth2);
			}
		}
	}
}
