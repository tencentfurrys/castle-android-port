using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Get KeyCode and set to string ")]
	public class GetKeyCodeToString : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString storeResult;

		public FsmEvent sendEvent;

		private KeyCode kcode;

		private KeyCode newcode;

		public override void Reset()
		{
			storeResult = null;
		}

		public override void OnUpdate()
		{
			detectpressedkey();
		}

		public void detectpressedkey()
		{
			foreach (int value in Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKey((KeyCode)value))
				{
					newcode = (KeyCode)value;
				}
				if (Input.anyKey)
				{
					storeResult.Value = newcode.ToString();
					base.Fsm.Event(sendEvent);
				}
			}
		}
	}
}
