using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rotates a RigidBody to a particular rotation. It's more efficient then accessing the transform of the gameObject and interacts properly with colliders and physics materials")]
	[ActionCategory(ActionCategory.Physics)]
	public class RigidBodyMoveRotation : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		[Tooltip("The rigid body to rotate.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("A rotation quaternion.")]
		public FsmQuaternion rotation;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		private Rigidbody rigidBody;

		public override void Reset()
		{
			gameObject = null;
			rotation = null;
			everyFrame = true;
		}

		public override void Awake()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				LogError("Missing gameObject target");
				return;
			}
			rigidBody = ownerDefaultTarget.GetComponent<Rigidbody>();
			if (rigidBody == null)
			{
				LogError("Missing Rigidbody component");
			}
		}

		public override void OnFixedUpdate()
		{
			DoRotate();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoRotate()
		{
			if (rigidBody != null)
			{
				rigidBody.MoveRotation(rotation.Value);
			}
		}
	}
}
