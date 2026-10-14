using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Remove item at a specified index of a PlayMaker ArrayList Proxy component")]
[ActionCategory("ArrayMaker/ArrayList")]
public class ArrayListRemoveAt : ArrayListActions
{
	[ActionSection("Set up")]
	[RequiredField]
	[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
	[CheckForComponent(typeof(PlayMakerArrayListProxy))]
	public FsmOwnerDefault gameObject;

	[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
	public FsmString reference;

	[Tooltip("The index to remove at")]
	[UIHint(UIHint.FsmInt)]
	public FsmInt index;

	[ActionSection("Result")]
	[UIHint(UIHint.FsmEvent)]
	[Tooltip("The event to trigger if the removeAt throw errors")]
	public FsmEvent failureEvent;

	public override void Reset()
	{
		gameObject = null;
		failureEvent = null;
		reference = null;
		index = null;
	}

	public override void OnEnter()
	{
		if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
		{
			doArrayListRemoveAt();
		}
		Finish();
	}

	public void doArrayListRemoveAt()
	{
		if (!isProxyValid())
		{
			return;
		}
		try
		{
			proxy.arrayList.RemoveAt(index.Value);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			Fsm.Event(failureEvent);
		}
	}
}
