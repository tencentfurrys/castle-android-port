using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Obsolete("This action is obsolete. Use mask and layers to control subset of transforms in a skeleton")]
	[ActionCategory("Animator")]
	[Tooltip("Returns true if a transform is controlled by the Animator. Can also send events")]
	public class GetAnimatorIsControlled : FsmStateAction
	{
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The Target. An Animator component is required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The GameObject transform to check for control.")]
		[RequiredField]
		public FsmGameObject transform;

		[Tooltip("True if automatic matching is active")]
		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		public FsmBool isControlled;

		[Tooltip("Event send if the transform is controlled")]
		public FsmEvent isControlledEvent;

		[Tooltip("Event send if the transform is not controlled")]
		public FsmEvent isNotControlledEvent;

		private Animator _animator;

		private Transform _transform;

		public override void Reset()
		{
			gameObject = null;
			transform = null;
			isControlled = null;
			isControlledEvent = null;
			isNotControlledEvent = null;
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
			GameObject value = transform.Value;
			if (value == null)
			{
				Finish();
				return;
			}
			_transform = value.transform;
			DoCheckIsControlled();
			Finish();
		}

		private void DoCheckIsControlled()
		{
			if (!(_animator == null))
			{
				bool flag = _animator.IsControlled(_transform);
				isControlled.Value = flag;
				if (flag)
				{
					base.Fsm.Event(isControlledEvent);
				}
				else
				{
					base.Fsm.Event(isNotControlledEvent);
				}
			}
		}
	}
}
