using UnityEngine;

public class Language_Check : MonoBehaviour
{
	public int Mode_Code;

	public bool Done;

	private void LoadTalk()
	{
		if (Application.systemLanguage == SystemLanguage.Chinese)
		{
			Mode_Code = 2;
		}
		else if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
		{
			Mode_Code = 2;
		}
		else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
		{
			Mode_Code = 2;
		}
		else if (Application.systemLanguage == SystemLanguage.Japanese)
		{
			Mode_Code = 3;
		}
		else
		{
			Mode_Code = 1;
		}
		Done = true;
	}
}
