using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Parent of a Game Object. It uses the Transform.SetParent method")]
	[ActionCategory(ActionCategory.GameObject)]
	public class SetTransformParent : FsmStateAction
	{
		[Tooltip("The Game Object to parent.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The new parent for the Game Object.")]
		public FsmGameObject parent;

		[Tooltip("If true, the parent-relative position, scale and rotation is modified such that the object keeps the same world space position, rotation and scale as before.")]
		public FsmBool worldPositionStays;

		public override void Reset()
		{
			gameObject = null;
			parent = null;
			worldPositionStays = true;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			GameObject value = parent.Value;
			Transform transform = null;
			if (value != null)
			{
				transform = value.transform;
			}
			if (ownerDefaultTarget != null)
			{
				ownerDefaultTarget.transform.SetParent(transform, worldPositionStays.Value);
			}
			Finish();
		}
	}
}
