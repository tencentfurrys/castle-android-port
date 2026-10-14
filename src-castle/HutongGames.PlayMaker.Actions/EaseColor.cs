using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Easing Animation - Color")]
[ActionCategory(ActionCategory.AnimateVariables)]
public class EaseColor : EaseFsmAction
{
	[RequiredField]
	public FsmColor fromValue;

	[RequiredField]
	public FsmColor toValue;

	[UIHint(UIHint.Variable)]
	public FsmColor colorVariable;

	private bool finishInNextStep;

	public override void Reset()
	{
		base.Reset();
		colorVariable = null;
		fromValue = null;
		toValue = null;
		finishInNextStep = false;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		fromFloats = new float[4];
		fromFloats[0] = fromValue.Value.r;
		fromFloats[1] = fromValue.Value.g;
		fromFloats[2] = fromValue.Value.b;
		fromFloats[3] = fromValue.Value.a;
		toFloats = new float[4];
		toFloats[0] = toValue.Value.r;
		toFloats[1] = toValue.Value.g;
		toFloats[2] = toValue.Value.b;
		toFloats[3] = toValue.Value.a;
		resultFloats = new float[4];
		finishInNextStep = false;
	}

	public override void OnExit()
	{
		base.OnExit();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (!colorVariable.IsNone && isRunning)
		{
			colorVariable.Value = new Color(resultFloats[0], resultFloats[1], resultFloats[2], resultFloats[3]);
		}
		if (finishInNextStep)
		{
			Finish();
			if (finishEvent != null)
			{
				Fsm.Event(finishEvent);
			}
		}
		if (finishAction && !finishInNextStep)
		{
			if (!colorVariable.IsNone)
			{
				FsmColor fsmColor = colorVariable;
				float num;
				if (reverse.IsNone)
				{
					num = toValue.Value.r;
				}
				else
				{
					num = ((!reverse.Value) ? toValue.Value.r : fromValue.Value.r);
				}
				FsmColor fsmColor2 = fsmColor;
				float num2 = num;
				float num3;
				if (reverse.IsNone)
				{
					num3 = toValue.Value.g;
				}
				else
				{
					num3 = ((!reverse.Value) ? toValue.Value.g : fromValue.Value.g);
				}
				FsmColor fsmColor3 = fsmColor2;
				float num4 = num2;
				float num5 = num3;
				float num6;
				if (reverse.IsNone)
				{
					num6 = toValue.Value.b;
				}
				else
				{
					num6 = ((!reverse.Value) ? toValue.Value.b : fromValue.Value.b);
				}
				FsmColor fsmColor4 = fsmColor3;
				float r = num4;
				float g = num5;
				float b = num6;
				float a;
				if (reverse.IsNone)
				{
					a = toValue.Value.a;
				}
				else
				{
					a = ((!reverse.Value) ? toValue.Value.a : fromValue.Value.a);
				}
				fsmColor4.Value = new Color(r, g, b, a);
			}
			finishInNextStep = true;
		}
	}
}
