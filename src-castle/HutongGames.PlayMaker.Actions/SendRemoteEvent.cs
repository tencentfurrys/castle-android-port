using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Network)]
[Tooltip("Send an Fsm Event on a remote machine. Uses Unity RPC functions.")]
public class SendRemoteEvent : ComponentAction<NetworkView>
{
	[RequiredField]
	[CheckForComponent(typeof(NetworkView))]
	[Tooltip("The game object that sends the event.")]
	public FsmOwnerDefault gameObject;

	[RequiredField]
	[Tooltip("The event you want to send.")]
	public FsmEvent remoteEvent;

	[Tooltip("Optional string data. Use 'Get Event Info' action to retrieve it.")]
	public FsmString stringData;

	[Tooltip("Option for who will receive an RPC.")]
	public RPCMode mode;

	public override void Reset()
	{
		gameObject = null;
		remoteEvent = null;
		mode = RPCMode.All;
		stringData = null;
		mode = RPCMode.All;
	}

	public override void OnEnter()
	{
		DoRemoteEvent();
		Finish();
	}

	private void DoRemoteEvent()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			if (!stringData.IsNone && stringData.Value != string.Empty)
			{
				networkView.RPC("SendRemoteFsmEventWithData", mode, remoteEvent.Name, stringData.Value);
			}
			else
			{
				networkView.RPC("SendRemoteFsmEvent", mode, remoteEvent.Name);
			}
		}
	}
}
