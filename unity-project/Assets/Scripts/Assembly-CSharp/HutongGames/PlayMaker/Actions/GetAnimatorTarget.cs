using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the position and rotation of the target specified by SetTarget(AvatarTarget targetIndex, float targetNormalizedTime)).\nThe position and rotation are only valid when a frame has being evaluated after the SetTarget call")]
	[ActionCategory("Animator")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1058")]
	public class GetAnimatorTarget : FsmStateAction
	{
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
		public bool everyFrame;

		[UIHint(UIHint.Variable)]
		[Tooltip("The target position")]
		[ActionSection("Results")]
		public FsmVector3 targetPosition;

		[Tooltip("The target rotation")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion targetRotation;

		[Tooltip("If set, apply the position and rotation to this gameObject")]
		public FsmGameObject targetGameObject;

		private PlayMakerAnimatorMoveProxy _animatorProxy;

		private Animator _animator;

		private Transform _transform;

		public override void Reset()
		{
			gameObject = null;
			targetPosition = null;
			targetRotation = null;
			targetGameObject = null;
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
			GameObject value = targetGameObject.Value;
			if (value != null)
			{
				_transform = value.transform;
			}
			DoGetTarget();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (_animatorProxy == null)
			{
				DoGetTarget();
			}
		}

		public void OnAnimatorMoveEvent()
		{
			DoGetTarget();
		}

		private void DoGetTarget()
		{
			if (!(_animator == null))
			{
				targetPosition.Value = _animator.targetPosition;
				targetRotation.Value = _animator.targetRotation;
				if (_transform != null)
				{
					_transform.position = _animator.targetPosition;
					_transform.rotation = _animator.targetRotation;
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
