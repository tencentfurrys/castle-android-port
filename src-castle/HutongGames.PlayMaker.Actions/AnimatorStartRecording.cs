using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("Animator")]
[Tooltip("Sets the animator in recording mode, and allocates a circular buffer of size frameCount. After this call, the recorder starts collecting up to frameCount frames in the buffer. Note it is not possible to start playback until a call to StopRecording is made")]
public class AnimatorStartRecording : FsmStateAction
{
	[CheckForComponent(typeof(Animator))]
	[RequiredField]
	[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
	public FsmOwnerDefault gameObject;

	[RequiredField]
	[Tooltip("The number of frames (updates) that will be recorded. If frameCount is 0, the recording will continue until the user calls StopRecording. The maximum value for frameCount is 10000.")]
	public FsmInt frameCount;

	public override void Reset()
	{
		gameObject = null;
		frameCount = 0;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget == null)
		{
			Finish();
			return;
		}
		Animator component = ownerDefaultTarget.GetComponent<Animator>();
		if (component != null)
		{
			component.StartRecording(frameCount.Value);
		}
		Finish();
	}
}
