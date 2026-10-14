using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Physics)]
[Tooltip("Sets whether a Game Object's Rigidy Body is affected by Gravity.")]
public class UseGravity : ComponentAction<Rigidbody>
{
	[CheckForComponent(typeof(Rigidbody))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[RequiredField]
	public FsmBool useGravity;

	public override void Reset()
	{
		gameObject = null;
		useGravity = true;
	}

	public override void OnEnter()
	{
		DoUseGravity();
		Finish();
	}

	private void DoUseGravity()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			rigidbody.useGravity = useGravity.Value;
		}
	}
}
