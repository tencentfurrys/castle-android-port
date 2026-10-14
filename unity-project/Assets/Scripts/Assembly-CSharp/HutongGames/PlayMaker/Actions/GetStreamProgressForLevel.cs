using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the Stream progres for a level")]
	[ActionCategory(ActionCategory.Level)]
	public class GetStreamProgressForLevel : FsmStateAction
	{
		[Tooltip("The name of the level to load. NOTE: Must be in the list of levels defined in File->Build Settings... ")]
		[RequiredField]
		public FsmString levelName;

		[Tooltip("Store the loading progress: range 0-1")]
		[UIHint(UIHint.Variable)]
		public FsmFloat loadingProgress;

		[Tooltip("Event to send when the level has loaded. NOTE: This only makes sense if the FSM is still in the scene!")]
		public FsmEvent loadedEvent;

		public override void Reset()
		{
			levelName = null;
			loadingProgress = null;
			loadedEvent = null;
		}

		public override void OnEnter()
		{
			GetProgress();
		}

		public override void OnUpdate()
		{
			GetProgress();
		}

		private void GetProgress()
		{
			loadingProgress.Value = Application.GetStreamProgressForLevel(levelName.Value);
			if (loadingProgress.Value == 1f)
			{
				base.Fsm.Event(loadedEvent);
				Finish();
			}
		}
	}
}
