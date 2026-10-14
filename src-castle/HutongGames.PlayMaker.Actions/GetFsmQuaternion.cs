using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Get the value of a Quaternion Variable from another FSM.")]
[ActionCategory(ActionCategory.StateMachine)]
public class GetFsmQuaternion : FsmStateAction
{
	[RequiredField]
	[Tooltip("The GameObject that owns the FSM.")]
	public FsmOwnerDefault gameObject;

	[Tooltip("Optional name of FSM on Game Object")]
	[UIHint(UIHint.FsmName)]
	public FsmString fsmName;

	[UIHint(UIHint.FsmQuaternion)]
	[RequiredField]
	public FsmString variableName;

	[RequiredField]
	[UIHint(UIHint.Variable)]
	public FsmQuaternion storeValue;

	[Tooltip("Repeat every frame.")]
	public bool everyFrame;

	private GameObject goLastFrame;

	protected PlayMakerFSM fsm;

	public override void Reset()
	{
		gameObject = null;
		fsmName = string.Empty;
		variableName = string.Empty;
		storeValue = null;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		DoGetFsmVariable();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoGetFsmVariable();
	}

	private void DoGetFsmVariable()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget == null)
		{
			return;
		}
		if (ownerDefaultTarget != goLastFrame)
		{
			goLastFrame = ownerDefaultTarget;
			fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
		}
		if (!(fsm == null) && storeValue != null)
		{
			FsmQuaternion fsmQuaternion = fsm.FsmVariables.GetFsmQuaternion(variableName.Value);
			if (fsmQuaternion != null)
			{
				storeValue.Value = fsmQuaternion.Value;
			}
		}
	}
}
