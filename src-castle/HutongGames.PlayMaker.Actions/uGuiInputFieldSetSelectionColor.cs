using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("uGui")]
[Tooltip("Sets the selection's color of a UGui InputField component. This is the color of the highlighter to show what characters are selected")]
public class uGuiInputFieldSetSelectionColor : FsmStateAction
{
	[Tooltip("The GameObject with the InputField ui component.")]
	[CheckForComponent(typeof(InputField))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("The color of the highlighter to show what characters are selected for the UGui InputField component.")]
	[RequiredField]
	public FsmColor selectionColor;

	[Tooltip("Reset when exiting this state.")]
	public FsmBool resetOnExit;

	[Tooltip("Repeats every frame")]
	public bool everyFrame;

	private InputField _inputField;

	private Color _originalValue;

	public override void Reset()
	{
		gameObject = null;
		selectionColor = null;
		resetOnExit = null;
		everyFrame = false;
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
			_originalValue = _inputField.selectionColor;
		}
		DoSetValue();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoSetValue();
	}

	private void DoSetValue()
	{
		if (_inputField != null)
		{
			_inputField.selectionColor = selectionColor.Value;
		}
	}

	public override void OnExit()
	{
		if (!(_inputField == null) && resetOnExit.Value)
		{
			_inputField.selectionColor = _originalValue;
		}
	}
}
