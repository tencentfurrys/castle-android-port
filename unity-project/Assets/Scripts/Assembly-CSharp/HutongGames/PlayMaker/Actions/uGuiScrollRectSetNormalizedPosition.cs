using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("uGui")]
	[Tooltip("The scroll normalized position as a Vector2 between (0,0) and (1,1) with (0,0) being the lower left corner.")]
	public class uGuiScrollRectSetNormalizedPosition : FsmStateAction
	{
		[CheckForComponent(typeof(ScrollRect))]
		[Tooltip("The GameObject with the ScrollRect UGui component.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The position's value of the UGui Scrollbar component. Ranges from 0.0 to 1.0.")]
		public FsmVector2 normalizedPosition;

		[HasFloatSlider(0f, 1f)]
		[Tooltip("The horizontal position's value of the UGui ScrollRect component. Ranges from 0.0 to 1.0.")]
		public FsmFloat horizontalPosition;

		[Tooltip("The vertical position's value of the UGui ScrollRect component. Ranges from 0.0 to 1.0.")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat verticalPosition;

		[Tooltip("Reset when exiting this state.")]
		public FsmBool resetOnExit;

		[Tooltip("Repeats every frame")]
		public bool everyFrame;

		private ScrollRect _scrollRect;

		private Vector2 _originalValue;

		public override void Reset()
		{
			gameObject = null;
			normalizedPosition = null;
			horizontalPosition = new FsmFloat
			{
				UseVariable = true
			};
			verticalPosition = new FsmFloat
			{
				UseVariable = true
			};
			resetOnExit = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_scrollRect = ownerDefaultTarget.GetComponent<ScrollRect>();
			}
			if (resetOnExit.Value)
			{
				_originalValue = _scrollRect.normalizedPosition;
			}
			DoSetValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetValue();
		}

		private void DoSetValue()
		{
			if (_scrollRect != null)
			{
				Vector2 value = _scrollRect.normalizedPosition;
				if (!normalizedPosition.IsNone)
				{
					value = normalizedPosition.Value;
				}
				if (!horizontalPosition.IsNone)
				{
					value.x = horizontalPosition.Value;
				}
				if (!verticalPosition.IsNone)
				{
					value.y = verticalPosition.Value;
				}
				_scrollRect.normalizedPosition = value;
			}
		}

		public override void OnExit()
		{
			if (!(_scrollRect == null) && resetOnExit.Value)
			{
				_scrollRect.normalizedPosition = _originalValue;
			}
		}
	}
}
