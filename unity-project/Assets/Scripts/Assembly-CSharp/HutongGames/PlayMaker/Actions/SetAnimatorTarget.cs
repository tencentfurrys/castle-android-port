using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets an AvatarTarget and a targetNormalizedTime for the current state")]
	[ActionCategory("Animator")]
	public class SetAnimatorTarget : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The avatar target")]
		public AvatarTarget avatarTarget;

		[Tooltip("The current state Time that is queried")]
		public FsmFloat targetNormalizedTime;

		[Tooltip("Repeat every frame. Useful when changing over time.")]
		public bool everyFrame;

		private PlayMakerAnimatorMoveProxy _animatorProxy;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			avatarTarget = AvatarTarget.Body;
			targetNormalizedTime = null;
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
			SetTarget();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public void OnAnimatorMoveEvent()
		{
			if (_animatorProxy != null)
			{
				SetTarget();
			}
		}

		public override void OnUpdate()
		{
			if (_animatorProxy == null)
			{
				SetTarget();
			}
		}

		private void SetTarget()
		{
			if (_animator != null)
			{
				_animator.SetTarget(avatarTarget, targetNormalizedTime.Value);
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
