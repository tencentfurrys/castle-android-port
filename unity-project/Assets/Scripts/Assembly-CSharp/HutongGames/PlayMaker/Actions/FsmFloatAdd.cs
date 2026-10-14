using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Adds a value to a Float Variable in another FSM.")]
	public class FsmFloatAdd : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmFloat)]
		public FsmString variableName;

		[RequiredField]
		public FsmFloat add;

		public bool everyFrame;

		public bool perSecond;

		private GameObject goLastFrame;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = null;
			variableName = null;
			add = null;
			everyFrame = false;
			perSecond = false;
		}

		public override void OnEnter()
		{
			DoSetFsmFloatAdd();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmFloatAdd();
		}

		private void DoSetFsmFloatAdd()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != goLastFrame)
			{
				goLastFrame = ownerDefaultTarget;
				fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
			}
			if (fsm == null)
			{
				return;
			}
			FsmFloat fsmFloat = fsm.FsmVariables.GetFsmFloat(variableName.Value);
			if (fsmFloat != null)
			{
				if (!perSecond)
				{
					fsmFloat.Value += add.Value;
				}
				else
				{
					fsmFloat.Value += add.Value * Time.deltaTime;
				}
			}
		}
	}
}
