using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Checks if screen is full screen")]
	[ActionCategory(ActionCategory.Screen)]
	public class CheckIfFullScreen : FsmStateAction
	{
		[Tooltip("True if Application is full screen")]
		public FsmBool isFullScreen;

		[Tooltip("Event to send if application is full screen")]
		public FsmEvent isFullScreenEvent;

		[Tooltip("Event to send if application is not full screen")]
		public FsmEvent isNotFullScreenEvent;

		[Tooltip("Runs everyframe. Event are sent discretly, on action first execution, and then only when changes occur")]
		public bool everyFrame;

		private int _cache;

		public override void Reset()
		{
			isFullScreen = null;
			isFullScreenEvent = null;
			isNotFullScreenEvent = null;
			_cache = -1;
		}

		public override void OnEnter()
		{
			_cache = -1;
			Check();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			Check();
		}

		private void Check()
		{
			if (Screen.fullScreen && _cache != 1)
			{
				_cache = 1;
				isFullScreen.Value = true;
				base.Fsm.Event(isFullScreenEvent);
			}
			else if (!Screen.fullScreen && _cache != 0)
			{
				_cache = 0;
				isFullScreen.Value = false;
				base.Fsm.Event(isNotFullScreenEvent);
			}
		}
	}
}
