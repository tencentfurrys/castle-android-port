namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts a Float value to a String value with optional format.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertFloatToString : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The float variable to convert.")]
		[RequiredField]
		public FsmFloat floatVariable;

		[UIHint(UIHint.Variable)]
		[Tooltip("A string variable to store the converted value.")]
		[RequiredField]
		public FsmString stringVariable;

		[Tooltip("Optional Format, allows for leading zeroes. E.g., 0000")]
		public FsmString format;

		[Tooltip("Repeat every frame. Useful if the float variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			stringVariable = null;
			everyFrame = false;
			format = null;
		}

		public override void OnEnter()
		{
			DoConvertFloatToString();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertFloatToString();
		}

		private void DoConvertFloatToString()
		{
			if (format.IsNone || string.IsNullOrEmpty(format.Value))
			{
				stringVariable.Value = floatVariable.Value.ToString();
			}
			else
			{
				stringVariable.Value = floatVariable.Value.ToString(format.Value);
			}
		}
	}
}
