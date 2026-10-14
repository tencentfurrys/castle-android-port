namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends an Event based on the value of a String Variable.")]
	[ActionCategory(ActionCategory.Logic)]
	public class StringContainSwitch : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		[CompoundArray("String Switches", "Compare String", "Send Event")]
		public FsmString[] compareTo;

		public FsmEvent[] sendEvent;

		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			compareTo = new FsmString[1];
			sendEvent = new FsmEvent[1];
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoStringSwitch();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoStringSwitch();
		}

		private void DoStringSwitch()
		{
			if (stringVariable.IsNone)
			{
				return;
			}
			for (int i = 0; i < compareTo.Length; i++)
			{
				if (stringVariable.Value.Contains(compareTo[i].Value) && sendEvent[i] != null)
				{
					base.Fsm.Event(sendEvent[i]);
					break;
				}
			}
		}
	}
}
