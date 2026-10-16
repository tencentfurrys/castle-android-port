using System;
using UnityEditor;
using UnityEngine;

// CastleScanPrefabs: edit-mode instantiate tests targeting the native crash
// in Open_Title's boot FSM (PlayMaker CreateObject -> Instantiate).
// Tests, in order:
//   1..3) Instantiate(prefab, Vector3.zero, Quaternion.identity) for the three
//         boot prefabs (exact overload PlayMaker uses)
//   4)    Instantiate(null)            -- 1-arg overload (control, managed check)
//   5)    Instantiate(null, v, q)      -- 3-arg overload (what PlayMaker actually calls)
// Log flushed per line; a hard crash leaves the last step visible.
public static class CastleScanPrefabs
{
    private static System.IO.StreamWriter w;

    private static void L(string s)
    {
        try { w.WriteLine(s); w.Flush(); } catch (Exception) { }
        try { Debug.Log("[scanp] " + s); } catch (Exception) { }
    }

    public static void Run()
    {
        System.IO.Directory.CreateDirectory("D:/castle-port-56/scans");
        w = new System.IO.StreamWriter("D:/castle-port-56/scans/scan-prefabs.txt");
        try
        {
            L("=== prefab scan start " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ===");
            TestPrefab("Game_Manager", "Assets/GameObject/Game_Manager.prefab");
            TestPrefab("New_Save_Controller", "Assets/GameObject/New_Save_Controller.prefab");
            TestPrefab("Main_Player", "Assets/GameObject/Main_Player.prefab");
            TestNull1();
            TestNull3();
            L("=== ALL TESTS COMPLETED WITHOUT CRASH ===");
        }
        catch (Exception e)
        {
            L("GLOBAL EXCEPTION: " + e.ToString());
        }
        try { w.Close(); } catch (Exception) { }
        EditorApplication.Exit(0);
    }

    private static void TestPrefab(string tag, string path)
    {
        L("TEST [" + tag + "]: loading " + path);
        var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        L("   loaded: " + (go == null ? "NULL" : go.name + " (inst=" + go.GetInstanceID() + ")"));
        if (go == null) { return; }
        L("   about to Instantiate(go, V3.zero, Q.identity) ...");
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        try
        {
            UnityEngine.Object clone = UnityEngine.Object.Instantiate(go, Vector3.zero, Quaternion.identity);
            sw.Stop();
            L("   RESULT: OK in " + sw.ElapsedMilliseconds + "ms -> " + (clone == null ? "NULL" : clone.name));
            if (clone != null) { UnityEngine.Object.DestroyImmediate(clone); L("   clone destroyed"); }
        }
        catch (Exception e)
        {
            sw.Stop();
            L("   EXCEPTION after " + sw.ElapsedMilliseconds + "ms: " + e.GetType().Name + ": " + e.Message);
        }
    }

    private static void TestNull1()
    {
        L("TEST [null 1-arg]: about to Instantiate((Object)null) ...");
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        try
        {
            UnityEngine.Object clone = UnityEngine.Object.Instantiate((UnityEngine.Object)null);
            sw.Stop();
            L("   RESULT: OK in " + sw.ElapsedMilliseconds + "ms -> " + (clone == null ? "NULL" : clone.name));
        }
        catch (Exception e)
        {
            sw.Stop();
            L("   EXCEPTION after " + sw.ElapsedMilliseconds + "ms: " + e.GetType().Name + ": " + e.Message);
        }
    }

    private static void TestNull3()
    {
        L("TEST [null 3-arg]: about to Instantiate((Object)null, V3.zero, Q.identity) ...");
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        try
        {
            UnityEngine.Object clone = UnityEngine.Object.Instantiate((UnityEngine.Object)null, Vector3.zero, Quaternion.identity);
            sw.Stop();
            L("   RESULT: OK in " + sw.ElapsedMilliseconds + "ms -> " + (clone == null ? "NULL" : clone.name));
        }
        catch (Exception e)
        {
            sw.Stop();
            L("   EXCEPTION after " + sw.ElapsedMilliseconds + "ms: " + e.GetType().Name + ": " + e.Message);
        }
    }
}
