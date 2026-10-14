using UnityEngine;

public class ReSkinAnimation : MonoBehaviour
{
	public string spriteSheetName;

	public int arrayNumber;

	public string LastSheetName;

	private int LastarrayNumber;

	public SpriteRenderer NowRender;

	private void LateUpdate()
	{
		if (LastSheetName != spriteSheetName || LastarrayNumber != arrayNumber)
		{
			LastSheetName = spriteSheetName;
			LastarrayNumber = arrayNumber;
			Sprite[] array = Resources.LoadAll<Sprite>(spriteSheetName);
			NowRender.sprite = array[arrayNumber];
		}
	}
}
