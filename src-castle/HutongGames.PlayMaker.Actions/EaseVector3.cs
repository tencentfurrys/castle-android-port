using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Easing Animation - Vector3")]
[ActionCategory(ActionCategory.AnimateVariables)]
public class EaseVector3 : EaseFsmAction
{
	[RequiredField]
	public FsmVector3 fromValue;

	[RequiredField]
	public FsmVector3 toValue;

	[UIHint(UIHint.Variable)]
	public FsmVector3 vector3Variable;

	private bool finishInNextStep;

	public override void Reset()
	{
		base.Reset();
		vector3Variable = null;
		fromValue = null;
		toValue = null;
		finishInNextStep = false;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		fromFloats = new float[3];
		fromFloats[0] = fromValue.Value.x;
		fromFloats[1] = fromValue.Value.y;
		fromFloats[2] = fromValue.Value.z;
		toFloats = new float[3];
		toFloats[0] = toValue.Value.x;
		toFloats[1] = toValue.Value.y;
		toFloats[2] = toValue.Value.z;
		resultFloats = new float[3];
		finishInNextStep = false;
	}

	public override void OnExit()
	{
		base.OnExit();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (!vector3Variable.IsNone && isRunning)
		{
			vector3Variable.Value = new Vector3(resultFloats[0], resultFloats[1], resultFloats[2]);
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
			if (!vector3Variable.IsNone)
			{
				FsmVector3 fsmVector = vector3Variable;
				float num;
				if (reverse.IsNone)
				{
					num = toValue.Value.x;
				}
				else
				{
					num = ((!reverse.Value) ? toValue.Value.x : fromValue.Value.x);
				}
				FsmVector3 fsmVector2 = fsmVector;
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
				FsmVector3 fsmVector3 = fsmVector2;
				float x = num2;
				float y = num3;
				float z;
				if (reverse.IsNone)
				{
					z = toValue.Value.z;
				}
				else
				{
					z = ((!reverse.Value) ? toValue.Value.z : fromValue.Value.z);
				}
				fsmVector3.Value = new Vector3(x, y, z);
			}
			finishInNextStep = true;
		}
	}
}
