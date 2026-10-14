using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Sets various values of a UGui Radial Layout component.")]
[ActionCategory("uGui")]
public class uGuiRadialLayoutSetProperties : FsmStateAction
{
	[Tooltip("The GameObject with the Radial Layout component.")]
	[CheckForComponent(typeof(RadialLayout))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("The f distance.")]
	[ActionSection("Values")]
	public FsmFloat fDistance;

	[HasFloatSlider(0f, 360f)]
	[Tooltip("The min or start angle in degrees")]
	public FsmFloat minAngle;

	[Tooltip("The max or end angle in degrees")]
	[HasFloatSlider(0f, 360f)]
	public FsmFloat maxAngle;

	[HasFloatSlider(0f, 360f)]
	[Tooltip("The start Angle in degrees")]
	public FsmFloat startAngle;

	[Tooltip("Repeats every frame. Useful for animation")]
	public bool everyFrame;

	private RadialLayout _layoutElement;

	public override void Reset()
	{
		gameObject = null;
		fDistance = new FsmFloat
		{
			UseVariable = true
		};
		minAngle = new FsmFloat
		{
			UseVariable = true
		};
		maxAngle = new FsmFloat
		{
			UseVariable = true
		};
		startAngle = new FsmFloat
		{
			UseVariable = true
		};
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_layoutElement = ownerDefaultTarget.GetComponent<RadialLayout>();
		}
		DoSetValues();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoSetValues();
	}

	private void DoSetValues()
	{
		if (_layoutElement != null)
		{
			if (!fDistance.IsNone)
			{
				_layoutElement.fDistance = fDistance.Value;
			}
			if (!minAngle.IsNone)
			{
				_layoutElement.MinAngle = minAngle.Value;
			}
			if (!maxAngle.IsNone)
			{
				_layoutElement.MaxAngle = maxAngle.Value;
			}
			if (!startAngle.IsNone)
			{
				_layoutElement.StartAngle = startAngle.Value;
			}
			_layoutElement.CalculateLayoutInputVertical();
		}
	}
}
