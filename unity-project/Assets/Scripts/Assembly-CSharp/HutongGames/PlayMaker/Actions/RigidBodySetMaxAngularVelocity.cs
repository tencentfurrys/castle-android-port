using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Set The maximimum angular velocity of the rigidbody. (Default 7) range { 0, infinity }.")]
	public class RigidBodySetMaxAngularVelocity : ComponentAction<Rigidbody>
	{
		[Tooltip("The GameObject to set the  maximimum angular velocity of.")]
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The maximimum angular velocity. (Default 7) range { 0, infinity }.")]
		public FsmFloat maxAngularVelocity;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			maxAngularVelocity = 7f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetMaxAngularVelocity();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetMaxAngularVelocity();
		}

		private void DoSetMaxAngularVelocity()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				base.rigidbody.maxAngularVelocity = maxAngularVelocity.Value;
			}
		}

		public override string ErrorCheck()
		{
			return string.Empty;
		}
	}
}
