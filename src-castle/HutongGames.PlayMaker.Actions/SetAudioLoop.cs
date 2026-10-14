using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Sets looping on the AudioSource component on a Game Object.")]
[ActionCategory(ActionCategory.Audio)]
public class SetAudioLoop : ComponentAction<AudioSource>
{
	[RequiredField]
	[CheckForComponent(typeof(AudioSource))]
	public FsmOwnerDefault gameObject;

	public FsmBool loop;

	public override void Reset()
	{
		gameObject = null;
		loop = false;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			audio.loop = loop.Value;
		}
		Finish();
	}
}
