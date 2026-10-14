using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get a Float Variable square root value")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatSquareRoot : FsmStateAction
	{
		public FsmFloat floatVariable;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat result;

		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			result = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFloatSquareRoot();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFloatSquareRoot();
		}

		private void DoFloatSquareRoot()
		{
			if (!result.IsNone)
			{
				result.Value = Mathf.Sqrt(floatVariable.Value);
			}
		}
	}
}
