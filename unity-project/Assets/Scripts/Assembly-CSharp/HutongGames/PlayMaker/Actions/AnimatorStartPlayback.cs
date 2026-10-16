using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Animator")]
	[Tooltip("Sets the animator in playback mode.")]
	public class AnimatorStartPlayback : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				Finish();
				return;
			}
			Animator component = ownerDefaultTarget.GetComponent<Animator>();
			if (component != null)
			{
				component.StartPlayback();
			}
			Finish();
		}
	}
}
