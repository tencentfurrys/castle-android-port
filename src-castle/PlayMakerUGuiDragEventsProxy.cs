using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker.Ecosystem.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayMakerUGuiDragEventsProxy : MonoBehaviour, IEventSystemHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public bool debug;

	public PlayMakerEventTarget eventTarget = new PlayMakerEventTarget(true);

	[ShowOptions]
	[EventTargetVariable("eventTarget")]
	public PlayMakerEvent onBeginDragEvent = new PlayMakerEvent("UGUI / ON BEGIN DRAG");

	[EventTargetVariable("eventTarget")]
	[ShowOptions]
	public PlayMakerEvent onDragEvent = new PlayMakerEvent("UGUI / ON DRAG");

	[EventTargetVariable("eventTarget")]
	[ShowOptions]
	public PlayMakerEvent onEndDragEvent = new PlayMakerEvent("UGUI / ON END DRAG");

	public void OnBeginDrag(PointerEventData data)
	{
		if (debug)
		{
			Debug.Log("OnBeginDrag " + data.pointerId + " on " + gameObject.name);
		}
		GetLastPointerDataInfo.lastPointeEventData = data;
		onBeginDragEvent.SendEvent(PlayMakerUGuiSceneProxy.fsm, eventTarget);
	}

	public void OnDrag(PointerEventData data)
	{
		if (debug)
		{
			Debug.Log("OnDrag " + data.pointerId + " on " + gameObject.name);
		}
		GetLastPointerDataInfo.lastPointeEventData = data;
		onDragEvent.SendEvent(PlayMakerUGuiSceneProxy.fsm, eventTarget);
	}

	public void OnEndDrag(PointerEventData data)
	{
		if (debug)
		{
			Debug.Log("OnEndDrag " + data.pointerId + " on " + gameObject.name);
		}
		GetLastPointerDataInfo.lastPointeEventData = data;
		onEndDragEvent.SendEvent(PlayMakerUGuiSceneProxy.fsm, eventTarget);
	}
}
