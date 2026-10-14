using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Gets the value of a bool parameter")]
[ActionCategory("Animator")]
public class GetAnimatorBool : FsmStateAction
{
	[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
	[CheckForComponent(typeof(Animator))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("The animator parameter")]
	public FsmString parameter;

	[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
	public bool everyFrame;

	[RequiredField]
	[UIHint(UIHint.Variable)]
	[Tooltip("The bool value of the animator parameter")]
	[ActionSection("Results")]
	public FsmBool result;

	private PlayMakerAnimatorMoveProxy _animatorProxy;

	private Animator _animator;

	private int _paramID;

	public override void Reset()
	{
		gameObject = null;
		parameter = null;
		result = null;
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
		_paramID = Animator.StringToHash(parameter.Value);
		GetParameter();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public void OnAnimatorMoveEvent()
	{
		if (_animatorProxy != null)
		{
			GetParameter();
		}
	}

	public override void OnUpdate()
	{
		if (_animatorProxy == null)
		{
			GetParameter();
		}
	}

	private void GetParameter()
	{
		if (_animator != null)
		{
			result.Value = _animator.GetBool(_paramID);
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
