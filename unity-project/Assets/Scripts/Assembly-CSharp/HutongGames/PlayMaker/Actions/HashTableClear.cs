namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Remove all content of a PlayMaker hashtable Proxy component")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableClear : HashTableActions
	{
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[ActionSection("Set up")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				ClearHashTable();
			}
			Finish();
		}

		public void ClearHashTable()
		{
			if (isProxyValid())
			{
				proxy.hashTable.Clear();
			}
		}
	}
}
