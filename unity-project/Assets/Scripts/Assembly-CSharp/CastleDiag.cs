using System.Collections.Generic;
using UnityEngine;

// CastleDiag: tiny on-device readout (Android + editor). Proves touch presses
// land, shows scene/FPS/memory, last error, and recent CastleLogger lines.
// Screenshot it when something breaks. Runtime-created, no scene edits.
public class CastleDiag : MonoBehaviour
{
	private static string lastKey = "-";
	private static string lastError = "";
	private static int errorCount;
	private float fps;
	private string info = "";
	private bool showLog = true;
	private bool lastLoading;
	private int frame;
	private int objCount;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
		if (Application.platform != RuntimePlatform.Android && !Application.isEditor)
		{
			return;
		}
		CastleLogger.Init();
		CastleLogger.Log("diag init, first scene: " + Application.loadedLevelName);
		GameObject go = new GameObject("CastleDiag");
		Object.DontDestroyOnLoad(go);
		go.AddComponent<CastleDiag>();
	}

	public static void NoteKey(string name)
	{
		lastKey = name;
		CastleLogger.Log("touch: " + name);
	}

	private void OnLevelWasLoaded(int level)
	{
		HutongGames.PlayMaker.Actions.LoadLevel2.NotifyLoadComplete();
		CastleLogger.Log("scene loaded: " + Application.loadedLevelName + " (" + level + ") loading:" + (Application.isLoadingLevel ? "1" : "0"));
	}

	private void Update()
	{
		fps = fps * 0.9f + (1f / Mathf.Max(Time.deltaTime, 0.0001f)) * 0.1f;
		info = Application.loadedLevelName + " fps:" + ((int)fps).ToString()
			+ " mem:" + (System.GC.GetTotalMemory(false) / 1048576).ToString() + "MB"
			+ " key:" + lastKey
			+ " objs:" + objCount.ToString()
			+ " loading:" + (Application.isLoadingLevel ? "1" : "0");
		if ((frame++ % 60) == 0)
		{
			objCount = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Length;
		}
		if (Application.isLoadingLevel != lastLoading)
		{
			lastLoading = Application.isLoadingLevel;
			int nobjs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Length;
			objCount = nobjs;
			CastleLogger.Log("loading:" + (lastLoading ? "1" : "0") + " scene:" + Application.loadedLevelName + " objs:" + nobjs.ToString());
		}
		if (errorCount > 0)
		{
			info = info + " ERRx" + errorCount.ToString() + ":" + lastError;
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(8f, 8f, 1200f, 36f), "[CastlePort] " + info);
		if (!showLog)
		{
			return;
		}
		List<string> recent = CastleLogger.Recent();
		int n = recent.Count;
		int from = n > 8 ? n - 8 : 0;
		for (int i = from; i < n; i++)
		{
			GUI.Label(new Rect(8f, 44f + (i - from) * 28f, 1400f, 30f), recent[i]);
		}
	}
}
