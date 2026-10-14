using UnityEngine.Audio;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Audio)]
[Tooltip("Sets the float value of an exposed parameter for a Unity Audio Mixer.")]
public class AudioMixerSetFloatValue : FsmStateAction
{
	[RequiredField]
	[ObjectType(typeof(AudioMixer))]
	[Tooltip("The Audio Mixer with the exposed parameter.")]
	public FsmObject theMixer;

	[Tooltip("The name of the exposed parameter.")]
	[Title("Name of Parameter")]
	[RequiredField]
	public FsmString exposedFloatName;

	[Title("Float Value")]
	public FsmFloat floatvalue;

	public bool everyFrame;

	public override void Reset()
	{
		theMixer = null;
		exposedFloatName = null;
		floatvalue = null;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		DoSetMixerFloatValue();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoSetMixerFloatValue();
	}

	public void DoSetMixerFloatValue()
	{
		AudioMixer audioMixer = theMixer.Value as AudioMixer;
		if (audioMixer != null && !string.IsNullOrEmpty(exposedFloatName.Value))
		{
			string value = exposedFloatName.Value;
			float value2 = floatvalue.Value;
			audioMixer.SetFloat(value, value2);
		}
	}
}
