using System;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Add several items to a PlayMaker Array List Proxy component")]
[ActionCategory("ArrayMaker/ArrayList")]
public class ArrayListAddRange : ArrayListActions
{
	[ActionSection("Set up")]
	[RequiredField]
	[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
	[CheckForComponent(typeof(PlayMakerArrayListProxy))]
	public FsmOwnerDefault gameObject;

	[UIHint(UIHint.FsmString)]
	[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
	public FsmString reference;

	[RequiredField]
	[Tooltip("The variables to add.")]
	[ActionSection("Data")]
	public FsmVar[] variables;

	[Tooltip("Ints can be stored as bytes, useful when serializing over network for efficiency")]
	public bool convertIntsToBytes;

	public override void Reset()
	{
		gameObject = null;
		reference = null;
		variables = new FsmVar[2];
		convertIntsToBytes = false;
	}

	public override void OnEnter()
	{
		if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
		{
			DoArrayListAddRange();
		}
		Finish();
	}

	public void DoArrayListAddRange()
	{
		if (!isProxyValid())
		{
			return;
		}
		FsmVar[] array = variables;
		foreach (FsmVar fsmVar in array)
		{
			object valueFromFsmVar = PlayMakerUtils.GetValueFromFsmVar(Fsm, fsmVar);
			if (fsmVar.Type == VariableType.Int && convertIntsToBytes)
			{
				proxy.Add(Convert.ToByte(valueFromFsmVar), fsmVar.Type.ToString(), silent: true);
			}
			else
			{
				proxy.Add(valueFromFsmVar, fsmVar.Type.ToString(), silent: true);
			}
		}
	}
}
