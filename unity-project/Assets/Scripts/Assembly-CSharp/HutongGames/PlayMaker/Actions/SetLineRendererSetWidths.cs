using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Renderer)]
	[Tooltip("Set the start and end widths of a lineRenderer")]
	public class SetLineRendererSetWidths : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(LineRenderer))]
		[Tooltip("The GameObject with the LineRenderer component.")]
		[ActionSection("Setup")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The start Width")]
		public FsmFloat startWidth;

		[Tooltip("The end Width")]
		[RequiredField]
		public FsmFloat endWidth;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		private LineRenderer _lr;

		public override void Reset()
		{
			gameObject = null;
			startWidth = null;
			endWidth = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			SetWidths();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			SetWidths();
		}

		private void SetWidths()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				_lr = ownerDefaultTarget.GetComponent<LineRenderer>();
				if (!(_lr == null))
				{
					_lr.SetWidth(startWidth.Value, endWidth.Value);
				}
			}
		}
	}
}
