using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Shuriken")]
	[Tooltip("Stops a particleSystem")]
	public class ParticleSystemStop : FsmStateAction
	{
		[CheckForComponent(typeof(ParticleSystem))]
		[Tooltip("the GameObject with the particleSystem to stop")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("start playing again when state exits")]
		public FsmBool playOnExit;

		private ParticleSystem _ps;

		public override void Reset()
		{
			gameObject = null;
			playOnExit = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				_ps = ownerDefaultTarget.GetComponent<ParticleSystem>();
				if (!(_ps == null))
				{
					_ps.Stop();
					Finish();
				}
			}
		}

		public override void OnExit()
		{
			if (_ps != null && playOnExit.Value)
			{
				_ps.Play();
			}
		}
	}
}
