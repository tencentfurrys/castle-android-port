using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Animator")]
	[Tooltip("Gets the GameObject mapped to this human bone id")]
	public class GetAnimatorBoneGameObject : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The bone reference")]
		public HumanBodyBones bone;

		[Tooltip("The bone reference as a string, case insensitive")]
		public FsmString boneAsString;

		[ActionSection("Results")]
		[Tooltip("The Bone's GameObject")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject boneGameObject;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			bone = HumanBodyBones.Hips;
			boneAsString = new FsmString
			{
				UseVariable = false
			};
			boneGameObject = null;
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
			GetBoneTransform();
			Finish();
		}

		private void GetBoneTransform()
		{
			HumanBodyBones humanBoneId = ((!boneAsString.IsNone) ? ((HumanBodyBones)(int)Enum.Parse(typeof(HumanBodyBones), boneAsString.Value, true)) : bone);
			boneGameObject.Value = _animator.GetBoneTransform(humanBoneId).gameObject;
		}
	}
}
