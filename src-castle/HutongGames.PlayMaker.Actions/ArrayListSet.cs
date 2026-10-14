namespace HutongGames.PlayMaker.Actions;

[ActionCategory("ArrayMaker/ArrayList")]
[Tooltip("Set an item at a specified index to a PlayMaker array List component")]
public class ArrayListSet : ArrayListActions
{
	[RequiredField]
	[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
	[CheckForComponent(typeof(PlayMakerArrayListProxy))]
	[ActionSection("Set up")]
	public FsmOwnerDefault gameObject;

	[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
	[UIHint(UIHint.FsmString)]
	public FsmString reference;

	[UIHint(UIHint.FsmString)]
	[Tooltip("The index of the Data in the ArrayList")]
	public FsmInt atIndex;

	[Tooltip("Will resize the ArrayList to the index if needed. else it fires an 'Out of bound' exception.'")]
	public bool forceResizeIdNeeded;

	public bool everyFrame;

	[Tooltip("The variable to add.")]
	[ActionSection("Data")]
	public FsmVar variable;

	public override void Reset()
	{
		gameObject = null;
		reference = null;
		variable = null;
		forceResizeIdNeeded = false;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
		{
			SetToArrayList();
		}
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		SetToArrayList();
	}

	public void SetToArrayList()
	{
		if (!isProxyValid())
		{
			return;
		}
		if (proxy._arrayList.Count <= atIndex.Value && forceResizeIdNeeded)
		{
			while (proxy._arrayList.Count <= atIndex.Value)
			{
				proxy._arrayList.Add(null);
			}
		}
		proxy.Set(atIndex.Value, PlayMakerUtils.GetValueFromFsmVar(Fsm, variable), variable.Type.ToString());
	}
}
