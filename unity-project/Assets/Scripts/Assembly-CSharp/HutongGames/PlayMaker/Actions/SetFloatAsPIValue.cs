using System;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the value of a Float Variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class SetFloatAsPIValue : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		private FsmFloat floatValue;

		[RequiredField]
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			floatValue = (float)Math.PI;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			floatVariable.Value = floatValue.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			floatVariable.Value = floatValue.Value;
		}
	}
}
