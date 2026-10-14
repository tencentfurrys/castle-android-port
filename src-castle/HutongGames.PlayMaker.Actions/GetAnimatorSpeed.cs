using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1056")]
[Tooltip("Gets the playback speed of the Animator. 1 is normal playback speed")]
[ActionCategory("Animator")]
public class GetAnimatorSpeed : FsmStateAction
{
	[Tooltip("The Target. An Animator component is required")]
	[CheckForComponent(typeof(Animator))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[UIHint(UIHint.Variable)]
	[Tooltip("The playBack speed of the animator. 1 is normal playback speed")]
	[RequiredField]
	public FsmFloat speed;

	[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
	public bool everyFrame;

	private Animator _animator;

	public override void Reset()
	{
		gameObject = null;
		speed = null;
		everyFrame = false;
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
		GetPlaybackSpeed();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		GetPlaybackSpeed();
	}

	private void GetPlaybackSpeed()
	{
		if (_animator != null)
		{
			speed.Value = _animator.speed;
		}
	}
}
