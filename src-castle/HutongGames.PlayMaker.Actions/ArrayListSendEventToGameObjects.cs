using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Send event to all the GameObjects within an arrayList.")]
[ActionCategory("ArrayMaker/ArrayList")]
public class ArrayListSendEventToGameObjects : ArrayListActions
{
	[ActionSection("Set up")]
	public FsmEventTarget eventTarget;

	[RequiredField]
	[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
	[CheckForComponent(typeof(PlayMakerArrayListProxy))]
	public FsmOwnerDefault gameObject;

	[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
	public FsmString reference;

	[Tooltip("The event to send. NOTE: Events must be marked Global to send between FSMs.")]
	[RequiredField]
	public FsmEvent sendEvent;

	public FsmBool excludeSelf;

	public FsmBool sendToChildren;

	public override void Reset()
	{
		eventTarget = new FsmEventTarget();
		eventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;
		gameObject = null;
		reference = null;
		sendEvent = null;
		excludeSelf = false;
		sendToChildren = false;
	}

	public override void OnEnter()
	{
		if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
		{
			DoSendEvent();
		}
		Finish();
	}

	private void DoSendEvent()
	{
		if (!isProxyValid())
		{
			return;
		}
		foreach (GameObject array in proxy.arrayList)
		{
			sendEventToGO(array);
		}
	}

	private void sendEventToGO(GameObject _go)
	{
		FsmEventTarget fsmEventTarget = new FsmEventTarget();
		fsmEventTarget.excludeSelf = excludeSelf.Value;
		FsmOwnerDefault fsmOwnerDefault = new FsmOwnerDefault();
		fsmOwnerDefault.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
		fsmOwnerDefault.GameObject = new FsmGameObject();
		fsmOwnerDefault.GameObject.Value = _go;
		fsmEventTarget.gameObject = fsmOwnerDefault;
		fsmEventTarget.target = FsmEventTarget.EventTarget.GameObject;
		fsmEventTarget.sendToChildren = sendToChildren.Value;
		Fsm.Event(fsmEventTarget, sendEvent);
	}
}
