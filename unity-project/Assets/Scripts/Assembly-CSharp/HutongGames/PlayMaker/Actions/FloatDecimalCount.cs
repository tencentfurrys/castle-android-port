using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=11776.0")]
	[Tooltip("Count Float decimals into an Int")]
	public class FloatDecimalCount : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		[Tooltip("Count Result")]
		public FsmInt decimalNumberCount;

		private int decimalCount;

		public override void Reset()
		{
			floatVariable = null;
			decimalNumberCount = null;
		}

		public override void OnEnter()
		{
			decimal d = (decimal)floatVariable.Value;
			decimalCount = GetDecimals(d);
			decimalNumberCount.Value = decimalCount;
			Finish();
		}

		private int GetDecimals(decimal d, int i = 0)
		{
			decimal num = (decimal)((double)d * Math.Pow(10.0, i));
			if (Math.Round(num) == num)
			{
				return i;
			}
			return GetDecimals(d, i + 1);
		}
	}
}
