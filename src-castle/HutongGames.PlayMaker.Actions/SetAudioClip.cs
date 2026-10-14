using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Audio)]
[Tooltip("Sets the Audio Clip played by the AudioSource component on a Game Object.")]
public class SetAudioClip : ComponentAction<AudioSource>
{
	[RequiredField]
	[Tooltip("The GameObject with the AudioSource component.")]
	[CheckForComponent(typeof(AudioSource))]
	public FsmOwnerDefault gameObject;

	[ObjectType(typeof(AudioClip))]
	[Tooltip("The AudioClip to set.")]
	public FsmObject audioClip;

	public override void Reset()
	{
		gameObject = null;
		audioClip = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			audio.clip = audioClip.Value as AudioClip;
		}
		Finish();
	}
}
