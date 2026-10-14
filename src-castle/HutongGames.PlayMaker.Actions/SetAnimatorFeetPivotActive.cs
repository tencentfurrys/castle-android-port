using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1065")]
[Tooltip("Activates feet pivot. At 0% blending point is body mass center. At 100% blending point is feet pivot")]
[ActionCategory("Animator")]
public class SetAnimatorFeetPivotActive : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(Animator))]
	[Tooltip("The Target. An Animator component is required")]
	public FsmOwnerDefault gameObject;

	[Tooltip("Activates feet pivot. At 0% blending point is body mass center. At 100% blending point is feet pivot")]
	public FsmFloat feetPivotActive;

	private Animator _animator;

	public override void Reset()
	{
		gameObject = null;
		feetPivotActive = null;
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
		DoFeetPivotActive();
		Finish();
	}

	private void DoFeetPivotActive()
	{
		if (!(_animator == null))
		{
			_animator.feetPivotActive = feetPivotActive.Value;
		}
	}
}
