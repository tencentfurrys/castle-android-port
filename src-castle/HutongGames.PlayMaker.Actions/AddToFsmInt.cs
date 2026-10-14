using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Adds a value to an fsm Integer Variable.")]
[ActionCategory(ActionCategory.StateMachine)]
public class AddToFsmInt : FsmStateAction
{
	[Tooltip("The GameObject that owns the FSM.")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[UIHint(UIHint.FsmName)]
	[Tooltip("Optional name of FSM on Game Object")]
	public FsmString fsmName;

	[RequiredField]
	[UIHint(UIHint.FsmInt)]
	public FsmString variableName;

	[RequiredField]
	[Tooltip("Repeat every frame. Useful if the value is changing.")]
	public FsmInt add;

	[Tooltip("Optional storage of the result")]
	[UIHint(UIHint.Variable)]
	public FsmInt storeResult;

	[Tooltip("Repeat every frame. Useful if the value is changing.")]
	public bool everyFrame;

	private GameObject goLastFrame;

	private PlayMakerFSM fsm;

	public override void Reset()
	{
		gameObject = null;
		add = null;
		fsmName = string.Empty;
		storeResult = null;
	}

	public override void OnEnter()
	{
		DoAddToFsmInt();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoAddToFsmInt();
	}

	private void DoAddToFsmInt()
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
		if (fsm == null)
		{
			return;
		}
		FsmInt fsmInt = fsm.FsmVariables.GetFsmInt(variableName.Value);
		if (fsmInt != null)
		{
			fsmInt.Value += add.Value;
			if (storeResult != null)
			{
				storeResult.Value = fsmInt.Value;
			}
		}
	}
}
