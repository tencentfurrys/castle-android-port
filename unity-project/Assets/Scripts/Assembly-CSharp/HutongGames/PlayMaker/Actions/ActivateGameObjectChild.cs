using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Activates/deactivates a Game Object child with an exception. Use this to hide/show areas, or enable/disable many Behaviours at once.")]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=10602.0")]
	public class ActivateGameObjectChild : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Parent GameObject to activate/deactivate.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Except this child GameObject")]
		public FsmGameObject[] exceptThisChild;

		[Tooltip("Check to activate, uncheck to deactivate Game Object.")]
		[RequiredField]
		public FsmBool activate;

		[Tooltip("Repeat this action every frame. Useful if Activate changes over time.")]
		public bool everyFrame;

		private GameObject activatedGameObject;

		private bool equal;

		private GameObject temp;

		public override void Reset()
		{
			gameObject = null;
			exceptThisChild = new FsmGameObject[1];
			activate = true;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoActivateGameObject();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoActivateGameObject();
		}

		private void DoActivateGameObject()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				SetActiveRecursively(ownerDefaultTarget, activate.Value);
			}
		}

		public void SetActiveRecursively(GameObject go, bool state)
		{
			equal = false;
			foreach (Transform item in go.transform)
			{
				for (int i = 0; i < exceptThisChild.Length; i++)
				{
					temp = exceptThisChild[i].Value;
					if (item.gameObject == temp)
					{
						equal = true;
						break;
					}
					equal = false;
				}
				if (!equal)
				{
					item.gameObject.SetActive(state);
				}
			}
		}
	}
}
