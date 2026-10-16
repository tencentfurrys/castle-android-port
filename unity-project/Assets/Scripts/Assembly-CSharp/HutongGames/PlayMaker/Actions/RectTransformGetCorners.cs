using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the corners of the calculated rectangle in world space or local space of its Transform.")]
	[ActionCategory("RectTransform")]
	public class RectTransformGetCorners : FsmStateActionAdvanced
	{
		[RequiredField]
		[CheckForComponent(typeof(RectTransform))]
		[Tooltip("The GameObject target.")]
		public FsmOwnerDefault gameObject;

		public FsmBool localspace;

		[UIHint(UIHint.Variable)]
		[Tooltip("The BottomLeft Corner")]
		public FsmVector3 bottomLeft;

		[UIHint(UIHint.Variable)]
		[Tooltip("The TopLeft Corner")]
		public FsmVector3 topLeft;

		[UIHint(UIHint.Variable)]
		[Tooltip("The TopRight Corner")]
		public FsmVector3 topRight;

		[Tooltip("The BottomRight Corner")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 bottomRight;

		private RectTransform _rt;

		private Vector3[] corners = new Vector3[4];

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			localspace = null;
			topLeft = null;
			topRight = null;
			bottomLeft = null;
			bottomRight = null;
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
			if (localspace.Value)
			{
				_rt.GetLocalCorners(corners);
			}
			else
			{
				_rt.GetWorldCorners(corners);
			}
			if (!bottomLeft.IsNone)
			{
				bottomLeft.Value = corners[0];
			}
			if (!topLeft.IsNone)
			{
				topLeft.Value = corners[1];
			}
			if (!topRight.IsNone)
			{
				topRight.Value = corners[2];
			}
			if (!bottomRight.IsNone)
			{
				bottomRight.Value = corners[3];
			}
		}
	}
}
