using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a trigger parameter to active or inactive. Triggers are parameters that act mostly like booleans, but get resets to inactive when they are used in a transition.")]
	[ActionCategory("Animator")]
	public class SetAnimatorTrigger : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The trigger name")]
		public FsmString trigger;

		private Animator _animator;

		private int _paramID;

		public override void Reset()
		{
			gameObject = null;
			trigger = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
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
			SetTrigger();
			Finish();
		}

		private void SetTrigger()
		{
			if (_animator != null)
			{
				_animator.SetTrigger(trigger.Value);
			}
		}
	}
}
