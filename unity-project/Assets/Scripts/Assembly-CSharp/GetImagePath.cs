using UnityEngine;

public class GetImagePath : MonoBehaviour
{
	public string spriteSheetName;

	public int arrayNumber;

	private void LateUpdate()
	{
		Sprite[] array = Resources.LoadAll<Sprite>(spriteSheetName);
		GetComponent<SpriteRenderer>().sprite = array[arrayNumber];
	}
}
