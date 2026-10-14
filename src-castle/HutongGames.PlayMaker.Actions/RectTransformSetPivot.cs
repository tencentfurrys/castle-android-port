using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("RectTransform")]
[Tooltip("The normalized position in this RectTransform that it rotates around.")]
public class RectTransformSetPivot : FsmStateActionAdvanced
{
	[Tooltip("The GameObject target.")]
	[CheckForComponent(typeof(RectTransform))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("The Vector2 pivot. Set to none for no effect, and/or set individual axis below.")]
	public FsmVector2 pivot;

	[Tooltip("Setting only the x value. Overides pivot x value if set. Set to none for no effect")]
	[HasFloatSlider(0f, 1f)]
	public FsmFloat x;

	[Tooltip("Setting only the x value. Overides pivot y value if set. Set to none for no effect")]
	[HasFloatSlider(0f, 1f)]
	public FsmFloat y;

	private RectTransform _rt;

	public override void Reset()
	{
		base.Reset();
		gameObject = null;
		pivot = null;
		x = new FsmFloat
		{
			UseVariable = true
		};
		y = new FsmFloat
		{
			UseVariable = true
		};
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_rt = ownerDefaultTarget.GetComponent<RectTransform>();
		}
		DoSetPivotPosition();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnActionUpdate()
	{
		DoSetPivotPosition();
	}

	private void DoSetPivotPosition()
	{
		Vector2 value = _rt.pivot;
		if (!pivot.IsNone)
		{
			value = pivot.Value;
		}
		if (!x.IsNone)
		{
			value.x = x.Value;
		}
		if (!y.IsNone)
		{
			value.y = y.Value;
		}
		_rt.pivot = value;
	}
}
