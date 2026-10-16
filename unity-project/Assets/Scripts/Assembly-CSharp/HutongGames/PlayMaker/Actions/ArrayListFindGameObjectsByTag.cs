using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Store all active GameObjects with a specific tag. Tags must be declared in the tag manager before using them")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class ArrayListFindGameObjectsByTag : ArrayListActions
	{
		[ActionSection("Set up")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("the tag")]
		public FsmString tag;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			tag = null;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				FindGOByTag();
			}
			Finish();
		}

		public void FindGOByTag()
		{
			if (isProxyValid())
			{
				proxy.arrayList.Clear();
				GameObject[] c = GameObject.FindGameObjectsWithTag(tag.Value);
				proxy.arrayList.InsertRange(0, c);
			}
		}
	}
}
