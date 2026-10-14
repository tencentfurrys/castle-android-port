using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Sets the isOn property of a UGui Toggle component.")]
[ActionCategory("uGui")]
public class uGuiToggleSetIsOn : FsmStateAction
{
	[Tooltip("The GameObject with the Toggle UGui component.")]
	[RequiredField]
	[CheckForComponent(typeof(Toggle))]
	public FsmOwnerDefault gameObject;

	[Tooltip("Should the toggle be on?")]
	[RequiredField]
	public FsmBool isOn;

	[Tooltip("Reset when exiting this state.")]
	public FsmBool resetOnExit;

	private Toggle _toggle;

	private bool _originalValue;

	public override void Reset()
	{
		gameObject = null;
		isOn = null;
		resetOnExit = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_toggle = ownerDefaultTarget.GetComponent<Toggle>();
		}
		if (resetOnExit.Value)
		{
			_originalValue = _toggle.isOn;
		}
		DoSetValue();
		Finish();
	}

	private void DoSetValue()
	{
		if (_toggle != null)
		{
			_toggle.isOn = isOn.Value;
		}
	}

	public override void OnExit()
	{
		if (!(_toggle == null) && resetOnExit.Value)
		{
			_toggle.isOn = _originalValue;
		}
	}
}
