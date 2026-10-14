using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("Animator")]
[Tooltip("Gets the current transition information on a specified layer. Only valid when during a transition.")]
public class GetAnimatorCurrentTransitionInfo : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(Animator))]
	[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
	public FsmOwnerDefault gameObject;

	[Tooltip("The layer's index")]
	[RequiredField]
	public FsmInt layerIndex;

	[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
	public bool everyFrame;

	[ActionSection("Results")]
	[UIHint(UIHint.Variable)]
	[Tooltip("The unique name of the Transition")]
	public FsmString name;

	[UIHint(UIHint.Variable)]
	[Tooltip("The unique name of the Transition")]
	public FsmInt nameHash;

	[UIHint(UIHint.Variable)]
	[Tooltip("The user-specidied name of the Transition")]
	public FsmInt userNameHash;

	[Tooltip("Normalized time of the Transition")]
	[UIHint(UIHint.Variable)]
	public FsmFloat normalizedTime;

	private PlayMakerAnimatorMoveProxy _animatorProxy;

	private Animator _animator;

	public override void Reset()
	{
		gameObject = null;
		layerIndex = null;
		name = null;
		nameHash = null;
		userNameHash = null;
		normalizedTime = null;
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
		GetTransitionInfo();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		if (_animatorProxy == null)
		{
			GetTransitionInfo();
		}
	}

	public void OnAnimatorMoveEvent()
	{
		if (_animatorProxy != null)
		{
			GetTransitionInfo();
		}
	}

	private void GetTransitionInfo()
	{
		if (_animator != null)
		{
			AnimatorTransitionInfo animatorTransitionInfo = _animator.GetAnimatorTransitionInfo(layerIndex.Value);
			if (!name.IsNone)
			{
				name.Value = _animator.GetLayerName(layerIndex.Value);
			}
			nameHash.Value = animatorTransitionInfo.nameHash;
			userNameHash.Value = animatorTransitionInfo.userNameHash;
			normalizedTime.Value = animatorTransitionInfo.normalizedTime;
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
