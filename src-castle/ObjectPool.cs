using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	public GameObject prefab;

	public GameObject Parents;

	public int initailSize;

	private void Awake()
	{
		for (int i = 0; i < initailSize; i++)
		{
			GameObject gameObject = Object.Instantiate(prefab);
			gameObject.transform.parent = Parents.transform;
			gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
	}
}
