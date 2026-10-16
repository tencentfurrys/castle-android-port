using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Advanced interpolation between 2 floats.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatLerpAdvanced : FsmStateAction
	{
		public enum LerpInterpolationType
		{
			Linear = 0,
			Quadratic = 1,
			EaseIn = 2,
			EaseOut = 3,
			Smoothstep = 4,
			Smootherstep = 5,
			DeltaTime = 6
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
			switch (type)
			{
			case LerpInterpolationType.Quadratic:
				return t * t;
			case LerpInterpolationType.EaseIn:
				return 1f - Mathf.Cos(t * (float)Math.PI * 0.5f);
			case LerpInterpolationType.EaseOut:
				return Mathf.Sin(t * (float)Math.PI * 0.5f);
			case LerpInterpolationType.Smoothstep:
				return t * t * (3f - 2f * t);
			case LerpInterpolationType.Smootherstep:
				return t * t * t * (t * (6f * t - 15f) + 10f);
			case LerpInterpolationType.DeltaTime:
				return Time.deltaTime * t;
			default:
				return t;
			}
		}
	}
}
