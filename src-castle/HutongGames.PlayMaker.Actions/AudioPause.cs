using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Pauses playing the Audio Clip played by an Audio Source component on a Game Object.")]
[ActionCategory(ActionCategory.Audio)]
public class AudioPause : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(AudioSource))]
	[Tooltip("The GameObject with an Audio Source component.")]
	public FsmOwnerDefault gameObject;

	public override void Reset()
	{
		gameObject = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			AudioSource component = ownerDefaultTarget.GetComponent<AudioSource>();
			if (component != null)
			{
				component.Pause();
			}
		}
		Finish();
	}
}
