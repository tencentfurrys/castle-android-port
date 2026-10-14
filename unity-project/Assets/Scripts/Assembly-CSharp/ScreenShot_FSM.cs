using UnityEngine;
using UnityEngine.UI;

public class ScreenShot_FSM : MonoBehaviour
{
	public PlayMakerFSM fsm;

	public RawImage NowTexture;

	private void ScreenShot()
	{
		NowTexture.texture = ScreenshotSystem.ToTexture();
		fsm.SendEvent("Self_Next");
	}
}
