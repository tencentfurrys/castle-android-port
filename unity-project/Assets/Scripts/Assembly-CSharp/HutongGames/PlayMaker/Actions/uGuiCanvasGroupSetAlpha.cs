using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("uGui")]
	[Tooltip("Set Group Alpha.")]
	public class uGuiCanvasGroupSetAlpha : FsmStateAction
	{
		[CheckForComponent(typeof(CanvasGroup))]
		[Tooltip("The GameObject with an Unity UI CanvasGroup component.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The alpha of the UI component.")]
		public FsmFloat alpha;

		[Tooltip("Reset when exiting this state.")]
		public FsmBool resetOnExit;

		[Tooltip("Repeats every frame, useful for animation")]
		public bool everyFrame;

		private CanvasGroup _component;

		private float _originalValue;

		public override void Reset()
		{
			gameObject = null;
			alpha = null;
			resetOnExit = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_component = ownerDefaultTarget.GetComponent<CanvasGroup>();
			}
			if (resetOnExit.Value)
			{
				_originalValue = _component.alpha;
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
			if (_component != null)
			{
				_component.alpha = alpha.Value;
			}
		}

		public override void OnExit()
		{
			if (!(_component == null) && resetOnExit.Value)
			{
				_component.alpha = _originalValue;
			}
		}
	}
}
