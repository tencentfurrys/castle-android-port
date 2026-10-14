using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the avatar body mass center position and rotation.Optionally accept a GameObject to get the body transform. \nThe position and rotation are local to the gameobject")]
	[ActionCategory("Animator")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1036")]
	public class GetAnimatorRoot : FsmStateAction
	{
		[Tooltip("The target.")]
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Repeat every frame. Useful when changing over time.")]
		public bool everyFrame;

		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		[Tooltip("The avatar body mass center")]
		public FsmVector3 rootPosition;

		[Tooltip("The avatar body mass center")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion rootRotation;

		[Tooltip("If set, apply the body mass center position and rotation to this gameObject")]
		public FsmGameObject bodyGameObject;

		private PlayMakerAnimatorMoveProxy _animatorProxy;

		private Animator _animator;

		private Transform _transform;

		public override void Reset()
		{
			gameObject = null;
			rootPosition = null;
			rootRotation = null;
			bodyGameObject = null;
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
			GameObject value = bodyGameObject.Value;
			if (value != null)
			{
				_transform = value.transform;
			}
			DoGetBodyPosition();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public void OnAnimatorMoveEvent()
		{
			if (_animatorProxy != null)
			{
				DoGetBodyPosition();
			}
		}

		public override void OnUpdate()
		{
			if (_animatorProxy == null)
			{
				DoGetBodyPosition();
			}
		}

		private void DoGetBodyPosition()
		{
			if (!(_animator == null))
			{
				rootPosition.Value = _animator.rootPosition;
				rootRotation.Value = _animator.rootRotation;
				if (_transform != null)
				{
					_transform.position = _animator.rootPosition;
					_transform.rotation = _animator.rootRotation;
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
