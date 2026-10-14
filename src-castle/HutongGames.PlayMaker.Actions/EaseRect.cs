using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("AnimateVariables")]
[Tooltip("Easing Animation - Rect.")]
public class EaseRect : EaseFsmAction
{
	[RequiredField]
	public FsmRect fromValue;

	[RequiredField]
	public FsmRect toValue;

	[UIHint(UIHint.Variable)]
	public FsmRect rectVariable;

	private bool finishInNextStep;

	public override void Reset()
	{
		base.Reset();
		rectVariable = null;
		fromValue = null;
		toValue = null;
		finishInNextStep = false;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		fromFloats = new float[4];
		fromFloats[0] = fromValue.Value.x;
		fromFloats[1] = fromValue.Value.y;
		fromFloats[2] = fromValue.Value.width;
		fromFloats[3] = fromValue.Value.height;
		toFloats = new float[4];
		toFloats[0] = toValue.Value.x;
		toFloats[1] = toValue.Value.y;
		toFloats[2] = toValue.Value.width;
		toFloats[3] = toValue.Value.height;
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
		if (!rectVariable.IsNone && isRunning)
		{
			rectVariable.Value = new Rect(resultFloats[0], resultFloats[1], resultFloats[2], resultFloats[3]);
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
			if (!rectVariable.IsNone)
			{
				FsmRect fsmRect = rectVariable;
				float num;
				if (reverse.IsNone)
				{
					num = toValue.Value.x;
				}
				else
				{
					num = ((!reverse.Value) ? toValue.Value.x : fromValue.Value.x);
				}
				FsmRect fsmRect2 = fsmRect;
				float num2 = num;
				float num3;
				if (reverse.IsNone)
				{
					num3 = toValue.Value.y;
				}
				else
				{
					num3 = ((!reverse.Value) ? toValue.Value.y : fromValue.Value.y);
				}
				FsmRect fsmRect3 = fsmRect2;
				float num4 = num2;
				float num5 = num3;
				float num6;
				if (reverse.IsNone)
				{
					num6 = toValue.Value.width;
				}
				else
				{
					num6 = ((!reverse.Value) ? toValue.Value.width : fromValue.Value.width);
				}
				FsmRect fsmRect4 = fsmRect3;
				float x = num4;
				float y = num5;
				float width = num6;
				float height;
				if (reverse.IsNone)
				{
					height = toValue.Value.height;
				}
				else
				{
					height = ((!reverse.Value) ? toValue.Value.height : fromValue.Value.height);
				}
				fsmRect4.Value = new Rect(x, y, width, height);
			}
			finishInNextStep = true;
		}
	}
}
