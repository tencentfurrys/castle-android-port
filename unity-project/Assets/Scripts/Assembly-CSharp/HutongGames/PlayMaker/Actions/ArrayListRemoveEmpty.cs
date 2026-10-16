using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Remove an empty (null) element of a PlayMaker Array List Proxy component")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class ArrayListRemoveEmpty : ArrayListActions
	{
		[ActionSection("Set up")]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		public FsmString reference;

		private new PlayMakerArrayListProxy proxy;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
		}

		public override void OnEnter()
		{
			proxy = GetArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value, true);
			if (proxy != null)
			{
				doArrayListRemoveEmpty();
			}
			else
			{
				Debug.LogWarning("<b>[ArrayListRemoveEmpty]</b><color=#FF9900ff> Maybe wrong name - Please review!</color>", base.Owner);
			}
			Finish();
		}

		public void doArrayListRemoveEmpty()
		{
			int num = proxy.arrayList.Count;
			for (int i = 0; i < num; i++)
			{
				object obj = null;
				try
				{
					obj = proxy.arrayList[i];
				}
				catch (Exception)
				{
					Debug.LogWarning("<b>[ArrayListRemoveEmpty]</b><color=#FF9900ff> Please review!</color>", base.Owner);
					break;
				}
				if (obj == null || obj.Equals(null))
				{
					proxy.arrayList.RemoveAt(i);
					num--;
					i--;
					if (num == 0 && obj != null)
					{
						break;
					}
				}
			}
		}
	}
}
