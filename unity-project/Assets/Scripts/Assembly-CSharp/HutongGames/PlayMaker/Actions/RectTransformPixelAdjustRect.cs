using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Given a rect transform, return the corner points in pixel accurate coordinates.")]
	[ActionCategory("RectTransform")]
	public class RectTransformPixelAdjustRect : FsmStateActionAdvanced
	{
		[RequiredField]
		[Tooltip("The GameObject target.")]
		[CheckForComponent(typeof(RectTransform))]
		public FsmOwnerDefault gameObject;

		[CheckForComponent(typeof(Canvas))]
		[RequiredField]
		[Tooltip("The canvas. Leave to none to use the canvas of the gameObject")]
		public FsmGameObject canvas;

		[Tooltip("Pixel adjusted rect.")]
		[ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmRect pixelRect;

		private RectTransform _rt;

		private Canvas _canvas;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			canvas = new FsmGameObject
			{
				UseVariable = true
			};
			pixelRect = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_rt = ownerDefaultTarget.GetComponent<RectTransform>();
			}
			GameObject value = canvas.Value;
			if (value != null)
			{
				_canvas = value.GetComponent<Canvas>();
			}
			if (_canvas == null && ownerDefaultTarget != null)
			{
				Graphic component = ownerDefaultTarget.GetComponent<Graphic>();
				if (component != null)
				{
					_canvas = component.canvas;
				}
			}
			DoAction();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnActionUpdate()
		{
			DoAction();
		}

		private void DoAction()
		{
			pixelRect.Value = RectTransformUtility.PixelAdjustRect(_rt, _canvas);
		}
	}
}
