using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("uGui")]
[Tooltip("Fires an event when user submits for a UGui InputField component. \nThis only fires if the user press Enter, not when field looses focus or user escaped the field.\nEvent string data will feature the text value")]
public class uGuiInputFieldOnSubmitEvent : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(InputField))]
	[Tooltip("The GameObject with the InputField ui component.")]
	public FsmOwnerDefault gameObject;

	[Tooltip("Send this event when editing ended.")]
	public FsmEvent sendEvent;

	[UIHint(UIHint.Variable)]
	[Tooltip("The content of the InputField when submitting")]
	public FsmString text;

	private InputField _inputField;

	public override void Reset()
	{
		gameObject = null;
		sendEvent = null;
		text = null;
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
		if (!_inputField.wasCanceled)
		{
			text.Value = value;
			Fsm.EventData.StringData = value;
			Fsm.Event(sendEvent);
			Finish();
		}
	}
}
