using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Returns the smallest integer greater to or equal to f.")]
	public class FloatCeiling : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The Float variable to process.")]
		[RequiredField]
		public FsmFloat floatVariable;

		[Tooltip("The float result.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatResult;

		[UIHint(UIHint.Variable)]
		[Tooltip("The int result.")]
		public FsmInt intResult;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			floatResult = null;
			intResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoMath();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMath();
		}

		private void DoMath()
		{
			if (!floatResult.IsNone)
			{
				floatResult.Value = Mathf.Ceil(floatVariable.Value);
			}
			if (!intResult.IsNone)
			{
				intResult.Value = Mathf.CeilToInt(floatVariable.Value);
			}
		}
	}
}
