using UnityEngine;

public class OnTriggerSetActive : MonoBehaviour
{
	public float _DelayTime;

	public bool _SetActive;

	[Header(" 是否針對設定目標開啟，False的話就是針對自己")]
	public bool _IsTargetObj;

	public GameObject _TargetObj;

	public string _TriggerTargetTag;

	public bool IsDestory;

	private SphereCollider Collider;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == _TriggerTargetTag)
		{
			Invoke("Active", _DelayTime);
		}
	}

	private void Active()
	{
		if (_IsTargetObj)
		{
			Collider = GetComponent<SphereCollider>();
			Collider.enabled = false;
		}
		else
		{
			Collider = GetComponent<SphereCollider>();
			Collider.enabled = false;
		}
	}
}
