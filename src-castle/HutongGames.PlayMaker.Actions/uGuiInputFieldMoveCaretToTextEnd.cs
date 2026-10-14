using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Move Caret to text end on a UGui InputField component. Optionaly select from the current caret position")]
[ActionCategory("uGui")]
public class uGuiInputFieldMoveCaretToTextEnd : FsmStateAction
{
	[Tooltip("The GameObject with the InputField ui component.")]
	[RequiredField]
	[CheckForComponent(typeof(InputField))]
	public FsmOwnerDefault gameObject;

	[Tooltip("Define if we select or not from the current caret position. Default is true = no selection")]
	public FsmBool shift;

	private InputField _inputField;

	public override void Reset()
	{
		gameObject = null;
		shift = true;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
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
			_inputField.MoveTextEnd(shift.Value);
		}
	}
}
