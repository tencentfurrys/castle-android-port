using System;

namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=12323.0")]
	[Tooltip("Request a collection explicitly")]
	[ActionCategory("Garbage Collection")]
	public class GarbageCollection : FsmStateAction
	{
		[Tooltip("Allow Garbage Collection")]
		public FsmBool collectionOn;

		public override void Reset()
		{
			collectionOn = true;
		}

		public override void OnEnter()
		{
			if (collectionOn.Value)
			{
				GC.Collect();
			}
			Finish();
		}
	}
}
