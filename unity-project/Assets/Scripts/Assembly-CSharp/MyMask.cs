using UnityEngine;
using UnityEngine.UI;

public class MyMask : Mask
{
	protected override void Start()
	{
		base.Start();
		int width = Screen.width;
		int height = Screen.height;
		int num = 960;
		int num2 = 640;
		float num3 = (float)num / (float)num2;
		float num4 = (float)width / (float)height;
		float num5 = 1f;
		if (num3 > num4)
		{
			num5 = num3 / num4;
		}
		Canvas component = GameObject.Find("Canvas").GetComponent<Canvas>();
		Vector2 localPoint;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(component.transform as RectTransform, base.transform.position, component.GetComponent<Camera>(), out localPoint))
		{
			ParticleSystem[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleSystem>();
			RectTransform rectTransform = base.transform as RectTransform;
			float num6 = rectTransform.rect.x + localPoint.x;
			float num7 = rectTransform.rect.y + localPoint.y;
			float num8 = num6 + rectTransform.rect.width;
			float num9 = num7 + rectTransform.rect.height;
			ParticleSystem[] array = componentsInChildren;
			foreach (ParticleSystem particleSystem in array)
			{
				particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MinX", num6 / 100f / num5);
				particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MinY", num7 / 100f / num5);
				particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MaxX", num8 / 100f / num5);
				particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MaxY", num9 / 100f / num5);
			}
		}
	}
}
