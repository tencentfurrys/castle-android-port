using UnityEngine;

public class LoadSprite : MonoBehaviour
{
	public string spriteSheetName;

	public int arrayNumber;

	public SpriteRenderer NowRender;

	public PlayMakerFSM fsm;

	private void LoadImage()
	{
		Sprite[] array = Resources.LoadAll<Sprite>(spriteSheetName);
		if (arrayNumber >= array.Length)
		{
			fsm.SendEvent("Self_Stop");
			return;
		}
		NowRender.sprite = array[arrayNumber];
		fsm.SendEvent("Self_Next");
	}
}
