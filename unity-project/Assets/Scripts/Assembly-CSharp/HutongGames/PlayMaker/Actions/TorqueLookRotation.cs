using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Order a game object to look at a target by using Torque on its axi")]
	[ActionCategory(ActionCategory.Physics)]
	public class TorqueLookRotation : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The object to effect.")]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;

		[Tooltip("The target it should be looking at")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject targetObject;

		[Tooltip("The multiplier of torque to apply.")]
		public FsmFloat forceMultiplier;

		[Tooltip("The multiplier of torque to apply.")]
		public FsmFloat storeAngleTo;

		public FsmFloat maxSpeed;

		public override void Reset()
		{
			gameObject = null;
			targetObject = null;
			forceMultiplier = new FsmFloat
			{
				UseVariable = true
			};
			maxSpeed = 100f;
			storeAngleTo = new FsmFloat
			{
				UseVariable = true
			};
		}

		public override void OnLateUpdate()
		{
			DoLookAt();
		}

		private void DoLookAt()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			GameObject value = targetObject.Value;
			Vector3 vector = value.transform.position - ownerDefaultTarget.transform.position;
			float num = Vector3.Angle(ownerDefaultTarget.transform.forward, vector);
			storeAngleTo.Value = num;
			Vector3 vector2 = Vector3.Cross(ownerDefaultTarget.transform.forward, vector);
			vector2 = Vector3.ClampMagnitude(vector2, maxSpeed.Value);
			ownerDefaultTarget.GetComponent<Rigidbody>().AddTorque(vector2 * num * forceMultiplier.Value);
		}
	}
}
