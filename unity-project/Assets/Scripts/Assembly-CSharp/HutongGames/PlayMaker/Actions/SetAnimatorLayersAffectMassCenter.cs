using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Animator")]
	[Tooltip("If true, additionnal layers affects the mass center")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1070")]
	public class SetAnimatorLayersAffectMassCenter : FsmStateAction
	{
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The Target. An Animator component is required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("If true, additionnal layers affects the mass center")]
		public FsmBool affectMassCenter;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			affectMassCenter = null;
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
			SetAffectMassCenter();
			Finish();
		}

		private void SetAffectMassCenter()
		{
			if (!(_animator == null))
			{
				_animator.layersAffectMassCenter = affectMassCenter.Value;
			}
		}
	}
}
