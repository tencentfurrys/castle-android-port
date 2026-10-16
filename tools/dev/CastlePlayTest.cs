using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// CastlePlayTest v3: batch-mode play-mode repro harness for the boot crash.
// Opens Open_Title, adds a Probe component, enters play mode.
// Logs: editor ticks (frame/playing/paused/realtime), probe Update frames,
// non-Log log messages (asserts/warnings/errors), playmode state changes.
// Auto-unpauses if the editor pauses on assert; exits after 30s of play time
// OR 40s wall clock (so it can never hang forever).
public static class CastlePlayTest
{
    private static System.IO.StreamWriter w;
    private static int ticks = 0;
    private static int lastFrame = -1;
    private static int capturedLogs = 0;
    private static float t0;
    private static DateTime t0wall;
    private static bool playActiveLogged;

    public static void L(string s)
    {
        try { w.WriteLine(s); w.Flush(); } catch (Exception) { }
    }

    public class Probe : MonoBehaviour
    {
        private void Awake() { L("probe: Awake"); }
        private void Start() { L("probe: Start frame=" + Time.frameCount); }
        private void Update()
        {
            int f = Time.frameCount;
            if (f <= 3 || f == 10 || f == 25 || (f % 50) == 0)
            {
                L("probe: Update frame=" + f + " playing=" + Application.isPlaying
                    + " rt=" + Time.realtimeSinceStartup.ToString("F1"));
            }
        }
    }

    public static void Run()
    {
        System.IO.Directory.CreateDirectory("D:/castle-port-56/scans");
        w = new System.IO.StreamWriter("D:/castle-port-56/scans/playtest.txt");
        try
        {
            L("=== playtest v3 start " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                + " unity=" + Application.unityVersion + " ===");
            string scenePath = null;
            foreach (var s in EditorBuildSettings.scenes)
            {
                if (s.path.IndexOf("Open_Title") >= 0) { scenePath = s.path; break; }
            }
            L("scene: " + scenePath);
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            var go = new GameObject("CastlePlayProbe");
            go.AddComponent<Probe>();
            L("scene opened; probe added; requesting play mode");
            EditorApplication.playmodeStateChanged += OnPlaymodeChanged;
            Application.logMessageReceived += OnUnityLog;
            t0 = Time.realtimeSinceStartup;
            t0wall = DateTime.UtcNow;
            EditorApplication.update += Pump;
            EditorApplication.isPlaying = true;
            L("play mode requested");
        }
        catch (Exception e)
        {
            L("EXCEPTION: " + e.ToString());
            try { w.Close(); } catch (Exception) { }
            EditorApplication.Exit(0);
        }
    }

    private static void OnUnityLog(string message, string stack, LogType type)
    {
        if (type == LogType.Log) { return; }
        if (capturedLogs >= 300) { return; }
        capturedLogs++;
        string m = message;
        if (m != null && m.Length > 150) { m = m.Substring(0, 150); }
        L("LOG " + type + ": " + m);
    }

    private static void OnPlaymodeChanged()
    {
        L("playmodeStateChanged: isPlaying=" + EditorApplication.isPlaying
            + " willChange=" + EditorApplication.isPlayingOrWillChangePlaymode
            + " isPaused=" + EditorApplication.isPaused
            + " frame=" + Time.frameCount + " rt=" + Time.realtimeSinceStartup.ToString("F1"));
    }

    private static void Pump()
    {
        ticks++;
        string st;
        try
        {
            st = "frame=" + Time.frameCount + " playing=" + EditorApplication.isPlaying
                + " paused=" + EditorApplication.isPaused
                + " rt=" + (Time.realtimeSinceStartup - t0).ToString("F1");
        }
        catch (Exception) { st = "state-err"; }
        if (ticks <= 15 || (ticks % 200) == 0) { L("tick " + ticks + " " + st); }
        try
        {
            if (!playActiveLogged && EditorApplication.isPlaying) { playActiveLogged = true; L("PLAY ACTIVE " + st); }
            if (EditorApplication.isPaused) { EditorApplication.isPaused = false; L("auto-unpause applied at " + st); }
        }
        catch (Exception) { }
        float el = 0f;
        try { el = Time.realtimeSinceStartup - t0; } catch (Exception) { }
        if (el > 30f)
        {
            L("SURVIVED 30s of play mode - EXITING (frames=" + lastFrame + ")");
            try { w.Close(); } catch (Exception) { }
            try { EditorApplication.isPlaying = false; } catch (Exception) { }
            EditorApplication.Exit(0);
            return;
        }
        double wall = 0;
        try { wall = (DateTime.UtcNow - t0wall).TotalSeconds; } catch (Exception) { }
        if (wall > 40.0)
        {
            L("WALL-CLOCK EXIT after 40s (" + st + ") frames=" + lastFrame);
            try { w.Close(); } catch (Exception) { }
            try { EditorApplication.isPlaying = false; } catch (Exception) { }
            EditorApplication.Exit(0);
        }
    }
}
