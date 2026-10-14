using System.Collections;
using UnityEngine;

public class CoinLoaded : MonoBehaviour
{
	public int count;

	public GameObject prefab;

	public GameObject Parents;

	public string Rename;

	public bool done;

	private void Start()
	{
	}

	private void Update()
	{
		StartCoroutine("One");
		base.enabled = false;
	}

	private IEnumerator One()
	{
		for (int i = 0; i < count; i++)
		{
			if (i % 10 == 0)
			{
				yield return null;
			}
			GameObject go = Object.Instantiate(prefab);
			if (Rename != string.Empty)
			{
				go.name = Rename;
			}
			go.transform.parent = Parents.transform;
			go.SetActive(false);
		}
		done = true;
		base.enabled = false;
	}
}
