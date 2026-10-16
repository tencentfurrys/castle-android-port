using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Renderer)]
	[Tooltip("Set a particular position of a lineRenderer")]
	public class SetLineRendererPosition : FsmStateAction
	{
		[Tooltip("The GameObject with the LineRenderer component.")]
		[ActionSection("Setup")]
		[CheckForComponent(typeof(LineRenderer))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The index")]
		[RequiredField]
		public FsmInt index;

		[Tooltip("The Position")]
		[RequiredField]
		public FsmVector3 position;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		private LineRenderer _lr;

		public override void Reset()
		{
			gameObject = null;
			position = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			SetPosition();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			SetPosition();
		}

		private void SetPosition()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				_lr = ownerDefaultTarget.GetComponent<LineRenderer>();
				if (!(_lr == null))
				{
					_lr.SetPosition(index.Value, position.Value);
				}
			}
		}
	}
}
