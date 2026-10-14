using UnityEngine;
using UnityEngine.Audio;

public class SoundOption : MonoBehaviour
{
	[SerializeField]
	private GameObject soundOptionCanvas;

	[SerializeField]
	private AudioMixerSnapshot gameSoundShot;

	[SerializeField]
	private AudioMixerSnapshot optionSoundShot;

	[SerializeField]
	private AudioMixer audioMixer;

	private void Update()
	{
		if (Input.GetKeyDown("4"))
		{
			soundOptionCanvas.SetActive(!soundOptionCanvas.activeSelf);
			if (soundOptionCanvas.activeSelf)
			{
				optionSoundShot.TransitionTo(0.01f);
			}
			else
			{
				gameSoundShot.TransitionTo(0.01f);
			}
		}
	}

	public void SetMaster(float volume)
	{
		audioMixer.SetFloat("MasterVol", volume);
	}

	public void SetBGM(float volume)
	{
		audioMixer.SetFloat("BGMVol", volume);
	}

	public void SetSE(float volume)
	{
		audioMixer.SetFloat("SEVol", volume);
	}
}
