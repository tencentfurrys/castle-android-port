using System.Collections;
using UnityEngine;

public class LoadingFinish : MonoBehaviour
{
	private AsyncOperation async;

	private void Start()
	{
		StartCoroutine(loadScene());
	}

	private IEnumerator loadScene()
	{
		async.allowSceneActivation = true;
		yield return new WaitForEndOfFrame();
	}
}
