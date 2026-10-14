using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Returns true if the current rig is humanoid, false if it is generic. Can also sends events")]
[ActionCategory("Animator")]
public class GetAnimatorIsHuman : FsmStateAction
{
	[CheckForComponent(typeof(Animator))]
	[Tooltip("The Target. An Animator component is required")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[ActionSection("Results")]
	[Tooltip("True if the current rig is humanoid, False if it is generic")]
	[UIHint(UIHint.Variable)]
	public FsmBool isHuman;

	[Tooltip("Event send if rig is humanoid")]
	public FsmEvent isHumanEvent;

	[Tooltip("Event send if rig is generic")]
	public FsmEvent isGenericEvent;

	private Animator _animator;

	public override void Reset()
	{
		gameObject = null;
		isHuman = null;
		isHumanEvent = null;
		isGenericEvent = null;
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
		DoCheckIsHuman();
		Finish();
	}

	private void DoCheckIsHuman()
	{
		if (!(_animator == null))
		{
			bool flag = _animator.isHuman;
			isHuman.Value = flag;
			if (flag)
			{
				Fsm.Event(isHumanEvent);
			}
			else
			{
				Fsm.Event(isGenericEvent);
			}
		}
	}
}
