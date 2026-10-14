using System.Collections;
using UnityEngine;

public class LoadingScript : MonoBehaviour
{
	private AsyncOperation async;

	private int progress;

	public int LoadNumber;

	public int displayProgress;

	public bool ChangeScene;

	public GameObject KeepObject;

	private void Start()
	{
		Object.DontDestroyOnLoad(KeepObject);
		StartCoroutine(loadScene());
	}

	private void Update()
	{
		if (ChangeScene)
		{
			StartCoroutine(loadScene());
		}
	}

	private IEnumerator loadScene()
	{
		if (ChangeScene)
		{
			async.allowSceneActivation = true;
			yield return new WaitForEndOfFrame();
		}
		int toProgress = 0;
		async = Application.LoadLevelAsync(LoadNumber);
		async.allowSceneActivation = false;
		while (async.progress < 0.9f)
		{
			toProgress = (int)async.progress * 100;
			while (displayProgress < toProgress)
			{
				displayProgress++;
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitForEndOfFrame();
		}
		toProgress = 100;
		while (displayProgress < toProgress)
		{
			displayProgress++;
			yield return new WaitForEndOfFrame();
		}
	}
}
