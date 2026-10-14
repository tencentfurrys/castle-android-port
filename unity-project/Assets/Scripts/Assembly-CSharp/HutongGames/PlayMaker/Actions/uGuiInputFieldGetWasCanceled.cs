using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the cancel state of a UGui InputField component. This relates to the last onEndEdit Event")]
	[ActionCategory("uGui")]
	public class uGuiInputFieldGetWasCanceled : FsmStateAction
	{
		[CheckForComponent(typeof(InputField))]
		[RequiredField]
		[Tooltip("The GameObject with the InputField ui component.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("The was canceled flag value of the UGui InputField component.")]
		public FsmBool wasCanceled;

		[Tooltip("Event sent if inputField was canceled")]
		public FsmEvent wasCanceledEvent;

		[Tooltip("Event sent if inputField was not canceled")]
		public FsmEvent wasNotCanceledEvent;

		private InputField _inputField;

		public override void Reset()
		{
			wasCanceled = null;
			wasCanceledEvent = null;
			wasNotCanceledEvent = null;
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
				wasCanceled.Value = _inputField.wasCanceled;
				if (_inputField.wasCanceled)
				{
					base.Fsm.Event(wasCanceledEvent);
				}
				else
				{
					base.Fsm.Event(wasNotCanceledEvent);
				}
			}
		}
	}
}
