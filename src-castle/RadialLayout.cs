using System;
using UnityEngine;
using UnityEngine.UI;

public class RadialLayout : LayoutGroup
{
	public float fDistance;

	[Range(0f, 360f)]
	public float MinAngle;

	[Range(0f, 360f)]
	public float MaxAngle;

	[Range(0f, 360f)]
	public float StartAngle;

	protected override void OnEnable()
	{
		base.OnEnable();
		CalculateRadial();
	}

	public override void SetLayoutHorizontal()
	{
	}

	public override void SetLayoutVertical()
	{
	}

	public override void CalculateLayoutInputVertical()
	{
		CalculateRadial();
	}

	public override void CalculateLayoutInputHorizontal()
	{
		CalculateRadial();
	}

	private void CalculateRadial()
	{
		m_Tracker.Clear();
		if (transform.childCount == 0)
		{
			return;
		}
		float num = (MaxAngle - MinAngle) / (float)(transform.childCount - 1);
		float num2 = StartAngle;
		for (int i = 0; i < transform.childCount; i++)
		{
			RectTransform rectTransform = (RectTransform)transform.GetChild(i);
			if (rectTransform != null)
			{
				m_Tracker.Add(this, rectTransform, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.Pivot);
				Vector3 vector = new Vector3(Mathf.Cos(num2 * ((float)Math.PI / 180f)), Mathf.Sin(num2 * ((float)Math.PI / 180f)), 0f);
				rectTransform.localPosition = vector * fDistance;
				Vector2 vector2 = (rectTransform.pivot = new Vector2(0.5f, 0.5f));
				vector2 = (rectTransform.anchorMax = vector2);
				rectTransform.anchorMin = vector2;
				num2 += num;
			}
		}
	}
}
