using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Sends an Event when the Keystring is pressed. Waring!! The String must be a valid KeyCode.")]
	public class GetKeyDownFromString : FsmStateAction
	{
		[Tooltip("The keystring to test. Waring!! The String must be a valid KeyCode.")]
		[RequiredField]
		public FsmString keyCode;

		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[UIHint(UIHint.Variable)]
		public FsmString storeKey;

		private KeyCode key;

		public override void Reset()
		{
			sendEvent = null;
			keyCode = null;
			storeResult = null;
			storeKey = null;
		}

		public override void OnEnter()
		{
			key = (KeyCode)(int)Enum.Parse(typeof(KeyCode), keyCode.Value);
		}

		public override void OnUpdate()
		{
			bool keyDown = (Input.GetKeyDown(key) || CastleTouch.Down(key));
			if (keyDown)
			{
				storeKey.Value = keyCode.Value;
				base.Fsm.Event(sendEvent);
			}
			storeResult.Value = keyDown;
		}
	}
}
