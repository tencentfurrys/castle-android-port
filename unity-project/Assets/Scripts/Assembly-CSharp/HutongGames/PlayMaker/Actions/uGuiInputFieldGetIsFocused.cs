using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("uGui")]
	[Tooltip("Gets the focused state of a UGui InputField component.")]
	public class uGuiInputFieldGetIsFocused : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject with the InputField ui component.")]
		[CheckForComponent(typeof(InputField))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("The is focused flag value of the UGui InputField component.")]
		public FsmBool isFocused;

		[Tooltip("Event sent if inputField is focused")]
		public FsmEvent isfocusedEvent;

		[Tooltip("Event sent if nputField is not focused")]
		public FsmEvent isNotFocusedEvent;

		private InputField _inputField;

		public override void Reset()
		{
			isFocused = null;
			isfocusedEvent = null;
			isNotFocusedEvent = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_inputField = ownerDefaultTarget.GetComponent<InputField>();
			}
			DoGetValue();
			Finish();
		}

		private void DoGetValue()
		{
			if (_inputField != null)
			{
				isFocused.Value = _inputField.isFocused;
				if (_inputField.isFocused)
				{
					base.Fsm.Event(isfocusedEvent);
				}
				else
				{
					base.Fsm.Event(isNotFocusedEvent);
				}
			}
		}
	}
}
