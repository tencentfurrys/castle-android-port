using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns true if the specified layer is in a transition. Can also send events")]
	[ActionCategory("Animator")]
	public class GetAnimatorIsLayerInTransition : FsmStateAction
	{
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
		public bool everyFrame;

		[Tooltip("True if automatic matching is active")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmBool isInTransition;

		[Tooltip("Event send if automatic matching is active")]
		public FsmEvent isInTransitionEvent;

		[Tooltip("Event send if automatic matching is not active")]
		public FsmEvent isNotInTransitionEvent;

		private PlayMakerAnimatorMoveProxy _animatorProxy;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			isInTransition = null;
			isInTransitionEvent = null;
			isNotInTransitionEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				Finish();
				return;
			}
			_animator = ownerDefaultTarget.GetComponent<Animator>();
			if (_animator == null)
			{
				Finish();
				return;
			}
			_animatorProxy = ownerDefaultTarget.GetComponent<PlayMakerAnimatorMoveProxy>();
			if (_animatorProxy != null)
			{
				_animatorProxy.OnAnimatorMoveEvent += OnAnimatorMoveEvent;
			}
			DoCheckIsInTransition();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public void OnAnimatorMoveEvent()
		{
			if (_animatorProxy != null)
			{
				DoCheckIsInTransition();
			}
		}

		public override void OnUpdate()
		{
			if (_animatorProxy == null)
			{
				DoCheckIsInTransition();
			}
		}

		private void DoCheckIsInTransition()
		{
			if (!(_animator == null))
			{
				bool flag = _animator.IsInTransition(layerIndex.Value);
				isInTransition.Value = flag;
				if (flag)
				{
					base.Fsm.Event(isInTransitionEvent);
				}
				else
				{
					base.Fsm.Event(isNotInTransitionEvent);
				}
			}
		}

		public override void OnExit()
		{
			if (_animatorProxy != null)
			{
				_animatorProxy.OnAnimatorMoveEvent -= OnAnimatorMoveEvent;
			}
		}
	}
}
