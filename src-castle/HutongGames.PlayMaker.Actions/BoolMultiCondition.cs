using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Adds multipe float variables to float variable.")]
[ActionCategory(ActionCategory.Logic)]
public class BoolMultiCondition : FsmStateAction
{
	[Tooltip("The variables to evaluate.")]
	[UIHint(UIHint.Variable)]
	public FsmBool[] booleans;

	[Tooltip("The passing conditions for the above variable list. Must be the same count as Booleans.")]
	public FsmBool[] conditions;

	[Tooltip("If the evaluated booleans DO match the conditions, this event can fire.")]
	public FsmEvent passEvent;

	[Tooltip("If the evaluated booleans do NOT match the conditions, this event can fire.")]
	public FsmEvent failEvent;

	[Tooltip("Repeat every frame while the state is active.")]
	public bool everyFrame;

	[Tooltip("Reports clear information about failure events.")]
	public bool debug;

	public override void Reset()
	{
		booleans = null;
		conditions = null;
		passEvent = null;
		failEvent = null;
		everyFrame = false;
		debug = false;
	}

	public override void OnEnter()
	{
		DoEvaluate();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoEvaluate();
	}

	private void DoEvaluate()
	{
		if (conditions.Length != booleans.Length)
		{
			Debug.LogError("<color=red>BoolMultiCondition Action Fatal Error:</color> The number of <i>Booleans</i> and <i>Conditions</i> must be equal!");
			Finish();
			return;
		}
		bool[] array = new bool[booleans.Length];
		for (int i = 0; i < booleans.Length; i++)
		{
			array[i] = booleans[i].Value == conditions[i].Value;
			if (!array[i])
			{
				if (debug)
				{
					Debug.Log(string.Concat("Failure at bool <b>Element ", i, ": (", booleans[i].Name, ")</b> against its pass condition of <b>", conditions[i], ".</b>"));
				}
				Fsm.Event(failEvent);
				if (!everyFrame)
				{
					Finish();
				}
				return;
			}
		}
		if (debug)
		{
			Debug.Log("All " + array.Length + " boolean conditions passed.");
		}
		Fsm.Event(passEvent);
		if (!everyFrame)
		{
			Finish();
		}
	}
}
