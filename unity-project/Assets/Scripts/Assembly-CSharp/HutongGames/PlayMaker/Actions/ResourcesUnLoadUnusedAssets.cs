using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("UnLoads unused Resources. Be careful to use it only once at a time as it crates hickups especially on mobile.")]
	[ActionCategory("Resources")]
	public class ResourcesUnLoadUnusedAssets : FsmStateAction
	{
		public FsmEvent UnloadDoneEvent;

		private AsyncOperation _op;

		public override void Reset()
		{
			UnloadDoneEvent = null;
		}

		public override void OnEnter()
		{
			_op = Resources.UnloadUnusedAssets();
		}

		public override void OnUpdate()
		{
			if (_op != null && _op.isDone)
			{
				base.Fsm.Event(UnloadDoneEvent);
				Finish();
			}
		}
	}
}
