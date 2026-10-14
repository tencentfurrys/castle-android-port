using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("This strategy works best for games where allocations (and therefore collections) are relatively infrequent and can be handled during pauses in gameplay.")]
[ActionCategory("Garbage Collection")]
[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=12323.0")]
public class GarbageCollectionLargeHeap : FsmStateAction
{
	[Tooltip("!!! Power of two - default is 1024")]
	[ActionSection("Large Heap Setup")]
	public FsmInt byteSize;

	public override void Reset()
	{
		byteSize = 1024;
	}

	public override void OnEnter()
	{
		if (IsPowerOfTwo(byteSize.Value))
		{
			object[] array = new object[byteSize.Value];
			for (int i = 0; i < byteSize.Value; i++)
			{
				array[i] = new byte[byteSize.Value];
			}
			array = null;
		}
		else
		{
			Debug.LogWarning("<b>[GarbageCollectionLargeHeap]</b><color=#FF9900ff>  'byteSize' number is not power of 2 - nothing happened  - Please review!</color>", Owner);
		}
		Finish();
	}

	private bool IsPowerOfTwo(int x)
	{
		return (x & (x - 1)) == 0;
	}
}
