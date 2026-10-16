using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// CastleScanCreateObject v2: batch-mode forensic scan of the Open_Title scene.
// - lists scene GameObjects with missing-script components
// - walks every PlayMakerFSM: name, start state (best effort), state list
// - dumps every CreateObject action (owner, state, field values)
// - then edit-mode Instantiate tests: FontAll.prefab, scene missing-script GOs,
//   CreateObject targets, and finally an explicit Instantiate(null).
// All per-FSM/per-state work is individually guarded so a single bad FSM
// cannot abort the scan. Log flushed per line (survives a hard crash).
public static class CastleScanCreateObject
{
    private static System.IO.StreamWriter w;
    private static int testCount = 0;
    private static List<int> testedIds = new List<int>();
    private static bool fsmDumped = false;

    private static void L(string s)
    {
        try { w.WriteLine(s); w.Flush(); } catch (Exception) { }
        try { Debug.Log("[scanc] " + s); } catch (Exception) { }
    }

    private static string Innermost(Exception e)
    {
        while (e.InnerException != null) { e = e.InnerException; }
        return e.GetType().Name + ": " + e.Message;
    }

    private static object GetMember(object o, string name)
    {
        if (o == null) { return null; }
        var t = o.GetType();
        var p = t.GetProperty(name);
        if (p != null && p.GetIndexParameters().Length == 0) { return p.GetValue(o, null); }
        var f = t.GetField(name);
        if (f != null) { return f.GetValue(o); }
        return null;
    }

    private static string Str(object o)
    {
        if (o == null) { return "<null>"; }
        string s;
        try { s = Convert.ToString(o); } catch (Exception) { s = "<tostring-err>"; }
        if (s == null) { s = "<null-str>"; }
        if (s.Length > 70) { s = s.Substring(0, 70) + "..."; }
        return s;
    }

    private static string GetPath(Transform t)
    {
        string p = t.name;
        while (t.parent != null) { t = t.parent; p = t.name + "/" + p; }
        return p;
    }

    private static string GetAssetPath(UnityEngine.Object o)
    {
        try
        {
            string p = AssetDatabase.GetAssetPath(o);
            return string.IsNullOrEmpty(p) ? "<scene>" : p;
        }
        catch (Exception) { return "<err>"; }
    }

