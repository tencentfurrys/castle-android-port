using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the position, rotation and weights of an IK goal. A GameObject can be set to use for the position and rotation")]
	[ActionCategory("Animator")]
	public class GetAnimatorIKGoal : FsmStateAction
	{
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component is required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The IK goal")]
		public AvatarIKGoal iKGoal;

		[Tooltip("Repeat every frame. Useful for changing over time.")]
		public bool everyFrame;

		[ActionSection("Results")]
		[Tooltip("The gameObject to apply ik goal position and rotation to")]
		public FsmGameObject goal;

		[Tooltip("Gets The position of the ik goal. If Goal GameObject define, position is used as an offset from Goal")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 position;

		[Tooltip("Gets The rotation of the ik goal.If Goal GameObject define, rotation is used as an offset from Goal")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion rotation;

		[UIHint(UIHint.Variable)]
		[Tooltip("Gets The translative weight of an IK goal (0 = at the original animation before IK, 1 = at the goal)")]
		public FsmFloat positionWeight;

		[Tooltip("Gets the rotational weight of an IK goal (0 = rotation before IK, 1 = rotation at the IK goal)")]
		[UIHint(UIHint.Variable)]
		public FsmFloat rotationWeight;

		private Animator _animator;

		private Transform _transform;

		public override void Reset()
		{
			gameObject = null;
			goal = null;
			position = null;
			rotation = null;
			positionWeight = null;
			rotationWeight = null;
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
			GameObject value = goal.Value;
			if (value != null)
			{
				_transform = value.transform;
			}
			DoGetIKGoal();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetIKGoal();
		}

		private void DoGetIKGoal()
		{
			if (!(_animator == null))
			{
				if (_transform != null)
				{
					_transform.position = _animator.GetIKPosition(iKGoal);
					_transform.rotation = _animator.GetIKRotation(iKGoal);
				}
				if (!position.IsNone)
				{
					position.Value = _animator.GetIKPosition(iKGoal);
				}
				if (!rotation.IsNone)
				{
					rotation.Value = _animator.GetIKRotation(iKGoal);
				}
				if (!positionWeight.IsNone)
				{
					positionWeight.Value = _animator.GetIKPositionWeight(iKGoal);
				}
				if (!rotationWeight.IsNone)
				{
					rotationWeight.Value = _animator.GetIKRotationWeight(iKGoal);
				}
			}
		}
	}
}
