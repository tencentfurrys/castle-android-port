using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Gets the layer's current weight")]
[ActionCategory("Animator")]
[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1052")]
public class GetAnimatorLayerWeight : FsmStateAction
{
	[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
	[RequiredField]
	[CheckForComponent(typeof(Animator))]
	public FsmOwnerDefault gameObject;

	[Tooltip("The layer's index")]
	[RequiredField]
	public FsmInt layerIndex;

	[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
	public bool everyFrame;

	[RequiredField]
	[Tooltip("The layer's current weight")]
	[ActionSection("Results")]
	[UIHint(UIHint.Variable)]
	public FsmFloat layerWeight;

	private PlayMakerAnimatorMoveProxy _animatorProxy;

	private Animator _animator;

	public override void Reset()
	{
		gameObject = null;
		layerIndex = null;
		layerWeight = null;
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
		GetLayerWeight();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public void OnAnimatorMoveEvent()
	{
		if (_animatorProxy != null)
		{
			GetLayerWeight();
		}
	}

	public override void OnUpdate()
	{
		if (_animatorProxy == null)
		{
			GetLayerWeight();
		}
	}

	private void GetLayerWeight()
	{
		if (_animator != null)
		{
			layerWeight.Value = _animator.GetLayerWeight(layerIndex.Value);
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
