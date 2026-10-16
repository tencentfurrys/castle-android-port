using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the current State information on a specified layer")]
	[ActionCategory("Animator")]
	public class GetAnimatorCurrentStateInfo : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The layer's index")]
		public FsmInt layerIndex;

		[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
		public bool everyFrame;

		[Tooltip("The layer's name.")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmString name;

		[UIHint(UIHint.Variable)]
		[Tooltip("The layer's name Hash. Obsolete in Unity 5, use fullPathHash or shortPathHash instead, nameHash will be the same as shortNameHash for legacy")]
		public FsmInt nameHash;

		[UIHint(UIHint.Variable)]
		[Tooltip("The full path hash for this state.")]
		public FsmInt fullPathHash;

		[Tooltip("The name Hash. Doest not include the parent layer's name")]
		[UIHint(UIHint.Variable)]
		public FsmInt shortPathHash;

		[Tooltip("The layer's tag hash")]
		[UIHint(UIHint.Variable)]
		public FsmInt tagHash;

		[Tooltip("Is the state looping. All animations in the state must be looping")]
		[UIHint(UIHint.Variable)]
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

		public override void OnUpdate()
		{
			if (_animatorProxy == null)
			{
				GetLayerInfo();
			}
		}

		public void OnAnimatorMoveEvent()
		{
			if (_animatorProxy != null)
			{
				GetLayerInfo();
			}
		}

		private void GetLayerInfo()
		{
			if (_animator != null)
			{
				AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(layerIndex.Value);
				fullPathHash.Value = currentAnimatorStateInfo.fullPathHash;
				shortPathHash.Value = currentAnimatorStateInfo.shortNameHash;
				nameHash.Value = currentAnimatorStateInfo.shortNameHash;
				if (!name.IsNone)
				{
					name.Value = _animator.GetLayerName(layerIndex.Value);
				}
				tagHash.Value = currentAnimatorStateInfo.tagHash;
				length.Value = currentAnimatorStateInfo.length;
				isStateLooping.Value = currentAnimatorStateInfo.loop;
				normalizedTime.Value = currentAnimatorStateInfo.normalizedTime;
				if (!loopCount.IsNone || !currentLoopProgress.IsNone)
				{
					loopCount.Value = (int)Math.Truncate(currentAnimatorStateInfo.normalizedTime);
					currentLoopProgress.Value = currentAnimatorStateInfo.normalizedTime - (float)loopCount.Value;
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
