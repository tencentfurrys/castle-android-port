using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Round a Float to the specified nearest value. Optional Int store value.")]
[ActionCategory(ActionCategory.Math)]
public class FloatRoundToNearest : FsmStateAction
{
	[RequiredField]
	[UIHint(UIHint.Variable)]
	public FsmFloat floatVariable;

	public FsmFloat nearest;

	[UIHint(UIHint.Variable)]
	public FsmInt resultAsInt;

	[UIHint(UIHint.Variable)]
	public FsmFloat resultAsFloat;

	public bool everyFrame;

	public override void Reset()
	{
		floatVariable = null;
		nearest = 10f;
		resultAsInt = null;
		resultAsFloat = null;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		DoFloatRound();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoFloatRound();
	}

	private void DoFloatRound()
	{
		resultAsFloat.Value = Mathf.Round(floatVariable.Value / nearest.Value) * nearest.Value;
		if (resultAsInt != null)
		{
			resultAsInt.Value = Mathf.RoundToInt(resultAsFloat.Value);
		}
	}
}
