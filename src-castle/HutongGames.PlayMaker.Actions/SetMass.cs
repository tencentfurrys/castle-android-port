using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Sets the Mass of a Game Object's Rigid Body.")]
[ActionCategory(ActionCategory.Physics)]
public class SetMass : ComponentAction<Rigidbody>
{
	[CheckForComponent(typeof(Rigidbody))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[HasFloatSlider(0.1f, 10f)]
	[RequiredField]
	public FsmFloat mass;

	public override void Reset()
	{
		gameObject = null;
		mass = 1f;
	}

	public override void OnEnter()
	{
		DoSetMass();
		Finish();
	}

	private void DoSetMass()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			rigidbody.mass = mass.Value;
		}
	}
}
