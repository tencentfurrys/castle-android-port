using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Animator")]
	[Tooltip("Check the current State name on a specified layer, this is more than the layer name, it holds the current state as well.")]
	public class GetAnimatorCurrentStateInfoIsName : FsmStateAction
	{
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The layer's index")]
		public FsmInt layerIndex;

		[Tooltip("The name to check the layer against.")]
		public FsmString name;

		public bool everyFrame;

		[ActionSection("Results")]
		public FsmEvent nameMatchEvent;

		public FsmEvent nameDoNotMatchEvent;

		private PlayMakerAnimatorMoveProxy _animatorProxy;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			layerIndex = null;
			name = null;
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
				if (_animator.GetCurrentAnimatorStateInfo(layerIndex.Value).IsName(name.Value))
				{
					base.Fsm.Event(nameMatchEvent);
				}
				else
				{
					base.Fsm.Event(nameDoNotMatchEvent);
				}
			}
		}
	}
}
