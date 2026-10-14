namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Count items from a PlayMaker ArrayList Proxy component")]
	public class ArrayListCount : ArrayListActions
	{
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[ActionSection("Set up")]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		public bool everyframe;

		[ActionSection("Result")]
		[Tooltip("Store the count value")]
		[UIHint(UIHint.Variable)]
		public FsmInt count;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			count = null;
			everyframe = false;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				getArrayListCount();
			}
			if (!everyframe)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			getArrayListCount();
		}

		public void getArrayListCount()
		{
			if (isProxyValid())
			{
				int value = proxy.arrayList.Count;
				count.Value = value;
			}
		}
	}
}
