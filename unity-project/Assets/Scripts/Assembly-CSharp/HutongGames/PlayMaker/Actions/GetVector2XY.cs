namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector2)]
	[Tooltip("Get the XYZ channels of a Vector2 Variable and storew them in Float Variables.")]
	public class GetVector2XY : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector2 vector2Variable;

		[UIHint(UIHint.Variable)]
		public FsmFloat storeX;

		[UIHint(UIHint.Variable)]
		public FsmFloat storeY;

		public bool everyFrame;

		public override void Reset()
		{
			vector2Variable = null;
			storeX = null;
			storeY = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetVector2XY();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetVector2XY();
		}

		private void DoGetVector2XY()
		{
			if (vector2Variable != null)
			{
				if (storeX != null)
				{
					storeX.Value = vector2Variable.Value.x;
				}
				if (storeY != null)
				{
					storeY.Value = vector2Variable.Value.y;
				}
			}
		}
	}
}
