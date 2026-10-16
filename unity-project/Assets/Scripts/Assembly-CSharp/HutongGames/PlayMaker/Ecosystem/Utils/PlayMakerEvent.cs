using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	[Serializable]
	public class PlayMakerEvent
	{
		public string eventName;

		public bool allowLocalEvents;

		public string defaultEventName;

		public PlayMakerEvent()
		{
		}

		public PlayMakerEvent(string defaultEventName)
		{
			this.defaultEventName = defaultEventName;
			eventName = defaultEventName;
		}

		public bool SendEvent(PlayMakerFSM fromFsm, PlayMakerEventTarget eventTarget)
		{
			if (fromFsm == null)
			{
				if (PlayMakerUtils.FsmEventSender == null)
				{
					PlayMakerUtils.FsmEventSender = new GameObject("PlayMaker Send Event Proxy").AddComponent<PlayMakerFSM>();
					PlayMakerUtils.FsmEventSender.FsmName = "Send Event Proxy";
					PlayMakerUtils.FsmEventSender.FsmDescription = "This Fsm was created at runtime, because a script or component is willing to send a PlayMaker event";
				}
				fromFsm = PlayMakerUtils.FsmEventSender;
			}
			if (eventTarget.eventTarget == ProxyEventTarget.BroadCastAll)
			{
				PlayMakerFSM.BroadcastEvent(eventName);
			}
			else if (eventTarget.eventTarget == ProxyEventTarget.Owner || eventTarget.eventTarget == ProxyEventTarget.GameObject)
			{
				PlayMakerUtils.SendEventToGameObject(fromFsm, eventTarget.gameObject, eventName, eventTarget.includeChildren);
			}
			else if (eventTarget.eventTarget == ProxyEventTarget.FsmComponent)
			{
				eventTarget.fsmComponent.SendEvent(eventName);
			}
			return true;
		}

		public override string ToString()
		{
			string arg = "<color=blue>" + eventName + "</color>";
			if (string.IsNullOrEmpty(eventName))
			{
				arg = "<color=red>None</color>";
			}
			return string.Format("PlayMaker Event : {0}", arg);
		}
	}
}
