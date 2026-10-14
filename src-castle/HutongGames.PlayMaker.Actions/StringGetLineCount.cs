using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.String)]
[Tooltip("Get the number of lines of a string.")]
public class StringGetLineCount : FsmStateAction
{
	[RequiredField]
	[Tooltip("The string.")]
	[UIHint(UIHint.Variable)]
	public FsmString theString;

	[Tooltip("The number of lines")]
	[RequiredField]
	[UIHint(UIHint.Variable)]
	public FsmInt lineCount;

	public override void Reset()
	{
		theString = null;
		lineCount = 0;
	}

	public override void OnEnter()
	{
		DoGetLineCountFromString();
		Finish();
	}

	private void DoGetLineCountFromString()
	{
		if (theString == null)
		{
			Debug.LogWarning("String not defined");
			return;
		}
		string[] array = theString.Value.Split('\n');
		lineCount.Value = array.Length;
	}
}
