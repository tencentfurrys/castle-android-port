using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("RectTransform")]
	[Tooltip("The normalized position in the parent RectTransform that the lower left corner is anchored to. This is relative screen space, values ranges from 0 to 1")]
	public class RectTransformSetAnchorMin : FsmStateActionAdvanced
	{
		[RequiredField]
		[CheckForComponent(typeof(RectTransform))]
		[Tooltip("The GameObject target.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The Vector2 anchor. Set to none for no effect, and/or set individual axis below.")]
		public FsmVector2 anchorMin;

		[Tooltip("Setting only the x value. Overides anchorMin x value if set. Set to none for no effect")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat x;

		[Tooltip("Setting only the x value. Overides anchorMin x value if set. Set to none for no effect")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat y;

		private RectTransform _rt;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			anchorMin = null;
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
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_rt = ownerDefaultTarget.GetComponent<RectTransform>();
			}
			DoSetAnchorMin();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnActionUpdate()
		{
			DoSetAnchorMin();
		}

		private void DoSetAnchorMin()
		{
			Vector2 value = _rt.anchorMin;
			if (!anchorMin.IsNone)
			{
				value = anchorMin.Value;
			}
			if (!x.IsNone)
			{
				value.x = x.Value;
			}
			if (!y.IsNone)
			{
				value.y = y.Value;
			}
			_rt.anchorMin = value;
		}
	}
}
