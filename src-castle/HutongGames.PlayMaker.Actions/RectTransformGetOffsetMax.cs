using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("RectTransform")]
[Tooltip("Get the offset of the upper right corner of the rectangle relative to the upper right anchor")]
public class RectTransformGetOffsetMax : FsmStateActionAdvanced
{
	[CheckForComponent(typeof(RectTransform))]
	[Tooltip("The GameObject target.")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("The offsetMax")]
	[UIHint(UIHint.Variable)]
	public FsmVector2 offsetMax;

	[UIHint(UIHint.Variable)]
	[Tooltip("The x component of the offsetMax")]
	public FsmFloat x;

	[UIHint(UIHint.Variable)]
	[Tooltip("The y component of the offsetMax")]
	public FsmFloat y;

	private RectTransform _rt;

	public override void Reset()
	{
		base.Reset();
		gameObject = null;
		offsetMax = null;
		x = null;
		y = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_rt = ownerDefaultTarget.GetComponent<RectTransform>();
		}
		DoGetValues();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnActionUpdate()
	{
		DoGetValues();
	}

	private void DoGetValues()
	{
		if (!offsetMax.IsNone)
		{
			offsetMax.Value = _rt.offsetMax;
		}
		if (!x.IsNone)
		{
			x.Value = _rt.offsetMax.x;
		}
		if (!y.IsNone)
		{
			y.Value = _rt.offsetMax.y;
		}
	}
}
