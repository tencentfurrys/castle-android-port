using System.Collections.Generic;
using System.IO;
using UnityEngine;

// CastleLogger: runtime log for remote debugging (Android + editor).
// - Ring buffer of recent lines, drawn by CastleDiag (screenshot-able).
// - Appends to persistentDataPath/CastlePort.log (survives crashes; pull via adb:
//   adb pull /storage/emulated/0/Android/data/com.tencentfurrys.castle/files/...
//   note: 5.3 writes persistentDataPath to /data/data on old targets).
// - Captures Unity Debug logs/errors via logMessageReceived.
// - Game events: scene loads, touch presses (via CastleDiag.NoteKey), saves.
public static class CastleLogger
{
	private static List<string> lines = new List<string>();
	private static string filePath = "";
	private static int counter;
	private const int MAX_LINES = 40;

	public static void Init()
	{
		if (filePath != "")
		{
			return;
		}
		filePath = Path.Combine(Application.persistentDataPath, "CastlePort.log");
		try
		{
			Append("=== boot " + System.DateTime.Now.ToString("HH:mm:ss")
				+ " " + Application.loadedLevelName + " ===\n");
		}
		catch (System.Exception)
		{
		}
		Application.logMessageReceived += OnUnityLog;
		Log("logger ready -> " + filePath);
	}

	public static void Log(string message)
	{
		counter++;
		string line = counter.ToString("D4") + " " + message;
		lines.Add(line);
		while (lines.Count > MAX_LINES)
		{
			lines.RemoveAt(0);
		}
		if (filePath != "")
		{
			try
			{
				Append(line + "\n");
			}
			catch (System.Exception)
			{
			}
		}
	}

	// File.AppendAllText is missing from the 5.3 unity_web mscorlib profile
	// (same reason S2 used FileStream in ScreenshotSystem): append via FileStream.
	private static void Append(string text)
	{
		using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
		{
			byte[] b = System.Text.Encoding.UTF8.GetBytes(text);
			fs.Write(b, 0, b.Length);
		}
	}

	public static List<string> Recent()
	{
		return lines;
	}

	private static void OnUnityLog(string message, string stackTrace, LogType type)
	{
		if (type == LogType.Log && message.StartsWith("[CastlePort]"))
		{
			return; // already logged by us
		}
		string shortMsg = message;
		if (shortMsg.Length > 140)
		{
			shortMsg = shortMsg.Substring(0, 140);
		}
		Log("U/" + type.ToString() + ": " + shortMsg);
	}
}
