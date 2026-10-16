using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=12323.0")]
	[ActionCategory("Garbage Collection")]
	[Tooltip("This strategy is often best for games that have long periods of gameplay where a smooth framerate is the main concern")]
	public class GarbageCollectionSmallHeap : FsmStateAction
	{
		[Tooltip("Allow Garbage Collection")]
		[ActionSection("Small Heap Setup")]
		public FsmBool collectionOn;

		public FsmInt frameCount;

		[ActionSection("Options")]
		[Tooltip("Stop the collection")]
		public FsmBool forceCancelled;

		public FsmEvent cancelledEvent;

		public override void Reset()
		{
			collectionOn = true;
			forceCancelled = false;
			cancelledEvent = null;
			frameCount = 30;
		}

		public override void OnEnter()
		{
		}

		public override void OnUpdate()
		{
			if (forceCancelled.Value)
			{
				base.Fsm.Event(cancelledEvent);
				Finish();
			}
			if (collectionOn.Value && Time.frameCount % frameCount.Value == 0)
			{
				GC.Collect();
			}
		}
	}
}
