using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Performs comparision on 2 Integers: ==, >,<,>=,<=,half,twice,opposite, OppositeSign. Allow the result to be saved in a FsmBool, and can send events.")]
	public class IntCompare2 : FsmStateAction
	{
		public enum Operation
		{
			Equal = 0,
			Greater = 1,
			GreaterOrEqual = 2,
			Less = 3,
			LessOrEqual = 4,
			OppositeSign = 5,
			Opposite = 6,
			Half = 7,
			Double = 8
		}

		[RequiredField]
		[Tooltip("The first integer to compare")]
		public FsmInt integer1;

		[RequiredField]
		[Tooltip("The second integer to compare")]
		public FsmInt integer2;

		[Tooltip("The comparison method")]
		public Operation operation;

		[UIHint(UIHint.Variable)]
		[Tooltip("The comparison result")]
		public FsmBool result;

		[Tooltip("Event sent if comparison is true")]
		public FsmEvent comparisonPassEvent;

		[Tooltip("Event sent if comparison is false")]
		public FsmEvent comparisonFailEvent;

		[Tooltip("Performs comparison everyframe, usefull when value is changing")]
		public bool everyFrame;

		public override void Reset()
		{
			integer1 = null;
			integer2 = null;
			operation = Operation.Equal;
			result = null;
			comparisonPassEvent = null;
			comparisonFailEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIntCompare();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIntCompare();
		}

		private void DoIntCompare()
		{
			int value = integer1.Value;
			int value2 = integer2.Value;
			bool flag = false;
			switch (operation)
			{
			case Operation.Equal:
				flag = value == value2;
				break;
			case Operation.Greater:
				flag = value > value2;
				break;
			case Operation.GreaterOrEqual:
				flag = value >= value2;
				break;
			case Operation.Less:
				flag = value < value2;
				break;
			case Operation.LessOrEqual:
				flag = value <= value2;
				break;
			case Operation.Opposite:
				flag = value == value2 * -1;
				break;
			case Operation.OppositeSign:
				flag = value * value2 < 0;
				break;
			}
			if (integer1 == null)
			{
				Debug.Log("integer1 is null");
			}
			result.Value = flag;
			if (flag)
			{
				base.Fsm.Event(comparisonPassEvent);
			}
			else
			{
				base.Fsm.Event(comparisonFailEvent);
			}
		}
	}
}
