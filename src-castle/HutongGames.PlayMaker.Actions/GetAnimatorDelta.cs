using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("Animator")]
[Tooltip("Gets the avatar delta position and rotation for the last evaluated frame.")]
public class GetAnimatorDelta : FsmStateAction
{
	[CheckForComponent(typeof(Animator))]
	[RequiredField]
	[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
	public FsmOwnerDefault gameObject;

	[Tooltip("The avatar delta position for the last evaluated frame")]
	[UIHint(UIHint.Variable)]
	public FsmVector3 deltaPosition;

	[Tooltip("The avatar delta position for the last evaluated frame")]
	[UIHint(UIHint.Variable)]
	public FsmQuaternion deltaRotation;

	[Tooltip("Repeat every frame. Useful when changing over time.")]
	public bool everyFrame;

	private PlayMakerAnimatorMoveProxy _animatorProxy;

	private Transform _transform;

	private Animator _animator;

	public override void Reset()
	{
		gameObject = null;
		deltaPosition = null;
		deltaRotation = null;
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
		DoGetDeltaPosition();
		Finish();
	}

	public override void OnUpdate()
	{
		if (_animatorProxy == null)
		{
			DoGetDeltaPosition();
		}
	}

	public void OnAnimatorMoveEvent()
	{
		DoGetDeltaPosition();
	}

	private void DoGetDeltaPosition()
	{
		if (!(_animator == null))
		{
			deltaPosition.Value = _animator.deltaPosition;
			deltaRotation.Value = _animator.deltaRotation;
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
