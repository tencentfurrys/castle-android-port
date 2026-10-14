using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Toggle between fullscreen and windowed mode.")]
	[ActionCategory("ScreenSettings")]
	public class ToggleFullScreen : FsmStateAction
	{
		public override void OnEnter()
		{
			Screen.fullScreen = !Screen.fullScreen;
			Finish();
		}
	}
}
