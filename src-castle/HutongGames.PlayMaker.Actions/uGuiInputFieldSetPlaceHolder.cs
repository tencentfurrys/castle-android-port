using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Sets the playceholder of a UGui InputField component. Optionally reset on exit")]
[ActionCategory("uGui")]
public class uGuiInputFieldSetPlaceHolder : FsmStateAction
{
	[CheckForComponent(typeof(InputField))]
	[Tooltip("The GameObject with the InputField ui component.")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[RequiredField]
	[Tooltip("The placeholder ( any graphic extended uGui Component) for the UGui InputField component.")]
	[CheckForComponent(typeof(Graphic))]
	public FsmGameObject placeholder;

	[Tooltip("Reset when exiting this state.")]
	public FsmBool resetOnExit;

	private InputField _inputField;

	private Graphic _originalValue;

	public override void Reset()
	{
		gameObject = null;
		placeholder = null;
		resetOnExit = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_inputField = ownerDefaultTarget.GetComponent<InputField>();
		}
		if (resetOnExit.Value)
		{
			_originalValue = _inputField.placeholder;
		}
		DoSetValue();
		Finish();
	}

	private void DoSetValue()
	{
		if (_inputField != null)
		{
			GameObject value = placeholder.Value;
			if (value == null)
			{
				_inputField.placeholder = null;
			}
			else
			{
				_inputField.placeholder = value.GetComponent<Graphic>();
			}
		}
	}

	public override void OnExit()
	{
		if (!(_inputField == null) && resetOnExit.Value)
		{
			_inputField.placeholder = _originalValue;
		}
	}
}
