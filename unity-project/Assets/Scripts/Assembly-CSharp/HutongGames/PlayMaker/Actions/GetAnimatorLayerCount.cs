using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns the Animator controller layer count")]
	[ActionCategory("Animator")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1051")]
	public class GetAnimatorLayerCount : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The Target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The Animator controller layer count")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[ActionSection("Results")]
		public FsmInt layerCount;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			layerCount = null;
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
			DoGetLayerCount();
			Finish();
		}

		private void DoGetLayerCount()
		{
			if (!(_animator == null))
			{
				layerCount.Value = _animator.layerCount;
			}
		}
	}
}
