using UnityEngine;

namespace Parabox.InteractivePrimitives;

public class InteractivePrism : InteractivePrimitive
{
	public override void ToMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[18]
		{
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0f, 0.5f, -0.5f),
			new Vector3(0f, 0.5f, 0.5f),
			new Vector3(0f, 0.5f, -0.5f),
			new Vector3(0f, 0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(0f, 0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0f, 0.5f, 0.5f)
		};
		mesh.uv = new Vector2[18];
		mesh.triangles = new int[24]
		{
			0, 1, 2, 1, 3, 2, 6, 5, 4, 6,
			7, 5, 10, 9, 8, 10, 11, 9, 14, 13,
			12, 15, 16, 17
		};
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<MeshRenderer>().sharedMaterial = DefaultDiffuse;
	}

	public override void OnFinishDragSizing()
	{
		float x = transform.localScale.x;
		float y = transform.localScale.y;
		float z = transform.localScale.z;
		float num = 1.414f;
		base.OnFinishDragSizing();
		Vector2[] uv = new Vector2[18]
		{
			new Vector2(0.5f * x, -0.5f * z),
			new Vector2(-0.5f * x, -0.5f * z),
			new Vector2(0.5f * x, 0.5f * z),
			new Vector2(-0.5f * x, 0.5f * z),
			new Vector2(num * y, -0.5f * z),
			new Vector2(num * y, 0.5f * z),
			new Vector2(0f, -0.5f * z),
			new Vector2(0f, 0.5f * z),
			new Vector2(0f, -0.5f * z),
			new Vector2(0f, 0.5f * z),
			new Vector2((0f - num) * y, -0.5f * z),
			new Vector2((0f - num) * y, 0.5f * z),
			new Vector2(-0.5f * x, -0.5f * y),
			new Vector2(0.5f * x, -0.5f * y),
			new Vector2(0f, 0.5f * y),
			new Vector2(0.5f * x, -0.5f * y),
			new Vector2(-0.5f * x, -0.5f * y),
			new Vector2(0f, 0.5f * y)
		};
		GetComponent<MeshFilter>().sharedMesh.uv = uv;
	}
}
