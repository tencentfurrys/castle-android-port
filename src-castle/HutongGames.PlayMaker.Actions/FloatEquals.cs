using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=9910")]
[Tooltip("Sends Events based on the comparison of multiple Floats.")]
[ActionCategory(ActionCategory.Logic)]
public class FloatEquals : FsmStateAction
{
	[Tooltip("Main float to compare to")]
	[ActionSection("Main float")]
	public FsmFloat keyFloat;

	[RequiredField]
	[ActionSection("Floats to compare with")]
	public FsmFloat[] floats;

	[ActionSection("Simple Tolerance")]
	[UIHint(UIHint.Description)]
	public string descriptionArea = "on";

	[RequiredField]
	[Tooltip("Tolerance for the Equal test (almost equal). 0 =off (will use the advance tolerance instead) / 1+ =on ")]
	public FsmFloat tolerance;

	[Tooltip("Tolerance above(+) the main number.")]
	[UIHint(UIHint.Description)]
	[ActionSection("Advance Tolerance")]
	public FsmFloat tolerancePositive;

	[Tooltip("Tolerance below(-) the main number.")]
	public FsmFloat toleranceNegative;

	[UIHint(UIHint.Variable)]
	[Tooltip("Set bool to true if all floats equal")]
	[ActionSection("Set bools")]
	public FsmBool isEqual;

	[Tooltip("Set bool to true if floats are not equal")]
	[UIHint(UIHint.Variable)]
	public FsmBool isNotEqual;

	[ActionSection("Events")]
	[Tooltip("Event sent if all floats equal")]
	public FsmEvent equal;

	[Tooltip("Event sent if floats are not equal")]
	public FsmEvent notEqual;

	[ActionSection("")]
	public bool everyFrame;

	public override void Reset()
	{
		floats = new FsmFloat[1];
		keyFloat = 0f;
		tolerance = 0f;
		tolerancePositive = 0f;
		toleranceNegative = 0f;
		isEqual = false;
		isNotEqual = false;
		equal = null;
		notEqual = null;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		DoFloatCompare();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoFloatCompare();
	}

	private void DoFloatCompare()
	{
		if (floats.Length < 1)
		{
			return;
		}
		float value = keyFloat.Value;
		if (tolerance.Value > 0f)
		{
			descriptionArea = "on";
			FsmFloat[] array = floats;
			foreach (FsmFloat fsmFloat in array)
			{
				if (!(Mathf.Abs(value - fsmFloat.Value) <= tolerance.Value))
				{
					isEqual.Value = false;
					isNotEqual.Value = true;
					Fsm.Event(notEqual);
					return;
				}
			}
		}
		else if (tolerance.Value == 0f)
		{
			descriptionArea = "off";
			FsmFloat[] array2 = floats;
			foreach (FsmFloat fsmFloat2 in array2)
			{
				if (Mathf.Abs(value - fsmFloat2.Value) == 0f)
				{
					continue;
				}
				if (!(value - fsmFloat2.Value > 0f))
				{
					if (!(Mathf.Abs(value - fsmFloat2.Value) <= tolerancePositive.Value))
					{
						isEqual.Value = false;
						isNotEqual.Value = true;
						Fsm.Event(notEqual);
						return;
					}
				}
				else if (value - fsmFloat2.Value > 0f && !(value - fsmFloat2.Value <= toleranceNegative.Value))
				{
					isEqual.Value = false;
					isNotEqual.Value = true;
					Fsm.Event(notEqual);
					return;
				}
			}
		}
		isEqual.Value = true;
		isNotEqual.Value = false;
		Fsm.Event(equal);
	}

	public override string ErrorCheck()
	{
		if (floats.Length < 1)
		{
			return "Action needs more than 1 float to compare";
		}
		return string.Empty;
	}
}
