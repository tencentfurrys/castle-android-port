using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Fires an event when editing ended for a UGui InputField component. Event string data will feature the text value, and the boolean will be true is it was a cancel action")]
[ActionCategory("uGui")]
public class uGuiInputFieldOnEndEditEvent : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(InputField))]
	[Tooltip("The GameObject with the InputField ui component.")]
	public FsmOwnerDefault gameObject;

	[Tooltip("Send this event when editing ended.")]
	public FsmEvent sendEvent;

	[Tooltip("The content of the InputField when edited ended")]
	[UIHint(UIHint.Variable)]
	public FsmString text;

	[Tooltip("The canceled state of the InputField when edited ended")]
	[UIHint(UIHint.Variable)]
	public FsmBool wasCanceled;

	private InputField _inputField;

	public override void Reset()
	{
		gameObject = null;
		sendEvent = null;
		text = null;
		wasCanceled = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_inputField = ownerDefaultTarget.GetComponent<InputField>();
			if (_inputField != null)
			{
				_inputField.onEndEdit.AddListener(DoOnEndEdit);
			}
			else
			{
				LogError("Missing UI.InputField on " + ownerDefaultTarget.name);
			}
		}
		else
		{
			LogError("Missing GameObject");
		}
	}

	public override void OnExit()
	{
		if (_inputField != null)
		{
			_inputField.onEndEdit.RemoveListener(DoOnEndEdit);
		}
	}

	public void DoOnEndEdit(string value)
	{
		text.Value = value;
		wasCanceled.Value = _inputField.wasCanceled;
		Fsm.EventData.StringData = value;
		Fsm.EventData.BoolData = _inputField.wasCanceled;
		Fsm.Event(sendEvent);
	}
}
