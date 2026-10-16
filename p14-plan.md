# p14 plan — stage-load crash on "New Game" (next session)

## Where p13 landed
- SHIPPED: v1.1.2-android-p13 (repack t28, vc6, same cert as p5-p12).
- Boot crash: FIXED — confirmed on user's device (Galaxy A16, Android 16/API 36, Mali-G68, GLES 3.2):
  tracer[p13t] -> 3x [CreateObject] ui-bare/ui-done -> frames counting -> title screen.
- Remaining on device:
  1) NEW BLOCKER: tap "New Game" -> CrossMove_Castle loads -> Stage_00 additive load
     logged "attempt:1 objs:12718->12718 (DROPPED)" -> BoxCollider warnings (likely
     Stage_00 objects activating) -> native crash ("app has a bug" dialog). No attempt:2 logged.
  2) Cosmetic: pink pixels on main menu (known 5.6 shader issue on GLES3/Mali; deal with later).
- User has no time for another device-test cycle right now; next iteration is p14.

## Load chain (from device log)
Open_Title title FSM (dontDestroyOnLoad) fires back-to-back on New Game tap:
  1. LoadLevelAsync: CrossMove_Castle (single, async)  -> completes ("scene loaded: CrossMove_Castle")
  2. LoadLevel2(Stage_00, additive+async) -> deferred ("waiting for active load")
     -> after slot frees: DoSyncAdditive (Android #if) -> Application.LoadLevelAdditive("Stage_00")
     -> same-frame object count unchanged -> "(DROPPED)" -> retry scheduled at +60 frames
     -> CRASH before/at retry (no attempt:2 line).
All scenes incl. Stage_00 are in build settings (verified from build log).

## Suspects for the crash (ranked)
(a) FALSE-DROPPED: 5.6 applies the legacy sync additive deferred (end-of-frame / next frame);
    our same-frame objs count misses it -> wrongly logs DROPPED -> +60 frames later we call
    LoadLevelAdditive("Stage_00") a SECOND time (double additive load of the whole stage,
    duplicate FSMs/scene objects) -> native crash. Fits the BoxCollider warnings right after
    attempt:1 (Stage_00 objects actually appearing) and the crash ~1s later.
(b) Stage_00 scene activation itself crashes natively (like the earlier prefab bug) — would
    mean the first load also crashed, retry never reached.
(c) Engine settling window longer than the 1-2 frame gate on this device (isLoadingLevel lag).
(d) Missing scripts on CrossMove_Castle loading UI (Percent, UI_Pic, UI_Pic02, UI_Word01,
    Loading, Load_Percent — "referenced script is missing") — likely separate; check whether
    loading screen is supposed to render (user: "loaded nothing").

## p14 fix plan (LoadLevel2.cs + diagnostics)
- After any load call, SAMPLE state over frames instead of same-frame delta:
  at +1, +5, +15 frames log: SceneManager.sceneCount, GetSceneByName(level).isLoaded,
  object count, Application.isLoadingLevel.
- Only retry if the scene is STILL not loaded after the sampling window; cap retries (<=2);
  never re-issue a load for a scene that is already loaded.
- Add log of Application.CanStreamedLevelBeLoaded(name) + isLoadingLevel before each call.
- Prefer SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive) + pump over legacy
  Application.LoadLevel* (fallback to legacy if op == null).
- Consider disabling retry entirely for one test build: if result is black screen (no crash)
  -> confirms (a) double-load crash; if crash persists -> (b) activation crash.

## Repro plan (to get a real stack before p14 device test)
- PC player test build with the sync-additive path enabled for standalone
  (widen #if UNITY_ANDROID && !UNITY_EDITOR -> include UNITY_STANDALONE for the test),
  plus a test-only auto-driver (#if !UNITY_ANDROID) that fires the New Game chain
  (~10s after boot: LoadLevelAsync CrossMove_Castle, then LoadLevelAdditive Stage_00).
- If PC reproduces -> crash dir + error.log gives the symbolicated stack -> fix precisely.
- Then: one Android build p14 -> repack (repack-p13t.ps1 pattern, OUT name p14, targetSdk 28)
  -> release v1.1.2-android-p14 -> user tests.

## Env reminders
- Repack script: D:\castle-port-56\tools\repack-p13t.ps1 (yml regex fixed for unquoted
  targetSdkVersion; update OutApk/markers for p14 + bump versionCode to 7, version 1.1.2t3,
  tracer TAG p14t).
- CastleBuild.cs sets versionCode/versionName before build.
- PC player: D:\Castle-Work\pc-test; crash dirs = timestamped folders with error.log.
- Device log: /storage/emulated/0/Download/CastlePort.log (mirror of app data dir).
- Editor play-mode harness learns: entering play wipes editor-assembly statics (domain
  reload) — use game-side tracer log (CastlePort.log in LocalLow) for evidence.

## Backup status (2026-10-16, RDP machine dying)
- Working tree backed up to the repo across commits 93c76df + this follow-up: Assets/Scripts, Assets/Editor, Assets/Plugins, ProjectSettings, Packages, scenes (Open_Title, CrossMove_Castle, Stage_00, Stage_01, Stage_End), tools/, p14-plan.md.
- Signing keystore: private repo tencentfurrys/castle-port-secrets (castle-debug.keystore + README with fingerprints; alias castle / storepass android). NEVER public.
- Huge scenes Stage_02/03/04/Gallery (431/215/470/268 MB): stored as release assets of v1.1.2-android-p1 (GitHub 100MB file limit blocks git).
- The remaining bulk of Assets/ (reserialized binaries) stays in repo history; Unity re-imports it on checkout.
