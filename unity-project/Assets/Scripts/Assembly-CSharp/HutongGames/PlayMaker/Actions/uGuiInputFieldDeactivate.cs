using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Deactivate to begin processing Events for a UGui InputField component. Optionally Activate on state exit")]
	[ActionCategory("uGui")]
	public class uGuiInputFieldDeactivate : FsmStateAction
	{
		[CheckForComponent(typeof(InputField))]
		[Tooltip("The GameObject with the InputField ui component.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Activate when exiting this state.")]
		public FsmBool activateOnExit;

		private InputField _inputField;

		public override void Reset()
		{
			gameObject = null;
			activateOnExit = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_inputField = ownerDefaultTarget.GetComponent<InputField>();
			}
			DoAction();
			Finish();
		}

		private void DoAction()
		{
			if (_inputField != null)
			{
				_inputField.DeactivateInputField();
			}
		}

		public override void OnExit()
		{
			if (!(_inputField == null) && activateOnExit.Value)
			{
				_inputField.ActivateInputField();
			}
		}
	}
}
