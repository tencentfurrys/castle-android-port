using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Find the modulo between two floats dividend % diviser.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatModulo : FsmStateAction
	{
		public FsmFloat dividend;

		public FsmFloat diviser;

		[UIHint(UIHint.Variable)]
		public FsmFloat result;

		public bool everyFrame;

		public override void Reset()
		{
			dividend = null;
			diviser = null;
			result = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoModulo();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoModulo();
		}

		private void DoModulo()
		{
			try
			{
				result.Value = dividend.Value % diviser.Value;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("Float Modulo error: " + ex);
			}
		}
	}
}
