using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Add an item to a PlayMaker Array List Proxy component")]
	public class ArrayListAdd : ArrayListActions
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
		[Tooltip("The variable to add.")]
		[ActionSection("Data")]
		public FsmVar variable;

		[Tooltip("Ints can be stored as bytes, useful when serializing over network for efficiency")]
		public bool convertIntToByte;

		[UIHint(UIHint.Variable)]
		[Tooltip("The index it was added at")]
		[ActionSection("Result")]
		public FsmInt index;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			variable = null;
			convertIntToByte = false;
			index = null;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				AddToArrayList();
			}
			Finish();
		}

		public void AddToArrayList()
		{
			if (isProxyValid())
			{
				object valueFromFsmVar = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable);
				if (variable.Type == VariableType.Int && convertIntToByte)
				{
					proxy.Add(Convert.ToByte(valueFromFsmVar), variable.Type.ToString());
				}
				else
				{
					proxy.Add(valueFromFsmVar, variable.Type.ToString());
				}
				index.Value = proxy.arrayList.Count - 1;
			}
		}
	}
}
