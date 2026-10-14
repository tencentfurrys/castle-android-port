using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the current screen resolution.")]
	[ActionCategory("ScreenSettings")]
	public class GetScreenResolution : FsmStateAction
	{
		[RequiredField]
		public FsmFloat storeScreenWidth;

		[RequiredField]
		public FsmFloat storeScreenHeight;

		public override void Reset()
		{
			storeScreenWidth = null;
			storeScreenHeight = null;
		}

		public override void OnEnter()
		{
			storeScreenWidth.Value = Screen.width;
			storeScreenHeight.Value = Screen.height;
			Finish();
		}
	}
}
