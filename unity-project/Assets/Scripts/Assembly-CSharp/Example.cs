using System.Collections;
using UnityEngine;

internal class Example : MonoBehaviour
{
	public int count;

	public GameObject prefab;

	public GameObject Parents;

	private IEnumerator Awake()
	{
		for (int i = 0; i < count; i++)
		{
			if (i % 10 == 0)
			{
				yield return null;
			}
			GameObject go = Object.Instantiate(prefab);
			go.transform.parent = Parents.transform;
			go.SetActive(false);
		}
	}
}
