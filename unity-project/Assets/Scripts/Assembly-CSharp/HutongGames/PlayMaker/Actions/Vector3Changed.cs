using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if the value of a Float variable changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
	public class Vector3Changed : FsmStateAction
	{
		[Tooltip("The Float variable to watch for a change.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 Vector3Variable;

		[Tooltip("Event to send if the float variable changes.")]
		public FsmEvent changedEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Set to True if the float variable changes.")]
		public FsmBool storeResult;

		private Vector3 previousValue;

		public override void Reset()
		{
			Vector3Variable = null;
			changedEvent = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			if (Vector3Variable.IsNone)
			{
				Finish();
			}
			else
			{
				previousValue = Vector3Variable.Value;
			}
		}

		public override void OnUpdate()
		{
			storeResult.Value = false;
			if (Vector3Variable.Value != previousValue)
			{
				previousValue = Vector3Variable.Value;
				storeResult.Value = true;
				base.Fsm.Event(changedEvent);
			}
		}
	}
}
