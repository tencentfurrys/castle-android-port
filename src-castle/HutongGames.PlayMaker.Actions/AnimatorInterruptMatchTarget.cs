using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Interrupts the automatic target matching. CompleteMatch will make the gameobject match the target completely at the next frame.")]
[ActionCategory("Animator")]
public class AnimatorInterruptMatchTarget : FsmStateAction
{
	[CheckForComponent(typeof(Animator))]
	[RequiredField]
	[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
	public FsmOwnerDefault gameObject;

	[Tooltip("Will make the gameobject match the target completely at the next frame")]
	public FsmBool completeMatch;

	public override void Reset()
	{
		gameObject = null;
		completeMatch = true;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget == null)
		{
			Finish();
			return;
		}
		Animator component = ownerDefaultTarget.GetComponent<Animator>();
		if (component != null)
		{
			component.InterruptMatchTarget(completeMatch.Value);
		}
		Finish();
	}
}
