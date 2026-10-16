namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get this FSM name.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmName : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the full label of the FSM.")]
		public FsmString fullLabel;

		[Tooltip("Store the name of the FSM.")]
		[UIHint(UIHint.Variable)]
		public FsmString name;

		public override void OnEnter()
		{
			fullLabel.Value = Fsm.GetFullFsmLabel(base.Fsm);
			name.Value = base.Fsm.Name;
			Finish();
		}
	}
}
