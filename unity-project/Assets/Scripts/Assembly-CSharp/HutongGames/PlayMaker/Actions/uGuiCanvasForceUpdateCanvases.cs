using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Force all canvases to update their content.\nCode that relies on up-to-date layout or content can call this method to ensure it before executing code that relies on it.")]
	[ActionCategory("uGui")]
	public class uGuiCanvasForceUpdateCanvases : FsmStateAction
	{
		public override void OnEnter()
		{
			Canvas.ForceUpdateCanvases();
			Finish();
		}
	}
}
