// Castle Android build driver (Unity 5.3.8f2 API).
// Drop into Assets/Editor/, run headless:
//   Unity.exe -batchmode -quit -projectPath <unity-project> -executeMethod CastleBuild.BuildAndroid -logFile build.log
// 5.3 notes: Mono2x scripting (IL2CPP experimental on 5.3 Android), ARMv7 only
// (ARM64 did not exist in 5.3 -> sideload-only APK, no Play Store upload).
using System.IO;
using UnityEditor;
using UnityEngine;

public static class CastleBuild
{
    public static void BuildWindows()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++) scenes[i] = EditorBuildSettings.scenes[i].path;
        string outExe = "D:/Castle-Work/pc-test/Castle-1.1.2-pc.exe";
        string res = BuildPipeline.BuildPlayer(scenes, outExe, BuildTarget.StandaloneWindows, BuildOptions.None);
        Debug.Log("Castle PC build result: '" + res + "' -> " + outExe);
        if (!string.IsNullOrEmpty(res))
            EditorApplication.Exit(1);
    }

    public static void BuildAndroid()
    {
        PlayerSettings.companyName = "CastlePort";
        PlayerSettings.productName = "Castle of Temptation";
        PlayerSettings.applicationIdentifier = "com.tencentfurrys.castle";
        PlayerSettings.bundleVersion = "1.1.2t2";
        PlayerSettings.Android.bundleVersionCode = 6;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel16;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.statusBarHidden = true;
        // Mono is the default scripting backend in 5.3 (no per-group API yet).
        // Full .NET 2.0: Subset strips IO members some ripped PlayMaker actions need.
        PlayerSettings.apiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0;
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android,
            new[] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2,
                    UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 });

        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++) scenes[i] = EditorBuildSettings.scenes[i].path;
        Debug.Log("Castle scenes:\n" + string.Join("\n", scenes));

        string outApk = Path.GetFullPath(Path.Combine(Application.dataPath, Path.Combine("..", "Castle-1.1.2-android.apk")));
        string res = BuildPipeline.BuildPlayer(scenes, outApk, BuildTarget.Android, BuildOptions.None);
        Debug.Log("Castle build result: '" + res + "' -> " + outApk);
        if (!string.IsNullOrEmpty(res))
            EditorApplication.Exit(1);
    }
}
