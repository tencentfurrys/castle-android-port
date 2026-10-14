using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Advanced interpolation between 2 floats.")]
[ActionCategory(ActionCategory.Math)]
public class FloatLerpAdvanced : FsmStateAction
{
	public enum LerpInterpolationType
	{
		Linear,
		Quadratic,
		EaseIn,
		EaseOut,
		Smoothstep,
		Smootherstep,
		DeltaTime
	}

	[RequiredField]
	[Tooltip("First Vector.")]
	public FsmFloat fromFloat;

	[Tooltip("Second Vector.")]
	[RequiredField]
	public FsmFloat toFloat;

	[HasFloatSlider(-1f, 1f)]
	[Tooltip("Interpolate between From Vector and ToVector by this amount. Value is clamped to -1 1 range. interpolation can be choosen below")]
	[RequiredField]
	public FsmFloat amount;

	public LerpInterpolationType interpolation;

	[RequiredField]
	[UIHint(UIHint.Variable)]
	[Tooltip("Store the result in this float variable.")]
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
		interpolation = LerpInterpolationType.Linear;
		storeResult = null;
		everyFrame = true;
	}

	public override void OnEnter()
	{
		DoLerp();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoLerp();
	}

	private void DoLerp()
	{
		float value = amount.Value;
		float num = ((!(value < 0f)) ? 1 : (-1));
		value = GetInterpolation(Mathf.Abs(value), interpolation);
		storeResult.Value = num * Mathf.Lerp(fromFloat.Value, toFloat.Value, value);
	}

	private float GetInterpolation(float t, LerpInterpolationType type)
	{
		return type switch
		{
			LerpInterpolationType.Quadratic => t * t, 
			LerpInterpolationType.EaseIn => 1f - Mathf.Cos(t * (float)Math.PI * 0.5f), 
			LerpInterpolationType.EaseOut => Mathf.Sin(t * (float)Math.PI * 0.5f), 
			LerpInterpolationType.Smoothstep => t * t * (3f - 2f * t), 
			LerpInterpolationType.Smootherstep => t * t * t * (t * (6f * t - 15f) + 10f), 
			LerpInterpolationType.DeltaTime => Time.deltaTime * t, 
			_ => t, 
		};
	}
}
