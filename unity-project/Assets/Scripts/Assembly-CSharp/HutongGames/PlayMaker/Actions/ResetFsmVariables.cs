namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Resets the variables to defaults")]
	[ActionCategory(ActionCategory.Math)]
	public sealed class ResetFsmVariables : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmVar[] variables;

		public override void Reset()
		{
			variables = new FsmVar[1];
		}

		public override void OnEnter()
		{
			FsmVar[] array = variables;
			foreach (FsmVar fsmVar in array)
			{
				PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, fsmVar, null);
			}
			Finish();
		}
	}
}
