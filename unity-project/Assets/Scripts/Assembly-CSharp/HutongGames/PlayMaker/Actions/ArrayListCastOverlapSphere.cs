using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Cast an overlap sphere and get the objects(by collider) that it hits. Note: This action returns the game objects that have colliders attached. If a hit game object has multiple colliders it will return copies of the game object.")]
	public class ArrayListCastOverlapSphere : ArrayListActions
	{
		[Tooltip("The gameObject to cast the Overlap sphere from.")]
		[ActionSection("Overlap Sphere Settings")]
		public FsmOwnerDefault scanOrigin;

		[Tooltip("The size of the overlap sphere.")]
		public FsmFloat scanRange;

		[ActionSection("ArrayList Info")]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault arrayListOwner;

		[Tooltip("The name of the arrayList you want to store the hit objects in.")]
		public FsmString arrayListReference;

		[ActionSection("Filter")]
		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		[Tooltip("Set to true to ignore colliders set to trigger.")]
		public FsmBool ignoreTriggerColliders;

		public FsmEvent ErrorEvent;

		private PlayMakerArrayListProxy colliders;

		public override void Reset()
		{
			arrayListOwner = null;
			arrayListReference = null;
			ErrorEvent = null;
			scanRange = null;
			layerMask = new FsmInt[0];
			invertMask = false;
			ignoreTriggerColliders = false;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(arrayListOwner), arrayListReference.Value))
			{
				FindObjectInRange();
			}
			Finish();
		}

		private void FindObjectInRange()
		{
			if (!isProxyValid())
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(scanOrigin);
			GameObject ownerDefaultTarget2 = base.Fsm.GetOwnerDefaultTarget(arrayListOwner);
			float value = scanRange.Value;
			if (ignoreTriggerColliders.Value)
			{
				Collider[] array = Physics.OverlapSphere(ownerDefaultTarget.transform.position, value, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value), QueryTriggerInteraction.Ignore);
				if (array.Length == 0)
				{
					base.Fsm.Event(ErrorEvent);
				}
				Collider[] array2 = array;
				foreach (Collider collider in array2)
				{
					proxy.Add(collider.gameObject, "gameObject");
				}
			}
			else
			{
				Collider[] array3 = Physics.OverlapSphere(ownerDefaultTarget.transform.position, value, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value), QueryTriggerInteraction.Collide);
				if (array3.Length == 0)
				{
					base.Fsm.Event(ErrorEvent);
				}
				Collider[] array4 = array3;
				foreach (Collider collider2 in array4)
				{
					proxy.Add(collider2.gameObject, "gameObject");
				}
			}
			Finish();
		}
	}
}
