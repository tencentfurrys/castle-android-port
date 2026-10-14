using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Math)]
[Tooltip("Linearly interpolates between 2 floats. It has an option to lerp against deltaTime for the amount, allowing framerate indepedant animations.")]
public class FloatLerp : FsmStateAction
{
	[RequiredField]
	[Tooltip("First Float")]
	public FsmFloat fromFloat;

	[Tooltip("Second Float.")]
	[RequiredField]
	public FsmFloat toFloat;

	[Tooltip("Interpolate between FromFloat and ToFloat by this amount. Value is clamped to 0-1 range. 0 = FromFloat; 1 = ToFloat; 0.5 = half way in between.")]
	[RequiredField]
	public FsmFloat amount;

	[Tooltip("Amount is multiplied by the deltatime")]
	public bool lerpAgainstDeltaTime;

	[UIHint(UIHint.Variable)]
	[Tooltip("Store the result in this float variable.")]
	[RequiredField]
	public FsmFloat storeResult;

	[Tooltip("Repeat every frame. Useful if any of the values are changing.")]
	public bool everyFrame;

	public override void Reset()
	{
		fromFloat = new FsmFloat
		{
			UseVariable = true
		};
		toFloat = new FsmFloat
		{
			UseVariable = true
		};
		amount = 0.5f;
		lerpAgainstDeltaTime = false;
		storeResult = null;
		everyFrame = true;
	}

	public override void OnEnter()
	{
		DoFloatLerp();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoFloatLerp();
	}

	private void DoFloatLerp()
	{
		float t = ((!lerpAgainstDeltaTime) ? amount.Value : (Time.deltaTime * amount.Value));
		storeResult.Value = Mathf.Lerp(fromFloat.Value, toFloat.Value, t);
	}
}
