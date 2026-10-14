using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableGetRandom : HashTableActions
	{
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[ActionSection("Set up")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		public FsmEvent failureEvent;

		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		public FsmString key;

		[UIHint(UIHint.Variable)]
		public FsmVar result;

		private ArrayList _keys;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			failureEvent = null;
			result = null;
		}

		public override void OnEnter()
		{
			if (!SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				base.Fsm.Event(failureEvent);
				Finish();
			}
			_keys = new ArrayList(proxy.hashTable.Keys);
			DoGetRandom();
			Finish();
		}

		private void DoGetRandom()
		{
			int index = UnityEngine.Random.Range(0, _keys.Count);
			object obj = null;
			try
			{
				obj = proxy.hashTable[_keys[index]];
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				base.Fsm.Event(failureEvent);
				return;
			}
			key.Value = (string)_keys[index];
			PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, result, obj);
		}
	}
}
