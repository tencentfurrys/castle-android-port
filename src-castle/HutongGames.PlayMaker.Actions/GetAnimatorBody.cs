using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("Animator")]
[Tooltip("Gets the avatar body mass center position and rotation.Optionally accept a GameObject to get the body transform. \nThe position and rotation are local to the gameobject")]
[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1036")]
public class GetAnimatorBody : FsmStateAction
{
	[CheckForComponent(typeof(Animator))]
	[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("Repeat every frame. Useful when changing over time.")]
	public bool everyFrame;

	[UIHint(UIHint.Variable)]
	[Tooltip("The avatar body mass center")]
	[ActionSection("Results")]
	public FsmVector3 bodyPosition;

	[UIHint(UIHint.Variable)]
	[Tooltip("The avatar body mass center")]
	public FsmQuaternion bodyRotation;

	[Tooltip("If set, apply the body mass center position and rotation to this gameObject")]
	public FsmGameObject bodyGameObject;

	private PlayMakerAnimatorMoveProxy _animatorProxy;

	private Animator _animator;

	private Transform _transform;

	public override void Reset()
	{
		gameObject = null;
		bodyPosition = null;
		bodyRotation = null;
		bodyGameObject = null;
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
		GameObject value = bodyGameObject.Value;
		if (value != null)
		{
			_transform = value.transform;
		}
		DoGetBodyPosition();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public void OnAnimatorMoveEvent()
	{
		if (_animatorProxy != null)
		{
			DoGetBodyPosition();
		}
	}

	public override void OnUpdate()
	{
		if (_animatorProxy == null)
		{
			DoGetBodyPosition();
		}
	}

	private void DoGetBodyPosition()
	{
		if (!(_animator == null))
		{
			bodyPosition.Value = _animator.bodyPosition;
			bodyRotation.Value = _animator.bodyRotation;
			if (_transform != null)
			{
				_transform.position = _animator.bodyPosition;
				_transform.rotation = _animator.bodyRotation;
			}
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
