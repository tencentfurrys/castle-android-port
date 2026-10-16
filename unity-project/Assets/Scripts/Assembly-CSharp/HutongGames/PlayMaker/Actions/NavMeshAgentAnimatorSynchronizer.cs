using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Synchronize a NavMesh Agent velocity and rotation with the animator process.")]
	[ActionCategory("Animator")]
	public class NavMeshAgentAnimatorSynchronizer : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(UnityEngine.AI.NavMeshAgent))]
		[CheckForComponent(typeof(Animator))]
		[CheckForComponent(typeof(PlayMakerAnimatorMoveProxy))]
		[Tooltip("The Agent target. An Animator component and a PlayMakerAnimatorMoveProxy component are required")]
		public FsmOwnerDefault gameObject;

		private PlayMakerAnimatorMoveProxy _animatorProxy;

		private Animator _animator;

		private UnityEngine.AI.NavMeshAgent _agent;

		private Transform _trans;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				Finish();
				return;
			}
			_agent = ownerDefaultTarget.GetComponent<UnityEngine.AI.NavMeshAgent>();
			_animator = ownerDefaultTarget.GetComponent<Animator>();
			if (_animator == null)
			{
				Finish();
				return;
			}
			_trans = ownerDefaultTarget.transform;
			_animatorProxy = ownerDefaultTarget.GetComponent<PlayMakerAnimatorMoveProxy>();
			if (_animatorProxy != null)
			{
				_animatorProxy.OnAnimatorMoveEvent += OnAnimatorMoveEvent;
			}
		}

		public void OnAnimatorMoveEvent()
		{
			_agent.velocity = _animator.deltaPosition / Time.deltaTime;
			_trans.rotation = _animator.rootRotation;
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
