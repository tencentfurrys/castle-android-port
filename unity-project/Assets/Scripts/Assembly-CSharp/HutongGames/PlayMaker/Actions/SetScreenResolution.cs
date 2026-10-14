using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the screen resolution.\nOptionally sets the Refresh Rate.")]
	[ActionCategory("ScreenSettings")]
	public class SetScreenResolution : FsmStateAction
	{
		[RequiredField]
		public FsmFloat screenWidth;

		[RequiredField]
		public FsmFloat screenHeight;

		public FsmBool fullscreen;

		public FsmInt refreshRate;

		public override void Reset()
		{
			screenWidth = 800f;
			screenHeight = 600f;
			fullscreen = false;
			refreshRate = 0;
		}

		public override void OnEnter()
		{
			int num = 0;
			num = Convert.ToInt32(screenWidth.Value);
			int num2 = 0;
			num2 = Convert.ToInt32(screenHeight.Value);
			bool flag = false;
			flag = fullscreen.Value;
			Screen.SetResolution(num, num2, flag, refreshRate.Value);
			Finish();
		}
	}
}
