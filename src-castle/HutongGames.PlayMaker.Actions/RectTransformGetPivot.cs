using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Get the normalized position in this RectTransform that it rotates around.")]
[ActionCategory("RectTransform")]
public class RectTransformGetPivot : FsmStateActionAdvanced
{
	[CheckForComponent(typeof(RectTransform))]
	[RequiredField]
	[Tooltip("The GameObject target.")]
	public FsmOwnerDefault gameObject;

	[UIHint(UIHint.Variable)]
	[Tooltip("The pivot")]
	public FsmVector2 pivot;

	[Tooltip("The x component of the pivot")]
	[UIHint(UIHint.Variable)]
	public FsmFloat x;

	[Tooltip("The y component of the pivot")]
	[UIHint(UIHint.Variable)]
	public FsmFloat y;

	private RectTransform _rt;

	public override void Reset()
	{
		base.Reset();
		gameObject = null;
		pivot = null;
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
		if (!pivot.IsNone)
		{
			pivot.Value = _rt.pivot;
		}
		if (!x.IsNone)
		{
			x.Value = _rt.pivot.x;
		}
		if (!y.IsNone)
		{
			y.Value = _rt.pivot.y;
		}
	}
}
