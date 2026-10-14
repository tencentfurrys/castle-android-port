using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Math)]
[Tooltip("Clamps the value of Float Variable to a Min/Max. This advanced versrion let you choose between the different types of updates")]
public class FloatClampAdvanced : FsmStateActionAdvanced
{
	[Tooltip("Float variable to clamp.")]
	[UIHint(UIHint.Variable)]
	[RequiredField]
	public FsmFloat floatVariable;

	[Tooltip("The minimum value.")]
	[RequiredField]
	public FsmFloat minValue;

	[Tooltip("The maximum value.")]
	[RequiredField]
	public FsmFloat maxValue;

	public override void Reset()
	{
		base.Reset();
		floatVariable = null;
		minValue = null;
		maxValue = null;
	}

	public override void OnActionUpdate()
	{
		DoClamp();
	}

	private void DoClamp()
	{
		floatVariable.Value = Mathf.Clamp(floatVariable.Value, minValue.Value, maxValue.Value);
	}
}
