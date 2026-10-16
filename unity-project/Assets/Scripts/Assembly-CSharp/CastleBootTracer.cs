using System;
using System.Collections.Generic;
using UnityEngine;

// CastleBootTracer (p12t): frame-level boot checkpoints for the 5.6 build (diagnostic).
// Writes into the same CastlePort.log via CastleLogger. Purpose: when the app
// dies with a native crash (no managed error), the last tracer line tells us
// exactly which boot step killed it:
//   beforeSceneLoad -> awake -> afterSceneLoad -> start (cams/gpu info)
//   -> preRender cam=X -> postRender cam=X (frame complete) -> frame N ...
// Also dumps SystemInfo (GPU/GL version) and captures threaded engine logs.
// Diagnostic only - remove before a final release build.
public class CastleBootTracer : MonoBehaviour
{
	private static CastleBootTracer instance;
	private static int frameCounter;
	private static int renderEvents;
	private static readonly Queue<string> pending = new Queue<string>();
	private const string TAG = "p13t";

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void BootBefore()
	{
		try
		{
			CastleLogger.Init(); // make sure the file logger is up first
			CastleLogger.Log("tracer[" + TAG + "]: beforeSceneLoad platform=" + Application.platform);
		}
		catch (Exception)
		{
		}
		try
		{
			GameObject go = new GameObject("CastleBootTracer");
			UnityEngine.Object.DontDestroyOnLoad(go);
			instance = go.AddComponent<CastleBootTracer>();
		}
		catch (Exception e)
		{
			SafeLog("tracer: create failed " + e.Message);
		}
		try
		{
			Camera.onPreRender += OnCamPre;
			Camera.onPostRender += OnCamPost;
			Application.logMessageReceivedThreaded += OnThreadedLog;
		}
		catch (Exception)
		{
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void BootAfter()
	{
		SafeLog("tracer: afterSceneLoad scene=" + Application.loadedLevelName);
	}

	private static void OnCamPre(Camera c)
	{
		if (renderEvents < 32)
		{
			renderEvents++;
			SafeLog("tracer: preRender cam=" + ((c != null) ? c.name : "null"));
		}
	}

	private static void OnCamPost(Camera c)
	{
		if (renderEvents < 32)
		{
			renderEvents++;
			SafeLog("tracer: postRender cam=" + ((c != null) ? c.name : "null"));
		}
	}

	private static void OnThreadedLog(string message, string stack, LogType type)
	{
		if (type == LogType.Log)
		{
			return;
		}
		string m = message;
		if (m != null && m.Length > 120)
		{
			m = m.Substring(0, 120);
		}
		lock (pending)
		{
			pending.Enqueue("tracer: T/" + type + ": " + m);
		}
	}

	private void Awake()
	{
		SafeLog("tracer: awake");
	}

	private void Start()
	{
		try
		{
			Camera[] cams = Camera.allCameras;
			string names = "";
			for (int i = 0; i < cams.Length && i < 8; i++)
			{
				names = names + ((i > 0) ? "," : "") + cams[i].name;
			}
			SafeLog("tracer: start scene=" + Application.loadedLevelName
				+ " cams=" + cams.Length + "[" + names + "]"
				+ " gpu=" + SystemInfo.graphicsDeviceName
				+ " gl=" + SystemInfo.graphicsDeviceVersion
				+ " shaderLvl=" + SystemInfo.graphicsShaderLevel
				+ " os=" + SystemInfo.operatingSystem
				+ " screen=" + Screen.width + "x" + Screen.height);
		}
		catch (Exception)
		{
		}
		StartCoroutine(Frames());
	}

	private System.Collections.IEnumerator Frames()
	{
		while (true)
		{
			yield return null;
			frameCounter++;
			string[] flushLines = null;
			lock (pending)
			{
				if (pending.Count > 0)
				{
					flushLines = pending.ToArray();
					pending.Clear();
				}
			}
			if (flushLines != null)
			{
				for (int i = 0; i < flushLines.Length; i++)
				{
					SafeLog(flushLines[i]);
				}
			}
			if (frameCounter <= 60 || (frameCounter <= 300 && (frameCounter % 10) == 0) || (frameCounter % 300) == 0)
			{
				SafeLog("tracer: frame " + frameCounter
					+ " scene=" + Application.loadedLevelName
					+ " loading=" + (Application.isLoadingLevel ? "1" : "0")
					+ " time=" + Time.realtimeSinceStartup.ToString("F1"));
			}
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		SafeLog("tracer: focus=" + focus);
	}

	private void OnApplicationPause(bool pause)
	{
		SafeLog("tracer: pause=" + pause);
	}

	private void OnApplicationQuit()
	{
		SafeLog("tracer: quit");
	}

	private void OnDestroy()
	{
		SafeLog("tracer: destroyed");
	}

	private static void SafeLog(string msg)
	{
		try
		{
			CastleLogger.Log(msg);
		}
		catch (Exception)
		{
		}
	}
}
