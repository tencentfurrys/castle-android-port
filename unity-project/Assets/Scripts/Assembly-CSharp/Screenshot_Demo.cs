using UnityEngine;
using UnityEngine.UI;

public class Screenshot_Demo : MonoBehaviour
{
	public Image target;

	public RawImage target_II;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			target.sprite = ScreenshotSystem.ToSprite();
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			target_II.texture = ScreenshotSystem.ToTexture();
		}
	}
}
