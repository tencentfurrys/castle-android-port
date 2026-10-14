using UnityEngine;

public class Particlerandomseed : MonoBehaviour
{
	private void Awake()
	{
		if (!(GetComponent<ParticleSystem>() == null))
		{
			GetComponent<ParticleSystem>().Stop();
			GetComponent<ParticleSystem>().randomSeed = (uint)Random.Range(0, 9999999);
			GetComponent<ParticleSystem>().Play();
		}
	}
}
