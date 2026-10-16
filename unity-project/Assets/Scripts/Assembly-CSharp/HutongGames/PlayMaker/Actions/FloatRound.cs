using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a Float Variable to its round value. You can store the round value as a float or as an int")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatRound : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		[UIHint(UIHint.Variable)]
		public FsmInt resultAsInt;

		[UIHint(UIHint.Variable)]
		public FsmFloat resultAsFloat;

		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			resultAsInt = null;
			resultAsFloat = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFloatRound();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFloatRound();
		}

		private void DoFloatRound()
		{
			if (!resultAsInt.IsNone)
			{
				resultAsInt.Value = Mathf.RoundToInt(floatVariable.Value);
			}
			if (!resultAsFloat.IsNone)
			{
				resultAsFloat.Value = Mathf.Round(floatVariable.Value);
			}
		}
	}
}
