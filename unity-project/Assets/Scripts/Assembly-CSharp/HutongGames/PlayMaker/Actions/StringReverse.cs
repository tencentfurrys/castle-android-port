using System;

namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=12369.0")]
	[Tooltip("Reverse a string")]
	[ActionCategory(ActionCategory.String)]
	public class StringReverse : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		[UIHint(UIHint.Variable)]
		public FsmString storeResult;

		public override void Reset()
		{
			stringVariable = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			storeResult.Value = Reverse(stringVariable.Value);
			Finish();
		}

		public string Reverse(string text)
		{
			if (text == null)
			{
				return null;
			}
			char[] array = text.ToCharArray();
			Array.Reverse(array);
			return new string(array);
		}
	}
}
