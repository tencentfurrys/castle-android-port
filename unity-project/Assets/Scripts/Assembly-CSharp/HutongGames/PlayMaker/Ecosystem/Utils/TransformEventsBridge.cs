using UnityEngine;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	public class TransformEventsBridge : MonoBehaviour
	{
		public PlayMakerEventTarget eventTarget = new PlayMakerEventTarget(false);

		[EventTargetVariable("eventTarget")]
		public PlayMakerEvent parentChangedEvent;

		[EventTargetVariable("eventTarget")]
		public PlayMakerEvent childrenChangedEvent;

		public bool debug;

		private void OnTransformParentChanged()
		{
			if (debug)
			{
				Debug.Log("OnTransformParentChanged(): Send " + parentChangedEvent.ToString() + " on " + eventTarget.ToString(), this);
			}
			parentChangedEvent.SendEvent(null, eventTarget);
		}

		private void OnTransformChildrenChanged()
		{
			if (debug)
			{
				Debug.Log("OnTransformChildrenChanged(): Send " + childrenChangedEvent.ToString() + " on " + eventTarget.ToString(), this);
			}
			childrenChangedEvent.SendEvent(null, eventTarget);
		}
	}
}
