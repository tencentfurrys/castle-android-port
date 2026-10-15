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

		public static void NotifyLoadComplete()
		{
			// Called by CastleDiag.OnLevelWasLoaded (survives scene switches).
			// Rescues the slot when the loading FSM died in a single load.
			activeLoad = null;
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
#if UNITY_ANDROID && !UNITY_EDITOR
					// CastlePort P9: LoadLevelAdditiveAsync never completes on
					// Android (5.x loader stalls silently: loading flag clears,
					// zero objects instantiate, no error). Synchronous additive
					// uses the main-thread path and gets through. The loading
					// screen (CrossMove) is already up, so the hitch is covered.
					Application.LoadLevelAdditive(levelName.Value);
					Debug.Log("LoadLevelAdditiveSync: " + levelName.Value);
					if (asyncProgress != null)
					{
						asyncProgress.Value = 1f;
					}
					Log("LOAD COMPLETE");
					base.Fsm.Event(loadedEvent);
					Finish();
					return;
#else
					TryBeginLoad(true);
					return;
#endif
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

		private void BeginNow(bool additiveLoad)
		{
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
				if (activeLoad == null || activeLoad.isDone)
				{
					BeginNow(additive);
				}
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
