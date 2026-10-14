using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the offset of the lower left corner of the rectangle relative to the lower left anchor")]
	[ActionCategory("RectTransform")]
	public class RectTransformGetOffsetMin : FsmStateActionAdvanced
	{
		[Tooltip("The GameObject target.")]
		[RequiredField]
		[CheckForComponent(typeof(RectTransform))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("The offsetMin")]
		public FsmVector2 offsetMin;

		[UIHint(UIHint.Variable)]
		[Tooltip("The x component of the offsetMin")]
		public FsmFloat x;

		[Tooltip("The y component of the offsetMin")]
		[UIHint(UIHint.Variable)]
		public FsmFloat y;

		private RectTransform _rt;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			offsetMin = null;
			x = null;
			y = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
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
			if (!offsetMin.IsNone)
			{
				offsetMin.Value = _rt.offsetMin;
			}
			if (!x.IsNone)
			{
				x.Value = _rt.offsetMin.x;
			}
			if (!y.IsNone)
			{
				y.Value = _rt.offsetMin.y;
			}
		}
	}
}