    private static void TestInstantiate(UnityEngine.Object target, string tag)
    {
        if (target != null)
        {
            int id = target.GetInstanceID();
            if (testedIds.Contains(id)) { return; }
            testedIds.Add(id);
        }
        testCount++;
        L("TEST-INSTANTIATE #" + testCount + " [" + tag + "]: " + (target == null ? "<null target>" : "'" + target.name + "' (" + target.GetType().Name + ", asset=" + GetAssetPath(target) + ")"));
        var sw = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            UnityEngine.Object clone = UnityEngine.Object.Instantiate(target);
            sw.Stop();
            L("   result: " + (clone == null ? "NULL" : "'" + clone.name + "'") + " in " + sw.ElapsedMilliseconds + "ms");
            if (clone != null) { UnityEngine.Object.DestroyImmediate(clone); L("   clone destroyed"); }
        }
        catch (Exception e)
        {
            sw.Stop();
            L("   EXCEPTION after " + sw.ElapsedMilliseconds + "ms: " + Innermost(e));
        }
    }

    public static void Run()
    {
        System.IO.Directory.CreateDirectory("D:/castle-port-56/scans");
        w = new System.IO.StreamWriter("D:/castle-port-56/scans/scan-createobject.txt");
        try
        {
            L("=== scan v2 start " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ===");

            string scenePath = null;
            foreach (var s in EditorBuildSettings.scenes)
            {
                if (s.path.IndexOf("Open_Title") >= 0) { scenePath = s.path; break; }
            }
            L("scene path: " + scenePath);
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            L("scene opened: " + scene.name + " roots=" + scene.GetRootGameObjects().Length);

            var sceneMissing = new List<GameObject>();
            L("=== missing script components (scene objects) ===");
            foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go == null || !go.scene.IsValid() || go.scene != scene) { continue; }
                foreach (var c in go.GetComponents<Component>())
                {
                    if (c == null)
                    {
                        L("MISSING SCRIPT on GO: '" + go.name + "' path='/" + GetPath(go.transform) + "'");
                        if (!sceneMissing.Contains(go)) { sceneMissing.Add(go); }
                    }
                }
            }
            L("missing-script scene objects: " + sceneMissing.Count);

            L("=== PlayMaker FSMs ===");
            var createTargets = new List<UnityEngine.Object>();
            int fsmCount = 0;
            int createCount = 0;
            foreach (var comp in Resources.FindObjectsOfTypeAll<Component>())
            {
                if (comp == null || comp.GetType().Name.IndexOf("PlayMakerFSM") < 0) { continue; }
                if (!comp.gameObject.scene.IsValid() || comp.gameObject.scene != scene) { continue; }
                fsmCount++;
                try
                {
                    object fsm = GetMember(comp, "Fsm");
                    if (fsm == null) { L("FSM on '" + comp.gameObject.name + "': Fsm member unavailable"); continue; }
                    if (!fsmDumped)
                    {
                        fsmDumped = true;
                        L("== Fsm member dump (first FSM: " + comp.gameObject.name + ") ==");
                        foreach (var f in fsm.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                        {
                            string val;
                            try { val = Str(f.GetValue(fsm)); } catch (Exception e2) { val = "<err " + e2.GetType().Name + ">"; }
                            L("  field " + f.Name + " : " + f.FieldType.Name + " = " + val);
                        }
                        foreach (var pr in fsm.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (pr.GetIndexParameters().Length > 0) { continue; }
                            string val;
                            try { val = Str(pr.GetValue(fsm, null)); } catch (Exception e2) { val = "<err " + e2.GetType().Name + ">"; }
                            L("  prop " + pr.Name + " : " + pr.PropertyType.Name + " = " + val);
                        }
                    }
                    var stateArr = GetMember(fsm, "States") as Array;
                    if (stateArr == null) { L("FSM on '" + comp.gameObject.name + "': no States"); continue; }
                    bool compEnabled = true;
                    try { object en = GetMember(comp, "enabled"); compEnabled = (en == null) || Convert.ToBoolean(en); } catch (Exception) { }
                    string fsmName = Convert.ToString(GetMember(comp, "FsmName"));
                    if (string.IsNullOrEmpty(fsmName)) { fsmName = Convert.ToString(GetMember(fsm, "Name")); }
                    string startState = "";
                    try { object ss = GetMember(fsm, "StartState"); if (ss != null) { startState = Convert.ToString(GetMember(ss, "Name")); } } catch (Exception e3) { startState = "<err " + e3.GetType().Name + ">"; }
                    L("FSM: GO='" + comp.gameObject.name + "' path='/" + GetPath(comp.transform) + "' fsmName='" + fsmName + "' startState='" + startState + "' enabled=" + compEnabled + " states=" + stateArr.Length);
                    foreach (var st in stateArr)
                    {
                        if (st == null) { continue; }
                        string stateName = "";
                        try { stateName = Convert.ToString(GetMember(st, "Name")); } catch (Exception) { }
                        Array actions = null;
                        try { actions = GetMember(st, "Actions") as Array; }
                        catch (Exception e4)
                        {
                            L("   STATE '" + stateName + "': actions unavailable (" + Innermost(e4) + ")");
                        }
                        if (actions == null) { continue; }
                        foreach (var a in actions)
                        {
                            if (a == null || a.GetType().Name != "CreateObject") { continue; }
                            createCount++;
                            L("--- CreateObject in GO='" + comp.gameObject.name + "' fsm='" + fsmName + "' state='" + stateName + "'");
                            L("      action.Enabled=" + Str(GetMember(a, "Enabled")));
                            foreach (var f in a.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                            {
                                object v = null;
                                try { v = f.GetValue(a); } catch (Exception) { }
                                string vs;
                                try
                                {
                                    if (v == null) { vs = "NULL"; }
                                    else
                                    {
                                        object vv = GetMember(v, "Value");
                                        var o = (vv as UnityEngine.Object) ?? (v as UnityEngine.Object);
                                        if (o != null)
                                        {
                                            vs = o.name + " [" + o.GetType().Name + ", inst=" + o.GetInstanceID() + ", asset=" + GetAssetPath(o) + "]";
                                            if (!testedIds.Contains(o.GetInstanceID()) && !createTargets.Contains(o)) { createTargets.Add(o); }
                                        }
                                        else if (vv != null) { vs = Str(vv) + " (non-object)"; }
                                        else { vs = Str(v); }
                                    }
                                }
                                catch (Exception e5) { vs = "<err " + e5.Message + ">"; }
                                L("      " + f.Name + " = " + vs);
                            }
                        }
                    }
                }
                catch (Exception eFsm)
                {
                    L("FSM EXCEPTION on '" + comp.gameObject.name + "': " + Innermost(eFsm));
                }
            }
            L("fsm count: " + fsmCount + "  CreateObject actions: " + createCount);

            L("=== instantiate tests ===");
            TestInstantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObject/FontAll.prefab"), "FontAll.prefab");
            foreach (var go in sceneMissing) { TestInstantiate(go, "scene missing-script GO"); }
            foreach (var t in createTargets) { TestInstantiate(t, "CreateObject target"); }
            TestInstantiate(null, "explicit null");
            L("=== ALL TESTS COMPLETED WITHOUT CRASH ===");
        }
        catch (Exception e)
        {
            L("GLOBAL EXCEPTION: " + e.ToString());
        }
        try { w.Close(); } catch (Exception) { }
        EditorApplication.Exit(0);
    }
}
