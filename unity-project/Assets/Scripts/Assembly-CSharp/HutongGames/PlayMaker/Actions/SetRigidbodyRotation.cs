using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Set a Rigidbody rotation. It's more efficient then accessing the transform of the gameObject.")]
	public class SetRigidbodyRotation : FsmStateAction
	{
		[Tooltip("The rigid body to rotate.")]
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The rotation.")]
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
			DoRotation();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoRotation()
		{
			if (rigidBody != null)
			{
				rigidBody.rotation = rotation.Value;
			}
		}
	}
}
