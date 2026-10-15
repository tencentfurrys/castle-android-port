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

## Dev log (Oct 2026, second box)

Build/tree now reproducible outside the original box:

- Editor: Unity **5.3.8f2** changeset `0c7e33ff9c0e` + Android module via
  Unity Hub headless
  (`install --version 5.3.8f2 --changeset 0c7e33ff9c0e --module android`).
- Era toolchain (5.3-only): SDK tools r25.2.5 + `platform-tools` /
  `android-23` / `build-tools 23.0.3` (`D:\Unity-5.3\sdk`), NDK r10e
  (`D:\Unity-5.3\android-ndk-r10e`), portable JDK8 Corretto
  (`D:\Unity-5.3\jdk8`, system Java untouched), wired into 5.3
  external-tools prefs (`AndroidSdkRoot`, `AndroidNdkRoot`, `JdkPath`).
- Build tree: short path required for the 5.3 importer
  (`D:\castle-port`, clean clone + Release scenes copied in).
  Licensing via `C:\ProgramData\Unity\Unity_v5.x.ulf`.
- Release flow per build: headless `CastleBuild.BuildAndroid` →
  apktool decode → `targetSdkVersion` 23→28 (`apktool.yml`) → rebuild →
  zipalign → jarsigner (`castle-debug.keystore`, alias `castle`, in git
  history) → GitHub release `v1.1.2-android-pN` (t28).

Device: Galaxy A16 (Helio G99 / Mali-G57, Android 16). Unity 5.3 outputs
32-bit ARMv7+x86 only (no ARM64 exists in 5.3); A16 keeps 32-bit
userspace so it runs. t28 retarget required for Android 14+ install.
`com.tencentfurrys.castle`, v1.1.2, minSdk 16.

Findings from device logs/video (CastlePort.log + diag overlay):

- P7: all 45 `.shader` files were AssetRipper dummies (opaque Lambert,
  `target 3.0`, no fallback); 14 shadowed Unity built-ins by name
  (`Sprites/Default`, `Particles/*`, …). Rewritten as transparent
  ES2-safe shaders (same names/guids, Properties preserved,
  `ETC1_EXTERNAL_ALPHA` split-alpha support). Title background art
  renders again (was black boxes).
- Pink sparkle quads on title are live Shuriken particles rendering
  Unity error-magenta. Proven NOT the particle shader (repointing the
  materials to proven `Sprites/Default` changed nothing), NOT vertex
  colors (no magenta inputs project-wide; blue channel impossible from
  white-tex × red gradients), NOT compile (clean GLES build, no errors).
  Suspects left: particle-renderer vertex streams / Mali driver behavior.
  Eye-candy only; parked until gameplay runs.
- Black screen after NEW GAME is a LOAD-ORDERING race, not a hang in one
  loader: the additive Stage_00 fired while the single CrossMove load was
  still in flight; the single load's scene switch then wiped the added
  objects (`objs:` flat at 13429, zero instantiation, no error —
  deterministic across P6/P7/p8/p9). Fix (P10): additive defers through
  the slot waiter and runs synchronously in CrossMove context.
- Gotcha: p8 release APK was built from the P7 tree (predates the P8
  commit); verify tree-vs-APK contents via `objs:` header / strings in
  `Assembly-CSharp.dll` before trusting a test video.

Releases: p5 gestures/t28, p6 alpha-split+serializer, p7 settle test,
p8 shader fix (APK actually P7 tree), p9 single-variant particles +
`objs:` probe, p10 sync additive + sparkle mat-swap test, p11 deferred
sync additive.
