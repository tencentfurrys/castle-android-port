using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rotates a 2d Game Object on it's z axis so its forward vector points at a 2d or 3d position. Optional clamp between angles")]
	[ActionCategory(ActionCategory.Transform)]
	public class LookAt2dClamped : FsmStateAction
	{
		[Tooltip("The GameObject to rotate.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The 2d position to Look At.")]
		public FsmVector2 vector2Target;

		[Tooltip("The 3d position to Look At. If not set to none, will be added to the 2d target")]
		public FsmVector3 vector3Target;

		[Tooltip("Set the GameObject starting offset. In degrees. 0 if your object is facing right, 180 if facing left etc...")]
		public FsmFloat rotationOffset;

		[Tooltip("The minimum angle limit (the limit being -180 degrees). Leave to none for no effect ")]
		public FsmFloat MinAngleLimit;

		[Tooltip("The maximum angle limit (the limit being 180 degrees). Leave to none for no effect ")]
		public FsmFloat MaxAngleLimit;

		[Tooltip("Draw a debug line from the GameObject to the Target.")]
		[Title("Draw Debug Line")]
		public FsmBool debug;

		[Tooltip("Color to use for the debug line.")]
		public FsmColor debugLineColor;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame = true;

		private GameObject go;

		private Vector3 lookAtPos;

		public override void Reset()
		{
			gameObject = null;
			vector2Target = null;
			vector3Target = new FsmVector3
			{
				UseVariable = true
			};
			MinAngleLimit = new FsmFloat
			{
				UseVariable = true
			};
			MaxAngleLimit = new FsmFloat
			{
				UseVariable = true
			};
			debug = false;
			debugLineColor = Color.green;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoLookAt();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoLookAt();
		}

		private void DoLookAt()
		{
			go = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}
			Vector3 vector = new Vector3(vector2Target.Value.x, vector2Target.Value.y, 0f);
			if (!vector3Target.IsNone)
			{
				vector += vector3Target.Value;
			}
			Vector3 vector2 = vector - go.transform.position;
			vector2.Normalize();
			float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f - rotationOffset.Value;
			float num2 = num;
			if (!MinAngleLimit.IsNone || !MaxAngleLimit.IsNone)
			{
				float min = ((!MinAngleLimit.IsNone) ? MinAngleLimit.Value : (-180f));
				float max = ((!MaxAngleLimit.IsNone) ? MaxAngleLimit.Value : 180f);
				num2 = Mathf.Clamp(num2, min, max);
			}
			go.transform.rotation = Quaternion.Euler(0f, 0f, num2);
			if (debug.Value)
			{
				if (Mathf.Abs(num - num2) > 0.01f)
				{
					float magnitude = (vector - go.transform.position).magnitude;
					Vector3 vector3 = go.transform.position + go.transform.TransformDirection(Vector3.right) * magnitude;
					Debug.DrawLine(go.transform.position, vector3, debugLineColor.Value);
					Debug.DrawLine(go.transform.position, vector, debugLineColor.Value);
					Debug.DrawLine(vector3, vector, debugLineColor.Value);
				}
				else
				{
					Debug.DrawLine(go.transform.position, vector, debugLineColor.Value);
				}
			}
		}
	}
}
