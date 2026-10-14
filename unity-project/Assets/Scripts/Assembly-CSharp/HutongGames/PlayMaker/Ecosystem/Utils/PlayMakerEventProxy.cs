using UnityEngine;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	public class PlayMakerEventProxy : MonoBehaviour
	{
		public PlayMakerEventTarget eventTarget = new PlayMakerEventTarget(false);

		[EventTargetVariable("eventTarget")]
		public PlayMakerEvent fsmEvent;

		public bool debug;

		protected void SendPlayMakerEvent()
		{
			if (debug || !Application.isPlaying)
			{
				Debug.Log("Send " + fsmEvent.ToString() + " on " + eventTarget.ToString(), this);
			}
			if (!Application.isPlaying)
			{
				Debug.Log("<color=RED>Application must run to send a PlayMaker Event, but the proxy at least works</color>", this);
			}
			else
			{
				fsmEvent.SendEvent(null, eventTarget);
			}
		}
	}
}
