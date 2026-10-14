namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Game Object that owns the FSM and stores it in a game object variable.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetOwner : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;

		public override void Reset()
		{
			storeGameObject = null;
		}

		public override void OnEnter()
		{
			storeGameObject.Value = base.Owner;
			Finish();
		}
	}
}
