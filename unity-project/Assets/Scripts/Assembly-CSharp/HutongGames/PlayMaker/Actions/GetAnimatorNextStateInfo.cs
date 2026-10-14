using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the next State information on a specified layer")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1054")]
	[ActionCategory("Animator")]
	public class GetAnimatorNextStateInfo : FsmStateAction
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

		[Tooltip("The layer's name.")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmString name;

		[Tooltip("The layer's name Hash. Obsolete in Unity 5, use fullPathHash or shortPathHash instead, nameHash will be the same as shortNameHash for legacy")]
		[UIHint(UIHint.Variable)]
		public FsmInt nameHash;

		[Tooltip("The full path hash for this state.")]
		[UIHint(UIHint.Variable)]
		public FsmInt fullPathHash;

		[Tooltip("The name Hash. Doest not include the parent layer's name")]
		[UIHint(UIHint.Variable)]
		public FsmInt shortPathHash;

		[Tooltip("The layer's tag hash")]
		[UIHint(UIHint.Variable)]
		public FsmInt tagHash;

		[UIHint(UIHint.Variable)]
		[Tooltip("Is the state looping. All animations in the state must be looping")]
		public FsmBool isStateLooping;

		[Tooltip("The Current duration of the state. In seconds, can vary when the State contains a Blend Tree ")]
		[UIHint(UIHint.Variable)]
		public FsmFloat length;

		[Tooltip("The integer part is the number of time a state has been looped. The fractional part is the % (0-1) of progress in the current loop")]
		[UIHint(UIHint.Variable)]
		public FsmFloat normalizedTime;

		[Tooltip("The integer part is the number of time a state has been looped. This is extracted from the normalizedTime")]
		[UIHint(UIHint.Variable)]
		public FsmInt loopCount;

		[UIHint(UIHint.Variable)]
		[Tooltip("The progress in the current loop. This is extracted from the normalizedTime")]
		public FsmFloat currentLoopProgress;

		private PlayMakerAnimatorMoveProxy _animatorProxy;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			layerIndex = null;
			name = null;
			nameHash = null;
			fullPathHash = null;
			shortPathHash = null;
			tagHash = null;
			length = null;
			normalizedTime = null;
			isStateLooping = null;
			loopCount = null;
			currentLoopProgress = null;
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
			_animatorProxy = ownerDefaultTarget.GetComponent<PlayMakerAnimatorMoveProxy>();
			if (_animatorProxy != null)
			{
				_animatorProxy.OnAnimatorMoveEvent += OnAnimatorMoveEvent;
			}
			GetLayerInfo();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public void OnAnimatorMoveEvent()
		{
			if (_animatorProxy != null)
			{
				GetLayerInfo();
			}
		}

		public override void OnUpdate()
		{
			if (_animatorProxy == null)
			{
				GetLayerInfo();
			}
		}

		private void GetLayerInfo()
		{
			if (_animator != null)
			{
				AnimatorStateInfo nextAnimatorStateInfo = _animator.GetNextAnimatorStateInfo(layerIndex.Value);
				fullPathHash.Value = nextAnimatorStateInfo.fullPathHash;
				shortPathHash.Value = nextAnimatorStateInfo.shortNameHash;
				nameHash.Value = nextAnimatorStateInfo.shortNameHash;
				if (!name.IsNone)
				{
					name.Value = _animator.GetLayerName(layerIndex.Value);
				}
				tagHash.Value = nextAnimatorStateInfo.tagHash;
				length.Value = nextAnimatorStateInfo.length;
				isStateLooping.Value = nextAnimatorStateInfo.loop;
				normalizedTime.Value = nextAnimatorStateInfo.normalizedTime;
				if (!loopCount.IsNone || !currentLoopProgress.IsNone)
				{
					loopCount.Value = (int)Math.Truncate(nextAnimatorStateInfo.normalizedTime);
					currentLoopProgress.Value = nextAnimatorStateInfo.normalizedTime - (float)loopCount.Value;
				}
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
}
