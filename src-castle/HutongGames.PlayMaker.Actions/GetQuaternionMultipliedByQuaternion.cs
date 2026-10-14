namespace HutongGames.PlayMaker.Actions;

[ActionCategory("Quaternion")]
[Tooltip("Get the quaternion from a quaternion multiplied by a quaternion.")]
[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W969")]
public class GetQuaternionMultipliedByQuaternion : QuaternionBaseAction
{
	[Tooltip("The first quaternion to multiply")]
	[RequiredField]
	public FsmQuaternion quaternionA;

	[Tooltip("The second quaternion to multiply")]
	[RequiredField]
	public FsmQuaternion quaternionB;

	[UIHint(UIHint.Variable)]
	[RequiredField]
	[Tooltip("The resulting quaternion")]
	public FsmQuaternion result;

	public override void Reset()
	{
		quaternionA = null;
		quaternionB = null;
		result = null;
		everyFrame = false;
		everyFrameOption = everyFrameOptions.Update;
	}

	public override void OnEnter()
	{
		DoQuatMult();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		if (everyFrameOption == everyFrameOptions.Update)
		{
			DoQuatMult();
		}
	}

	public override void OnLateUpdate()
	{
		if (everyFrameOption == everyFrameOptions.LateUpdate)
		{
			DoQuatMult();
		}
	}

	public override void OnFixedUpdate()
	{
		if (everyFrameOption == everyFrameOptions.FixedUpdate)
		{
			DoQuatMult();
		}
	}

	private void DoQuatMult()
	{
		result.Value = quaternionA.Value * quaternionB.Value;
	}
}
