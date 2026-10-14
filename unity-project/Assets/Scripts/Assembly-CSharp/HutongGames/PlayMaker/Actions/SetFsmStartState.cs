using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("set a new start position on an fsm. WARNING: use this action only on a start state!!")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmStartState : FsmStateAction
	{
		[Tooltip("Drag a PlayMakerFSM component here.")]
		public PlayMakerFSM fsmComponent;

		[Tooltip("If not specifyng the component above, specify the GameObject that owns the FSM")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of Fsm on Game Object. If left blank it will find the first PlayMakerFSM on the GameObject.")]
		public FsmString fsmName;

		[Tooltip("Store the state name in a string variable.")]
		[RequiredField]
		public FsmString stateName;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			fsmComponent = null;
			gameObject = null;
			fsmName = string.Empty;
			stateName = null;
		}

		public override void Awake()
		{
			DoSetFsmState();
			Finish();
		}

		private void DoSetFsmState()
		{
			if (!(fsm == null))
			{
				return;
			}
			if (fsmComponent != null)
			{
				fsm = fsmComponent;
			}
			else
			{
				GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
				if (ownerDefaultTarget != null)
				{
					fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
				}
			}
			if (fsm == null)
			{
				LogWarning("Could not find FSM: " + fsmName.Value);
			}
			else
			{
				fsm.Fsm.StartState = stateName.Value;
			}
		}
	}
}
