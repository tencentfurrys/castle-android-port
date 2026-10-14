using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Sets the Parent of a Game Object. Same as official action but lt you use 'owner' for the parent for convenience.")]
[ActionCategory(ActionCategory.GameObject)]
public class SetParent2 : FsmStateAction
{
	[Tooltip("The Game Object to parent.")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("The new parent for the Game Object.")]
	public FsmOwnerDefault parent;

	[Tooltip("Set the local position to 0,0,0 after parenting.")]
	public FsmBool resetLocalPosition;

	[Tooltip("Set the local rotation to 0,0,0 after parenting.")]
	public FsmBool resetLocalRotation;

	public override void Reset()
	{
		gameObject = new FsmOwnerDefault();
		gameObject.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
		parent = null;
		resetLocalPosition = null;
		resetLocalRotation = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		GameObject ownerDefaultTarget2 = Fsm.GetOwnerDefaultTarget(parent);
		if (ownerDefaultTarget2 == ownerDefaultTarget)
		{
			LogError("Can not parent to self");
		}
		if (ownerDefaultTarget != null)
		{
			ownerDefaultTarget.transform.parent = ((!(ownerDefaultTarget2 == null)) ? ownerDefaultTarget2.transform : null);
			if (resetLocalPosition.Value)
			{
				ownerDefaultTarget.transform.localPosition = Vector3.zero;
			}
			if (resetLocalRotation.Value)
			{
				ownerDefaultTarget.transform.localRotation = Quaternion.identity;
			}
		}
		Finish();
	}

	public override string ErrorCheck()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		GameObject ownerDefaultTarget2 = Fsm.GetOwnerDefaultTarget(parent);
		if (ownerDefaultTarget2 != null && ownerDefaultTarget2 == ownerDefaultTarget)
		{
			return "Can not parent to self";
		}
		return string.Empty;
	}
}
