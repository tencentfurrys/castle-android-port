using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
	private AsyncOperation async;

	private int progress;

	public Slider LoadingSlider;

	public string LoadName;

	private void Start()
	{
		StartCoroutine(loadScene());
	}

	private IEnumerator loadScene()
	{
		int displayProgress = 0;
		int toProgress = 0;
		async = Application.LoadLevelAsync("LoadName");
		async.allowSceneActivation = false;
		while (async.progress < 0.9f)
		{
			toProgress = (int)async.progress * 100;
			while (displayProgress < toProgress)
			{
				displayProgress++;
				SetLoadingSlider(displayProgress);
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitForEndOfFrame();
		}
		toProgress = 100;
		while (displayProgress < toProgress)
		{
			displayProgress++;
			SetLoadingSlider(displayProgress);
			yield return new WaitForEndOfFrame();
		}
	}

	private void SetLoadingSlider(int progress)
	{
		float value = (float)progress / 100f;
		LoadingSlider.value = value;
	}
}
