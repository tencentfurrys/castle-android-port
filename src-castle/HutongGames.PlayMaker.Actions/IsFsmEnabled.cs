using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.ScriptControl)]
[Tooltip("Gets the Enabled state of a Fsm Component.")]
public class IsFsmEnabled : FsmStateAction
{
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[UIHint(UIHint.FsmName)]
	[Tooltip("Optional name of FSM on Game Object")]
	public FsmString fsmName;

	[UIHint(UIHint.Variable)]
	[Tooltip("Store the enable state of the targeted fsm")]
	public FsmBool isEnabled;

	[Tooltip("Event to send if Fsm is enabled")]
	public FsmEvent isEnabledEvent;

	[Tooltip("Event to send if Fsm is disabled")]
	public FsmEvent isDisabledEvent;

	public bool everyFrame;

	private PlayMakerFSM fsm;

	public override void Reset()
	{
		gameObject = null;
		fsmName = string.Empty;
		isEnabled = null;
		isEnabledEvent = null;
		isDisabledEvent = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
		}
		DoGetFsmEnableState();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoGetFsmEnableState();
	}

	private void DoGetFsmEnableState()
	{
		if (!(fsm == null))
		{
			bool flag = fsm.enabled;
			isEnabled.Value = flag;
			if (flag)
			{
				Fsm.Event(isEnabledEvent);
			}
			else
			{
				Fsm.Event(isDisabledEvent);
			}
		}
	}
}
