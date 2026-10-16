using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends an Event when the user hits any Key or Mouse Button.")]
	[ActionCategory(ActionCategory.Input)]
	public class AnyKeyStoreString : FsmStateAction
	{
		[RequiredField]
		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		public FsmString storeResult;

		public override void Reset()
		{
			sendEvent = null;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			storeResult.Value = Input.inputString;
			if (Input.anyKey)
			{
				base.Fsm.Event(sendEvent);
			}
		}
	}
}
