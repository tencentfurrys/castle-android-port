using UnityEngine;

public class ReSkin2 : MonoBehaviour
{
	public Sprite[] subSprites;

	private void LateUpdate()
	{
		SpriteRenderer[] components = GetComponents<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in components)
		{
			spriteRenderer.sprite = subSprites[int.Parse(spriteRenderer.sprite.name.Substring(8))];
		}
	}
}
