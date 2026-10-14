using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Adds a PlayMakerHashTableProxy Component to a Game Object. Use this to create arrayList on the fly instead of during authoring.\n Optionally remove the HashTable component on exiting the state.\n Simply point to existing if the reference exists already.")]
[ActionCategory("ArrayMaker/HashTable")]
public class HashTableCreate : HashTableActions
{
	[RequiredField]
	[Tooltip("The Game Object to add the Hashtable Component to.")]
	[ActionSection("Set up")]
	public FsmOwnerDefault gameObject;

	[Tooltip("Author defined Reference of the PlayMaker arrayList proxy component ( necessary if several component coexists on the same GameObject")]
	[UIHint(UIHint.FsmString)]
	public FsmString reference;

	[Tooltip("Remove the Component when this State is exited.")]
	public FsmBool removeOnExit;

	[UIHint(UIHint.FsmEvent)]
	[Tooltip("The event to trigger if the hashtable exists already")]
	[ActionSection("Result")]
	public FsmEvent alreadyExistsEvent;

	private PlayMakerHashTableProxy addedComponent;

	public override void Reset()
	{
		gameObject = null;
		reference = null;
		alreadyExistsEvent = null;
	}

	public override void OnEnter()
	{
		DoAddPlayMakerHashTable();
		Finish();
	}

	public override void OnExit()
	{
		if (removeOnExit.Value && addedComponent != null)
		{
			Object.Destroy(addedComponent);
		}
	}

	private void DoAddPlayMakerHashTable()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		PlayMakerHashTableProxy hashTableProxyPointer = GetHashTableProxyPointer(ownerDefaultTarget, reference.Value, silent: true);
		if (hashTableProxyPointer != null)
		{
			Fsm.Event(alreadyExistsEvent);
			return;
		}
		addedComponent = ownerDefaultTarget.AddComponent<PlayMakerHashTableProxy>();
		if (addedComponent == null)
		{
			Debug.LogError("Can't add PlayMakerHashTableProxy");
		}
		else
		{
			addedComponent.referenceName = reference.Value;
		}
	}
}
