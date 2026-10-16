using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=15458.0")]
	[Tooltip("Store all active GameObjects with a collider component that are inside the specified collider while optionally filtering by tag and/or layer. NOTE: Tags and layers must be declared in the tag/layer manager before using them.")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class ListGameObjectsInsideCollider : ArrayListActions
	{
		[ActionSection("Set up")]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject).")]
		public FsmString reference;

		[Tooltip("The collider to check against other intersecting colliders.")]
		[ObjectType(typeof(Collider))]
		[RequiredField]
		public FsmObject colliderTarget;

		[ActionSection("Filter")]
		[UIHint(UIHint.Tag)]
		[Tooltip("Optionally filter by tag.")]
		public FsmString tag;

		[Tooltip("Also filter by layer?")]
		[Title("Incl Layer Filter")]
		public FsmBool layerFilterOn;

		[UIHint(UIHint.Layer)]
		public int layer;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the amount of matching GameObjects.")]
		[ActionSection("Optionally")]
		public FsmInt storeAmount;

		[Tooltip("Wheter to update on every frame.")]
		public FsmBool everyFrame;

		private List<GameObject> tempList = new List<GameObject>();

		public override void Reset()
		{
			gameObject = null;
			colliderTarget = null;
			reference = null;
			tag = "Untagged";
			layerFilterOn = false;
			layer = 0;
			storeAmount = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			FindGOByTag();
			if (!everyFrame.Value)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			FindGOByTag();
		}

		public void FindGOByTag()
		{
			if (!SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), reference.Value) || !isProxyValid())
			{
				return;
			}
			GameObject[] array = Object.FindObjectsOfType<GameObject>();
			if (array.Length == 0)
			{
				return;
			}
			tempList.Clear();
			Collider collider = colliderTarget.Value as Collider;
			Bounds bounds = collider.bounds;
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = array[i];
				if (!string.IsNullOrEmpty(tag.Value) && gameObject.tag != tag.Value)
				{
					continue;
				}
				Collider component = array[i].GetComponent<Collider>();
				if ((bool)component)
				{
					Bounds bounds2 = component.bounds;
					if (bounds.Intersects(bounds2) && (!layerFilterOn.Value || array[i].gameObject.layer == layer))
					{
						tempList.Add(array[i]);
					}
				}
			}
			proxy.arrayList.InsertRange(0, tempList);
			storeAmount.Value = tempList.Count;
		}

		public override string ErrorCheck()
		{
			if (colliderTarget.Value == null || colliderTarget.IsNone)
			{
				return "Please specify the Collider Target!";
			}
			return string.Empty;
		}
	}
}
