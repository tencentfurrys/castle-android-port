using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("uGui")]
[Tooltip("Gets the normalized value ( between 0 and 1) of a UGui Slider component.")]
public class uGuiSliderGetNormalizedValue : FsmStateAction
{
	[Tooltip("The GameObject with the slider UGui component.")]
	[RequiredField]
	[CheckForComponent(typeof(Slider))]
	public FsmOwnerDefault gameObject;

	[Tooltip("The normalized value of the UGui slider component.")]
	[UIHint(UIHint.Variable)]
	[RequiredField]
	public FsmFloat value;

	[Tooltip("Repeats every frame")]
	public bool everyFrame;

	private Slider _slider;

	public override void Reset()
	{
		gameObject = null;
		value = null;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_slider = ownerDefaultTarget.GetComponent<Slider>();
		}
		DoGetValue();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoGetValue();
	}

	private void DoGetValue()
	{
		if (_slider != null)
		{
			value.Value = _slider.normalizedValue;
		}
	}
}
