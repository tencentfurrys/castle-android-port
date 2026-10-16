using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Store all active GameObjects from inside a collider with a specific tag and/or layer. Will filter first by tag then by layer. Tags and/or layers must be declared in the tag/layer manager before using them")]
	[ActionCategory("ArrayMaker/ArrayList")]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=11754.0")]
	public class ArrayListFindGameObjectsInsideCollider : ArrayListActions
	{
		[ActionSection("Set up")]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		public FsmObject colliderTarget;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("by tag")]
		[ActionSection("Filter")]
		public FsmString tag;

		[UIHint(UIHint.FsmBool)]
		[Tooltip("Also filter by layer?")]
		[Title("Incl Layer Filter")]
		public FsmBool layerFilterOn;

		[UIHint(UIHint.Layer)]
		public int layer;

		private List<GameObject> tempList = new List<GameObject>();

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			tag = null;
			layerFilterOn = false;
			layer = 0;
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
			if (!isProxyValid())
			{
				return;
			}
			proxy.arrayList.Clear();
			GameObject[] array = GameObject.FindGameObjectsWithTag(tag.Value);
			if (array.Length == 0)
			{
				Debug.LogWarning("No object with tag:  " + tag.Value);
				return;
			}
			tempList.Clear();
			Collider collider = colliderTarget.Value as Collider;
			Bounds bounds = collider.bounds;
			if (!layerFilterOn.Value)
			{
				for (int i = 0; i < array.Length; i++)
				{
					Renderer component = array[i].GetComponent<Renderer>();
					Bounds bounds2 = component.bounds;
					if (bounds.Intersects(bounds2))
					{
						tempList.Add(array[i]);
					}
				}
			}
			if (layerFilterOn.Value)
			{
				for (int j = 0; j < array.Length; j++)
				{
					Renderer component2 = array[j].GetComponent<Renderer>();
					Bounds bounds3 = component2.bounds;
					if (bounds.Intersects(bounds3) && array[j].gameObject.layer == layer)
					{
						tempList.Add(array[j]);
					}
				}
			}
			proxy.arrayList.InsertRange(0, tempList);
		}
	}
}
