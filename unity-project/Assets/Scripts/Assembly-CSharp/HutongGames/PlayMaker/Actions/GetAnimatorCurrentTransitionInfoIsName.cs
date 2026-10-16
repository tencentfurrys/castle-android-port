using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Animator")]
	[Tooltip("Check the active Transition name on a specified layer. Format is 'CURRENT_STATE -> NEXT_STATE'.")]
	public class GetAnimatorCurrentTransitionInfoIsName : FsmStateAction
	{
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		[Tooltip("The name to check the transition against.")]
		public FsmString name;

		public bool everyFrame;

		[ActionSection("Results")]
		public FsmBool nameMatch;

		public FsmEvent nameMatchEvent;

		public FsmEvent nameDoNotMatchEvent;

		private PlayMakerAnimatorMoveProxy _animatorProxy;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			layerIndex = null;
			name = null;
			nameMatch = null;
			nameMatchEvent = null;
			nameDoNotMatchEvent = null;
			everyFrame = false;
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
			IsName();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			IsName();
		}

		private void IsName()
		{
			if (_animator != null)
			{
				if (_animator.GetAnimatorTransitionInfo(layerIndex.Value).IsName(name.Value))
				{
					nameMatch.Value = true;
					base.Fsm.Event(nameMatchEvent);
				}
				else
				{
					nameMatch.Value = false;
					base.Fsm.Event(nameDoNotMatchEvent);
				}
			}
		}
	}
}
