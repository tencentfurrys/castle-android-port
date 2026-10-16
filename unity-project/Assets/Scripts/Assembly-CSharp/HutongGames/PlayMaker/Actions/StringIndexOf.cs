namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Tests if a String contains another String.")]
	[ActionCategory(ActionCategory.Logic)]
	public class StringIndexOf : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The String variable to test.")]
		[RequiredField]
		public FsmString stringVariable;

		[Tooltip("Test if the String variable contains this string.")]
		[RequiredField]
		public FsmString containsString;

		public FsmInt IndexNum;

		public FsmInt StartNum;

		[Tooltip("Event to send if true.")]
		public FsmEvent trueEvent;

		[Tooltip("Event to send if false.")]
		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the true/false result in a bool variable.")]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame. Useful if any of the strings are changing over time.")]
		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			containsString = string.Empty;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
			IndexNum = 0;
			StartNum = 0;
		}

		public override void OnEnter()
		{
			DoStringContains();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoStringContains();
		}

		private void DoStringContains()
		{
			if (!stringVariable.IsNone && !containsString.IsNone)
			{
				bool flag = stringVariable.Value.Contains(containsString.Value);
				IndexNum.Value = stringVariable.Value.IndexOf(containsString.Value, StartNum.Value);
				if (storeResult != null)
				{
					storeResult.Value = flag;
				}
				if (flag && trueEvent != null)
				{
					base.Fsm.Event(trueEvent);
				}
				else if (!flag && falseEvent != null)
				{
					base.Fsm.Event(falseEvent);
				}
			}
		}
	}
}
