using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Does tag match the tag of the active state in the statemachine")]
[ActionCategory("Animator")]
public class GetAnimatorCurrentStateInfoIsTag : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(Animator))]
	[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
	public FsmOwnerDefault gameObject;

	[Tooltip("The layer's index")]
	[RequiredField]
	public FsmInt layerIndex;

	[Tooltip("The tag to check the layer against.")]
	public FsmString tag;

	public bool everyFrame;

	[ActionSection("Results")]
	public FsmBool tagMatch;

	public FsmEvent tagMatchEvent;

	public FsmEvent tagDoNotMatchEvent;

	private PlayMakerAnimatorMoveProxy _animatorProxy;

	private Animator _animator;

	public override void Reset()
	{
		gameObject = null;
		layerIndex = null;
		tag = null;
		tagMatch = null;
		tagMatchEvent = null;
		tagDoNotMatchEvent = null;
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
		IsTag();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		IsTag();
	}

	private void IsTag()
	{
		if (_animator != null)
		{
			if (_animator.GetCurrentAnimatorStateInfo(layerIndex.Value).IsTag(tag.Value))
			{
				tagMatch.Value = true;
				Fsm.Event(tagMatchEvent);
			}
			else
			{
				tagMatch.Value = false;
				Fsm.Event(tagDoNotMatchEvent);
			}
		}
	}
}
