using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Animator")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1063")]
	[Tooltip("Sets the value of a bool parameter")]
	public class SetAnimatorBool : FsmStateAction
	{
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The animator parameter")]
		public FsmString parameter;

		[Tooltip("The Bool value to assign to the animator parameter")]
		public FsmBool Value;

		[Tooltip("Repeat every frame. Useful when value is changing over time.")]
		public bool everyFrame;

		private PlayMakerAnimatorMoveProxy _animatorProxy;

		private Animator _animator;

		private int _paramID;

		public override void Reset()
		{
			gameObject = null;
			parameter = null;
			Value = null;
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
			_paramID = Animator.StringToHash(parameter.Value);
			SetParameter();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public void OnAnimatorMoveEvent()
		{
			if (_animatorProxy != null)
			{
				SetParameter();
			}
		}

		public override void OnUpdate()
		{
			if (_animatorProxy == null)
			{
				SetParameter();
			}
		}

		private void SetParameter()
		{
			if (_animator != null)
			{
				_animator.SetBool(_paramID, Value.Value);
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
