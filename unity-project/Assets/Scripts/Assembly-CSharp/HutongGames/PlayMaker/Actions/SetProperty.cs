namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.UnityObject)]
	[Tooltip("Sets the value of any public property or field on the targeted Unity Object. E.g., Drag and drop any component attached to a Game Object to access its properties.")]
	public class SetProperty : FsmStateAction
	{
		public FsmProperty targetProperty;

		public bool everyFrame;

		[Tooltip("Perform in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
		public bool lateUpdate;

		public override void Reset()
		{
			targetProperty = new FsmProperty
			{
				setProperty = true
			};
			everyFrame = false;
		}

		public override void OnEnter()
		{
			targetProperty.SetValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (!lateUpdate)
			{
				targetProperty.SetValue();
			}
		}

		public override void OnLateUpdate()
		{
			if (lateUpdate)
			{
				targetProperty.SetValue();
			}
			if (!everyFrame)
			{
				Finish();
			}
		}
	}
}
