using UnityEngine;
using UnityEngine.UI;

public class TextPosition : MonoBehaviour
{
	public Text textComp;

	public int charIndex;

	public Canvas canvas;

	public Vector3 WorldPosV3;

	private void Update()
	{
		string text = textComp.text;
		if (charIndex > text.Length)
		{
			base.enabled = false;
		}
		TextGenerator textGenerator = new TextGenerator(text.Length);
		Vector2 size = textComp.gameObject.GetComponent<RectTransform>().rect.size;
		textGenerator.Populate(text, textComp.GetGenerationSettings(size));
		int num = text.Substring(0, charIndex).Split(' ').Length - 1;
		int num2 = text.Substring(0, charIndex).Split(' ').Length - 1;
		int num3 = charIndex * 4 + num * 4 - 4;
		if (num3 < textGenerator.vertexCount)
		{
			Vector3 vector = (textGenerator.verts[num3].position + textGenerator.verts[num3 + 1].position + textGenerator.verts[num3 + 2].position + textGenerator.verts[num3 + 3].position) / 4f;
			MonoBehaviour.print(vector);
			PrintWorldPos(vector);
		}
		else
		{
			Debug.LogError("Out of text bound");
		}
		base.enabled = false;
	}

	private void PrintWorldPos(Vector3 testPoint)
	{
		Vector3 worldPosV = textComp.transform.TransformPoint(testPoint);
		WorldPosV3 = worldPosV;
	}
}
