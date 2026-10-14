using UnityEngine;

// CastleDiag: tiny on-device readout (Android only). Proves touch presses land,
// shows scene/FPS/memory for remote debugging. Runtime-created, no scene edits.
public class CastleDiag : MonoBehaviour
{
	private static string lastKey = "-";
	private static float lastKeyTime;
	private static string lastError = "";
	private static int errorCount;
	private float fps;
	private string info = "";

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		GameObject go = new GameObject("CastleDiag");
		Object.DontDestroyOnLoad(go);
		go.AddComponent<CastleDiag>();
		Application.logMessageReceived += OnLog;
	}

	private static void OnLog(string message, string stackTrace, LogType type)
	{
		if (type == LogType.Error || type == LogType.Exception)
		{
			errorCount++;
			lastError = message;
			if (lastError.Length > 160)
			{
				lastError = lastError.Substring(0, 160);
			}
		}
	}

	public static void NoteKey(string name)
	{
		lastKey = name;
		lastKeyTime = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		fps = fps * 0.9f + (1f / Mathf.Max(Time.deltaTime, 0.0001f)) * 0.1f;
		info = Application.loadedLevelName + " fps:" + ((int)fps).ToString()
			+ " mem:" + (System.GC.GetTotalMemory(false) / 1048576).ToString() + "MB"
			+ " key:" + lastKey;
		if (errorCount > 0)
		{
			info = info + " ERRx" + errorCount.ToString() + ":" + lastError;
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(8f, 8f, 900f, 40f), "[CastlePort] " + info);
	}
}
