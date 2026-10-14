using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Returns The current gravity weight based on current animations that are played")]
[ActionCategory("Animator")]
public class GetAnimatorGravityWeight : FsmStateAction
{
	[CheckForComponent(typeof(Animator))]
	[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
	public bool everyFrame;

	[ActionSection("Results")]
	[Tooltip("The current gravity weight based on current animations that are played")]
	[UIHint(UIHint.Variable)]
	public FsmFloat gravityWeight;

	private PlayMakerAnimatorMoveProxy _animatorProxy;

	private Animator _animator;

	public override void Reset()
	{
		gameObject = null;
		gravityWeight = null;
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
		_animatorProxy = ownerDefaultTarget.GetComponent<PlayMakerAnimatorMoveProxy>();
		if (_animatorProxy != null)
		{
			_animatorProxy.OnAnimatorMoveEvent += OnAnimatorMoveEvent;
		}
		DoGetGravityWeight();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public void OnAnimatorMoveEvent()
	{
		if (_animatorProxy != null)
		{
			DoGetGravityWeight();
		}
	}

	public override void OnUpdate()
	{
		if (_animatorProxy == null)
		{
			DoGetGravityWeight();
		}
	}

	private void DoGetGravityWeight()
	{
		if (!(_animator == null))
		{
			gravityWeight.Value = _animator.gravityWeight;
		}
	}

	public override void OnExit()
	{
		if (_animatorProxy != null)
		{
			_animatorProxy.OnAnimatorMoveEvent -= OnAnimatorMoveEvent;
		}
	}
}
