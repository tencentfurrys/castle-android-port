namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Check if a String variable is null or empty and sends Events based on the result.")]
	public class StringIsNullOrEmpty : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		public FsmEvent isNullOrEmptyEvent;

		public FsmEvent isNotNullOrEmptyEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the true/false result in a bool variable.")]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame. Useful if any the string is changing over time.")]
		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			isNullOrEmptyEvent = null;
			isNotNullOrEmptyEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoStringIsNullOrEmpty();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoStringIsNullOrEmpty();
		}

		private void DoStringIsNullOrEmpty()
		{
			bool flag = stringVariable == null || string.IsNullOrEmpty(stringVariable.Value);
			storeResult.Value = flag;
			if (flag)
			{
				base.Fsm.Event(isNullOrEmptyEvent);
			}
			else
			{
				base.Fsm.Event(isNotNullOrEmptyEvent);
			}
		}
	}
}
