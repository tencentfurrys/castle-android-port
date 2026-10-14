using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1062")]
	[Tooltip("Sets the position and rotation of the body. A GameObject can be set to control the position and rotation, or it can be manually expressed.")]
	[ActionCategory("Animator")]
	public class SetAnimatorBody : FsmStateAction
	{
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The gameObject target of the ik goal")]
		public FsmGameObject target;

		[Tooltip("The position of the ik goal. If Goal GameObject set, position is used as an offset from Goal")]
		public FsmVector3 position;

		[Tooltip("The rotation of the ik goal.If Goal GameObject set, rotation is used as an offset from Goal")]
		public FsmQuaternion rotation;

		[Tooltip("Repeat every frame. Useful when changing over time.")]
		public bool everyFrame;

		private PlayMakerAnimatorIKProxy _animatorProxy;

		private Animator _animator;

		private Transform _transform;

		public override void Reset()
		{
			gameObject = null;
			target = null;
			position = new FsmVector3
			{
				UseVariable = true
			};
			rotation = new FsmQuaternion
			{
				UseVariable = true
			};
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
			_animatorProxy = ownerDefaultTarget.GetComponent<PlayMakerAnimatorIKProxy>();
			if (_animatorProxy != null)
			{
				_animatorProxy.OnAnimatorIKEvent += OnAnimatorIKEvent;
			}
			GameObject value = target.Value;
			if (value != null)
			{
				_transform = value.transform;
			}
			DoSetBody();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public void OnAnimatorIKEvent(int layer)
		{
			if (_animatorProxy != null)
			{
				DoSetBody();
			}
		}

		public override void OnUpdate()
		{
			if (_animatorProxy == null)
			{
				DoSetBody();
			}
		}

		private void DoSetBody()
		{
			if (_animator == null)
			{
				return;
			}
			if (_transform != null)
			{
				if (position.IsNone)
				{
					_animator.bodyPosition = _transform.position;
				}
				else
				{
					_animator.bodyPosition = _transform.position + position.Value;
				}
				if (rotation.IsNone)
				{
					_animator.bodyRotation = _transform.rotation;
				}
				else
				{
					_animator.bodyRotation = _transform.rotation * rotation.Value;
				}
			}
			else
			{
				if (!position.IsNone)
				{
					_animator.bodyPosition = position.Value;
				}
				if (!rotation.IsNone)
				{
					_animator.bodyRotation = rotation.Value;
				}
			}
		}

		public override void OnExit()
		{
			if (_animatorProxy != null)
			{
				_animatorProxy.OnAnimatorIKEvent -= OnAnimatorIKEvent;
			}
		}
	}
}
