using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Gets an item from a PlayMaker ArrayList Proxy component")]
	public class ArrayListGet : ArrayListActions
	{
		[ActionSection("Set up")]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[UIHint(UIHint.FsmInt)]
		[Tooltip("The index to retrieve the item from")]
		public FsmInt atIndex;

		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		[RequiredField]
		public FsmVar result;

		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			atIndex = null;
			gameObject = null;
			failureEvent = null;
			result = null;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				GetItemAtIndex();
			}
			Finish();
		}

		public void GetItemAtIndex()
		{
			if (!isProxyValid())
			{
				return;
			}
			if (result.IsNone)
			{
				base.Fsm.Event(failureEvent);
				return;
			}
			object obj = null;
			try
			{
				obj = proxy.arrayList[atIndex.Value];
			}
			catch (Exception)
			{
				base.Fsm.Event(failureEvent);
				return;
			}
			PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, result, obj);
		}
	}
}
