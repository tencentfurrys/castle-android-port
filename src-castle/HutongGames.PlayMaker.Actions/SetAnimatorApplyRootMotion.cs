using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("Animator")]
[Tooltip("Set Apply Root Motion: If true, Root is controlled by animations")]
[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1061")]
public class SetAnimatorApplyRootMotion : FsmStateAction
{
	[CheckForComponent(typeof(Animator))]
	[Tooltip("The Target. An Animator component is required")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("If true, Root is controlled by animations")]
	public FsmBool applyRootMotion;

	private Animator _animator;

	public override void Reset()
	{
		gameObject = null;
		applyRootMotion = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget == null)
		{
			Finish();
			return;
		}
		_animator = ownerDefaultTarget.GetComponent<Animator>();
		if (_animator == null)
		{
			Finish();
			return;
		}
		DoApplyRootMotion();
		Finish();
	}

	private void DoApplyRootMotion()
	{
		if (!(_animator == null))
		{
			_animator.applyRootMotion = applyRootMotion.Value;
		}
	}
}
