using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("RectTransform")]
	[Tooltip("Get the Local position of this RectTransform. This is Screen Space values using the anchoring as reference, so 0,0 is the center of the screen if the anchor is te center of the screen.")]
	public class RectTransformGetLocalPosition : FsmStateActionAdvanced
	{
		public enum LocalPositionReference
		{
			Anchor = 0,
			CenterPosition = 1
		}

		[RequiredField]
		[CheckForComponent(typeof(RectTransform))]
		[Tooltip("The GameObject target.")]
		public FsmOwnerDefault gameObject;

		public LocalPositionReference reference;

		[UIHint(UIHint.Variable)]
		[Tooltip("The position")]
		public FsmVector3 position;

		[UIHint(UIHint.Variable)]
		[Tooltip("The position in a Vector 2d ")]
		public FsmVector2 position2d;

		[Tooltip("The x component of the Position")]
		[UIHint(UIHint.Variable)]
		public FsmFloat x;

		[UIHint(UIHint.Variable)]
		[Tooltip("The y component of the Position")]
		public FsmFloat y;

		[Tooltip("The z component of the Position")]
		[UIHint(UIHint.Variable)]
		public FsmFloat z;

		private RectTransform _rt;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			reference = LocalPositionReference.Anchor;
			position = null;
			position2d = null;
			x = null;
			y = null;
			z = null;
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
			if (!(_rt == null))
			{
				Vector3 localPosition = _rt.localPosition;
				if (reference == LocalPositionReference.CenterPosition)
				{
					localPosition.x += _rt.rect.center.x;
					localPosition.y += _rt.rect.center.y;
				}
				if (!position.IsNone)
				{
					position.Value = localPosition;
				}
				if (!position2d.IsNone)
				{
					position2d.Value = new Vector2(localPosition.x, localPosition.y);
				}
				if (!x.IsNone)
				{
					x.Value = localPosition.x;
				}
				if (!y.IsNone)
				{
					y.Value = localPosition.y;
				}
				if (!z.IsNone)
				{
					z.Value = localPosition.z;
				}
			}
		}
	}
}
