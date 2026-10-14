using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Rotates a 2d Game Object on it's z axis so its forward vector points at a Target. Optional clamp between angles")]
[ActionCategory(ActionCategory.Transform)]
public class LookAt2dGameObjectClamped : FsmStateAction
{
	[Tooltip("The GameObject to rotate.")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("The GameObject to Look At.")]
	public FsmGameObject targetObject;

	[Tooltip("Set the GameObject starting offset. In degrees. 0 if your object is facing right, 180 if facing left etc...")]
	public FsmFloat rotationOffset;

	[Tooltip("The minimum angle limit (the limit being -180 degrees). Leave to none for no effect ")]
	public FsmFloat MinAngleLimit;

	[Tooltip("The maximum angle limit (the limit being 180 degrees). Leave to none for no effect ")]
	public FsmFloat MaxAngleLimit;

	[Title("Draw Debug Line")]
	[Tooltip("Draw a debug line from the GameObject to the Target.")]
	public FsmBool debug;

	[Tooltip("Color to use for the debug line.")]
	public FsmColor debugLineColor;

	[Tooltip("Repeat every frame.")]
	public bool everyFrame = true;

	private GameObject go;

	private GameObject goTarget;

	private Vector3 lookAtPos;

	public override void Reset()
	{
		gameObject = null;
		targetObject = null;
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
		go = Fsm.GetOwnerDefaultTarget(gameObject);
		goTarget = targetObject.Value;
		if (go == null || targetObject == null)
		{
			return;
		}
		Vector3 vector = goTarget.transform.position - go.transform.position;
		vector.Normalize();
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f - rotationOffset.Value;
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
				float magnitude = (goTarget.transform.position - go.transform.position).magnitude;
				Vector3 vector2 = go.transform.position + go.transform.TransformDirection(Vector3.right) * magnitude;
				Debug.DrawLine(go.transform.position, vector2, debugLineColor.Value);
				Debug.DrawLine(go.transform.position, goTarget.transform.position, debugLineColor.Value);
				Debug.DrawLine(vector2, goTarget.transform.position, debugLineColor.Value);
			}
			else
			{
				Debug.DrawLine(go.transform.position, goTarget.transform.position, debugLineColor.Value);
			}
		}
	}
}
