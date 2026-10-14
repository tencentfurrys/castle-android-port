namespace HutongGames.PlayMaker.Actions;

[Tooltip("Removes any specified Characters from the given String.")]
[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=15458.0")]
[ActionCategory(ActionCategory.String)]
public class StringRemoveChars : FsmStateAction
{
	[Tooltip("The String to replace each char of.")]
	[RequiredField]
	public FsmString startString;

	[Tooltip("Insert any char that should be removed from the Start String.")]
	[RequiredField]
	public FsmString charsToRemove;

	[UIHint(UIHint.Variable)]
	[Tooltip("Store the final String in a variable. Can be the same as the Start String to override it or a new one to keep them separate and be able to revert these changes.")]
	[RequiredField]
	public FsmString storeResult;

	[Tooltip("If set replaces chars with space instead of removing them.")]
	public FsmBool leaveEmpty;

	[Tooltip("Repeat every frame while the state is active.")]
	public FsmBool everyFrame;

	private char[] tmpChar;

	private string tmpString;

	private string stringToModify;

	public override void Reset()
	{
		startString = null;
		charsToRemove = null;
		storeResult = null;
		leaveEmpty = false;
		everyFrame = false;
		tmpChar = null;
		tmpString = null;
		stringToModify = null;
	}

	public override void OnEnter()
	{
		DoRemove();
		if (!everyFrame.Value)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoRemove();
	}

	private void DoRemove()
	{
		tmpChar = charsToRemove.Value.ToCharArray();
		stringToModify = startString.Value;
		for (int i = 0; i < stringToModify.Length; i++)
		{
			char[] array = tmpChar;
			for (int j = 0; j < array.Length; j++)
			{
				char c = array[j];
				if (leaveEmpty.Value)
				{
					stringToModify = stringToModify.Replace(c.ToString(), " ");
				}
				else
				{
					stringToModify = stringToModify.Replace(c.ToString(), string.Empty);
				}
			}
		}
		storeResult.Value = stringToModify;
	}
}
