using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Delete a specificy line from a string. use -1 to delete the last line. else line 0 is the first line, etc")]
	[ActionCategory(ActionCategory.String)]
	public class StringDeleteLine : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The string.")]
		public FsmString theString;

		[Tooltip("The line to delete.")]
		[RequiredField]
		public FsmInt lineIndex;

		public override void Reset()
		{
			theString = null;
			lineIndex = -1;
		}

		public override void OnEnter()
		{
			DoDeleteLineFromString();
			Finish();
		}

		private void DoDeleteLineFromString()
		{
			if (theString == null)
			{
				Debug.LogWarning("String not defined");
				return;
			}
			string value = theString.Value;
			string text = string.Empty;
			int num = lineIndex.Value;
			string[] array = value.Split('\n');
			int num2 = array.Length;
			if (lineIndex.Value == -1)
			{
				num = num2 - 1;
			}
			if (num >= num2)
			{
				return;
			}
			for (int i = 0; i < num2; i++)
			{
				if (i != num)
				{
					text += array[i];
					if (i < num2 - 1)
					{
						text += "\n";
					}
				}
			}
			theString.Value = text;
		}
	}
}
