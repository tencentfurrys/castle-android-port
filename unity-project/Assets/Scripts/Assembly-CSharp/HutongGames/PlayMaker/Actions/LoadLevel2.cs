using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Level)]
	[Tooltip("Loads a Level by Name. For async loading, gives pogress feedback. NOTE: Before you can load a level, you have to add it to the list of levels defined in File->Build Settings...")]
	public class LoadLevel2 : FsmStateAction
	{
		[Tooltip("The name of the level to load. NOTE: Must be in the list of levels defined in File->Build Settings... ")]
		[RequiredField]
		public FsmString levelName;

		[Tooltip("Load the level additively, keeping the current scene.")]
		public bool additive;

		[Tooltip("Load the level asynchronously in the background.")]
		public bool async;

		[Tooltip("asynchronous loading progress.")]
		public FsmFloat asyncProgress;

		[Tooltip("Event to send when the level has loaded. NOTE: This only makes sense if the FSM is still in the scene!")]
		public FsmEvent loadedEvent;

		[Tooltip("Keep this GameObject in the new level. NOTE: The GameObject and components is disabled then enabled on load; uncheck Reset On Disable to keep the active state.")]
		public FsmBool dontDestroyOnLoad;

		private AsyncOperation asyncOperation;
		private bool waitingForSlot;

		// CastlePort: Unity 5.x never finishes a second async scene load that
		// starts while one is still in flight (title menu fires single +
		// additive back to back; on slow devices the additive stalls silently
		// with no error). Serialize: one async load at a time, rest wait.
		private static AsyncOperation activeLoad;

		// CastlePort P12: frame when the last scene load finished. The engine's
		// isLoadingLevel flag lags the AsyncOperation.isDone flag by a frame or
		// more; a sync additive fired while isLoadingLevel==1 is silently
		// ignored (p11: additive dropped, zero objects, no error, no callback).
		// Gate the additive on the engine flag plus a small settle margin.
		private static int lastLoadCompleteFrame = -100;
		private int attempts;
		private int retryAtFrame = -1;

		public static void NotifyLoadComplete()
		{
			// Called by CastleDiag.OnLevelWasLoaded (survives scene switches).
			// Rescues the slot when the loading FSM died in a single load.
			activeLoad = null;
			lastLoadCompleteFrame = Time.frameCount;
		}

		public override void Reset()
		{
			levelName = string.Empty;
			additive = false;
			async = false;
			asyncProgress = null;
			loadedEvent = null;
			dontDestroyOnLoad = false;
		}

		public override void OnEnter()
		{
			if (dontDestroyOnLoad.Value)
			{
				Transform root = base.Owner.transform.root;
				Object.DontDestroyOnLoad(root.gameObject);
			}
			if (additive)
			{
				if (async)
				{
							// CastlePort P10: do NOT run sync inline here: the single
							// load is usually still in flight and its scene switch
							// would wipe what we add. Defer through the slot waiter
							// (proven to survive into the new scene); BeginNow runs
							// the sync additive once the slot frees.
							TryBeginLoad(true);
							return;
				}
				Application.LoadLevelAdditive(levelName.Value);
				Debug.Log("LoadLevelAdditive: " + levelName.Value);
			}
			else
			{
				if (async)
				{
					TryBeginLoad(false);
					return;
				}
				Application.LoadLevel(levelName.Value);
				Debug.Log("LoadLevel: " + levelName.Value);
			}
			Log("LOAD COMPLETE");
			base.Fsm.Event(loadedEvent);
			Finish();
		}

		private void TryBeginLoad(bool additiveLoad)
		{
			if (activeLoad != null && !activeLoad.isDone)
			{
				waitingForSlot = true;
				asyncOperation = null;
				Debug.Log("LoadLevelDeferred: " + levelName.Value + " (waiting for active load)");
				return;
			}
			BeginNow(additiveLoad);
		}

		private static int CountObjects()
		{
			return Resources.FindObjectsOfTypeAll(typeof(GameObject)).Length;
		}

		private void DoSyncAdditive()
		{
			// CastlePort P12: synchronous additive for Android. Must only run
			// once the engine is actually idle (activeLoad done AND
			// isLoadingLevel 0). A call made while the previous load is still
			// settling is silently dropped: verify with an object-count delta
			// and retry a few frames later if nothing was added.
			attempts++;
			int before = CountObjects();
			Application.LoadLevelAdditive(levelName.Value);
			int after = CountObjects();
			bool added = (after - before) >= 1;
			Debug.Log("LoadLevelAdditiveSync: " + levelName.Value
				+ " attempt:" + attempts
				+ " objs:" + before + "->" + after
				+ (added ? " (added)" : " (DROPPED)"));
			if (!added && attempts < 5)
			{
				waitingForSlot = true;
				asyncOperation = null;
				retryAtFrame = Time.frameCount + 60;
				return;
			}
			if (asyncProgress != null)
			{
				asyncProgress.Value = 1f;
			}
			waitingForSlot = false;
			retryAtFrame = -1;
			Log(added ? "LOAD COMPLETE" : ("LOAD COMPLETE (additive gave up after " + attempts + " tries)"));
			base.Fsm.Event(loadedEvent);
			Finish();
		}

		private void BeginNow(bool additiveLoad)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			if (additiveLoad)
			{
				DoSyncAdditive();
				return;
			}
#endif
			asyncOperation = additiveLoad
				? Application.LoadLevelAdditiveAsync(levelName.Value)
				: Application.LoadLevelAsync(levelName.Value);
			if (asyncOperation == null)
			{
				waitingForSlot = true;
				Debug.Log("LoadLevelDeferred: " + levelName.Value + " (engine busy)");
				return;
			}
			waitingForSlot = false;
			activeLoad = asyncOperation;
			asyncProgress.Value = 0f;
			Debug.Log(additiveLoad ? "LoadLevelAdditiveAsyc: " + levelName.Value : "LoadLevelAsync: " + levelName.Value);
		}

		public override void OnUpdate()
		{
			if (waitingForSlot || asyncOperation == null)
			{
				if (activeLoad != null && !activeLoad.isDone)
				{
					return;
				}
				if (retryAtFrame > 0)
				{
					if (Time.frameCount >= retryAtFrame)
					{
						retryAtFrame = -1;
						BeginNow(additive);
					}
					return;
				}
				// CastlePort P12: the engine ignores load calls while its
				// internal loader is still settling after the previous load
				// (isLoadingLevel lags activeLoad.isDone). Wait for the flag
				// to clear plus a small margin before firing.
				if (waitingForSlot && (Application.isLoadingLevel || Time.frameCount < lastLoadCompleteFrame + 2))
				{
					return;
				}
				BeginNow(additive);
				return;
			}
			if (asyncOperation.isDone)
			{
				if (activeLoad == asyncOperation)
				{
					activeLoad = null;
				}
				asyncProgress.Value = 1f;
				base.Fsm.Event(loadedEvent);
				Finish();
			}
			else
			{
				asyncProgress.Value = asyncOperation.progress;
			}
		}
	}
}
