# Castle of Temptation 1.1.2 — PC-to-Android port

Same pipeline as `mkm-android-port`: AssetRipper 2.0 rip of the Windows build
(`Castle_of_Temptation_V1.1.2_Itch.rar`, Unity **5.3.8f2**, Mono) + ILSpy
decompile + Android patches + headless Mono ARMv7 build.

## Layout

- `unity-project/` — ripped Unity 5.3.8f2 project. Open/build with **5.3.8f2 +
  Android module** (exact version required).
- `src-castle/` — ILSpy reference decompile of `Assembly-CSharp.dll`.
- `build/` — `CastleBuild.cs` (5.3-API build driver, `BuildAndroid`/`BuildWindows`),
  `apply_patches.py` (S0–S2), `fix_hideflags.py` (S3).
- `quarantine/` — `Shake_Head_Old.prefab`, vestigial dialogue prefab excluded from
  builds (trips 5.3's DontSave check; only referenced by an FSM default that reads
  a runtime variable anyway).

## Android patches (applied to `unity-project/`)

- **S0** Steam stub: `SteamManager.Awake`/`Update` early-return on Android
  (otherwise `Application.Quit()` without `steam_api`), `AchLoad` no-op.
- **S1** ES2 save files (`sceneObjectsFile.txt`, `createdObjectsFile.txt`,
  `SavedPrefabs.txt`) wrapped to `Application.persistentDataPath` via
  `CastleSavePath.Abs()` (13 + 6 call sites, overload-aware).
- **S2** `ScreenshotSystem` write path `dataPath` → `persistentDataPath`,
  `FileStream` instead of missing `WriteAllBytes`.
- **S3** Ripper `HideAndDontSave(61)` flags reset to 0 in 5 prefabs.
- Compat fixes for the 5.3 C#4 compiler: `SmoothDamp` positional args,
  `ColorHSV` struct-ctor field assignment, `FileStream`/`Copy` instead of
  `Open`/`Move`, MovieTexture PlayMaker actions guarded out of Android,
  dropped Ripper `AssemblyInfo.cs` files.

## Big scenes (GitHub 100MB file limit)

These 4 ripped scenes ship as **Release assets**, not in git:

- `Stage_02.unity` (431MB), `Stage_03.unity` (215MB),
  `Stage_04.unity` (469MB), `Stage_Gallery.unity` (268MB)

Download them from the release into
`unity-project/Assets/Game_Resourse/Castle_MyGameData/Scenes/` to reassemble a
buildable tree.

## Toolchain (this box, all on D:)

- Editor: `D:\Unity-5.3.8f2` (portable NSIS extract + Android module merged)
- SDK: `D:\Unity-5.3\sdk` (tools r25.2.5, android-23, build-tools 23.0.3),
  NDK r10e, JDK8 Corretto (5.3-era only; modern NDK27/Java17 do not work)
- License: `C:\ProgramData\Unity\Unity_v5.x.ulf`
- Headless: `Unity.exe -batchmode -quit -projectPath <unity-project>
  -executeMethod CastleBuild.BuildAndroid -logFile buildN.log`
- APK: `unity-project/Castle-1.1.2-android.apk` (gitignored, via Releases),
  `com.tencentfurrys.castle`, v1.1.2/vc1, Mono ARMv7 (5.3 has no ARM64).

## Status

First bootable APK built (`v1.1.2-android-p1`, 156MB). Untested on device.
Next: install test, `persistentDataPath` save check, touch controls (Unity
StandardAssets `CrossPlatformInput` is already in the rip: Joystick/TouchPad).
