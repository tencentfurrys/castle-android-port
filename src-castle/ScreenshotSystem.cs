using System.IO;
using UnityEngine;

public class ScreenshotSystem : MonoBehaviour
{
	private Camera camera;

	private Texture2D tempTexture2D;

	public static ScreenshotSystem instance;

	private Rect rect;

	private void Awake()
	{
		instance = this;
		camera = GetComponent<Camera>();
		rect = new Rect(0f, 0f, Screen.width, Screen.height);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public static Sprite ToSprite()
	{
		Sprite sprite = new Sprite();
		if (instance == null)
		{
			Debug.LogError("\"ScreenshotSystem\" Component doesn't in any Camera");
			return null;
		}
		return sprite = instance.M_ToSprite();
	}

	public Sprite M_ToSprite()
	{
		tempTexture2D = CaptureCamera(camera, rect, p_saveToLocal: false);
		return Sprite.Create(tempTexture2D, new Rect(0f, 0f, tempTexture2D.width, tempTexture2D.height), Vector2.zero);
	}

	public static Texture ToTexture()
	{
		Texture texture = new Texture();
		if (instance == null)
		{
			Debug.LogError("\"ScreenshotSystem\" Component doesn't in any Camera");
			return null;
		}
		return texture = instance.M_ToTexture();
	}

	public Texture M_ToTexture()
	{
		tempTexture2D = CaptureCamera(camera, rect, p_saveToLocal: false);
		return tempTexture2D;
	}

	private Texture2D CaptureCamera(Camera camera, Rect rect, bool p_saveToLocal)
	{
		RenderTexture renderTexture = (camera.targetTexture = new RenderTexture((int)rect.width, (int)rect.height, 0));
		camera.Render();
		RenderTexture.active = renderTexture;
		Texture2D texture2D = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, mipmap: false);
		texture2D.ReadPixels(rect, 0, 0);
		texture2D.Apply();
		camera.targetTexture = null;
		RenderTexture.active = null;
		Object.Destroy(renderTexture);
		byte[] bytes = texture2D.EncodeToPNG();
		string path = Application.dataPath + "/Screenshot.png";
		if (p_saveToLocal)
		{
			File.WriteAllBytes(path, bytes);
		}
		return texture2D;
	}
}
