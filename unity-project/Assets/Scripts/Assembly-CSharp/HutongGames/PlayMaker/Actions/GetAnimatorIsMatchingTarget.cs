using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns true if automatic matching is active. Can also send events")]
	[ActionCategory("Animator")]
	public class GetAnimatorIsMatchingTarget : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Repeat every frame. Warning: do not use the events in this action if you set everyFrame to true")]
		public bool everyFrame;

		[Tooltip("True if automatic matching is active")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmBool isMatchingActive;

		[Tooltip("Event send if automatic matching is active")]
		public FsmEvent matchingActivatedEvent;

		[Tooltip("Event send if automatic matching is not active")]
		public FsmEvent matchingDeactivedEvent;

		private PlayMakerAnimatorMoveProxy _animatorProxy;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			isMatchingActive = null;
			matchingActivatedEvent = null;
			matchingDeactivedEvent = null;
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
			DoCheckIsMatchingActive();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (_animatorProxy == null)
			{
				DoCheckIsMatchingActive();
			}
		}

		public void OnAnimatorMoveEvent()
		{
			DoCheckIsMatchingActive();
		}

		private void DoCheckIsMatchingActive()
		{
			if (!(_animator == null))
			{
				bool isMatchingTarget = _animator.isMatchingTarget;
				isMatchingActive.Value = isMatchingTarget;
				if (isMatchingTarget)
				{
					base.Fsm.Event(matchingActivatedEvent);
				}
				else
				{
					base.Fsm.Event(matchingDeactivedEvent);
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
