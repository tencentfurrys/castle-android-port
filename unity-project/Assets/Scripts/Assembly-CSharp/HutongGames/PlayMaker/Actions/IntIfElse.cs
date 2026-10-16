namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Performs comparision on 2 Integers: ==, >,<,>=,<=,half,twice,opposite, OppositeSign. Allow the result to be saved in a FsmBool, and can send events.")]
	[ActionCategory(ActionCategory.Logic)]
	public class IntIfElse : FsmStateAction
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

		[CompoundArray("Ints", "Int 1", "Int 2")]
		[Tooltip("Number of Ifs Must Match Operations & Send Events")]
		[ActionSection("If")]
		[Title("Integer 1")]
		[RequiredField]
		public FsmInt[] ifInt1;

		[RequiredField]
		[Tooltip("The second integer to compare")]
		[Title("Integer 2")]
		public FsmInt[] ifInt2;

		[Tooltip("The comparison method")]
		[Title("Operation")]
		[CompoundArray("Operations & Send Events", "Operation", "Send Event")]
		public Operation[] ifOperation;

		[Title("Send Event")]
		[Tooltip("Event sent if comparison is true")]
		public FsmEvent[] ifEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("The comparison result")]
		public FsmBool result;

		[Title("Send Event")]
		[ActionSection("Else")]
		[Tooltip("Event sent if comparison is false")]
		public FsmEvent elseEvent;

		[Tooltip("Performs comparison everyframe, usefull when value is changing")]
		public bool everyFrame;

		public override void Reset()
		{
			ifInt1 = new FsmInt[1];
			ifInt2 = new FsmInt[1];
			ifOperation = new Operation[1];
			result = null;
			ifEvent = new FsmEvent[1];
			elseEvent = null;
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
			if (ifInt1.Length == 0 || ifOperation.Length == 0 || ifInt1.Length != ifOperation.Length)
			{
				return;
			}
			for (int i = 0; i < ifInt1.Length; i++)
			{
				float num = ifInt1[i].Value;
				float num2 = ifInt2[i].Value;
				bool flag = false;
				switch (ifOperation[i])
				{
				case Operation.Equal:
					flag = num == num2;
					break;
				case Operation.Greater:
					flag = num > num2;
					break;
				case Operation.GreaterOrEqual:
					flag = num >= num2;
					break;
				case Operation.Less:
					flag = num < num2;
					break;
				case Operation.LessOrEqual:
					flag = num <= num2;
					break;
				case Operation.Opposite:
					flag = num == num2 * -1f;
					break;
				case Operation.OppositeSign:
					flag = num * num2 < 0f;
					break;
				}
				result.Value = flag;
				if (flag)
				{
					base.Fsm.Event(ifEvent[i]);
					return;
				}
			}
			base.Fsm.Event(elseEvent);
		}
	}
}
