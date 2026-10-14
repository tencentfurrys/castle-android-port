using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Store all the values of a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy) into a PlayMaker arrayList Proxy component (PlayMakerArrayListProxy).")]
[ActionCategory("ArrayMaker/HashTable")]
public class HashTableValues : HashTableActions
{
	[ActionSection("Set up")]
	[RequiredField]
	[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
	[CheckForComponent(typeof(PlayMakerHashTableProxy))]
	public FsmOwnerDefault gameObject;

	[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
	public FsmString reference;

	[CheckForComponent(typeof(PlayMakerArrayListProxy))]
	[ActionSection("Result")]
	[RequiredField]
	[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component that will store the values")]
	public FsmOwnerDefault arrayListGameObject;

	[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component that will store the values ( necessary if several component coexists on the same GameObject")]
	public FsmString arrayListReference;

	public override void Reset()
	{
		gameObject = null;
		reference = null;
		arrayListGameObject = null;
		arrayListReference = null;
	}

	public override void OnEnter()
	{
		if (SetUpHashTableProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
		{
			doHashTableValues();
		}
		Finish();
	}

	public void doHashTableValues()
	{
		if (!isProxyValid())
		{
			return;
		}
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(arrayListGameObject);
		if (!(ownerDefaultTarget == null))
		{
			PlayMakerArrayListProxy arrayListProxyPointer = GetArrayListProxyPointer(ownerDefaultTarget, arrayListReference.Value, silent: false);
			if (arrayListProxyPointer != null)
			{
				arrayListProxyPointer.arrayList.AddRange(proxy.hashTable.Values);
			}
		}
	}
}
