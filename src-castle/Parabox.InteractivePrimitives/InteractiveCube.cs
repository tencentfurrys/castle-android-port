using UnityEngine;

namespace Parabox.InteractivePrimitives;

public class InteractiveCube : InteractivePrimitive
{
	public override void ToMesh()
	{
		Mesh mesh = new Mesh();
		mesh.name = "Cube";
		mesh.vertices = new Vector3[24]
		{
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f)
		};
		mesh.triangles = new int[36]
		{
			2, 1, 0, 2, 3, 1, 6, 5, 4, 6,
			7, 5, 10, 9, 8, 10, 11, 9, 14, 13,
			12, 14, 15, 13, 16, 17, 18, 17, 19, 18,
			22, 21, 20, 22, 23, 21
		};
		mesh.uv = new Vector2[mesh.vertices.Length];
		mesh.RecalculateNormals();
		mesh.Optimize();
		GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<MeshRenderer>().sharedMaterial = DefaultDiffuse;
	}

	public override void OnFinishDragSizing()
	{
		float num = Mathf.Abs(transform.localScale.x);
		float num2 = Mathf.Abs(transform.localScale.y);
		float num3 = Mathf.Abs(transform.localScale.z);
		base.OnFinishDragSizing();
		GetComponent<MeshFilter>().sharedMesh.uv = new Vector2[24]
		{
			new Vector2(-0.5f * num, -0.5f * num2),
			new Vector2(0.5f * num, -0.5f * num2),
			new Vector2(-0.5f * num, 0.5f * num2),
			new Vector2(0.5f * num, 0.5f * num2),
			new Vector2(-0.5f * num3, -0.5f * num2),
			new Vector2(0.5f * num3, -0.5f * num2),
			new Vector2(-0.5f * num3, 0.5f * num2),
			new Vector2(0.5f * num3, 0.5f * num2),
			new Vector2(-0.5f * num, -0.5f * num2),
			new Vector2(0.5f * num, -0.5f * num2),
			new Vector2(-0.5f * num, 0.5f * num2),
			new Vector2(0.5f * num, 0.5f * num2),
			new Vector2(-0.5f * num3, -0.5f * num2),
			new Vector2(0.5f * num3, -0.5f * num2),
			new Vector2(-0.5f * num3, 0.5f * num2),
			new Vector2(0.5f * num3, 0.5f * num2),
			new Vector2(0.5f * num, -0.5f * num3),
			new Vector2(-0.5f * num, -0.5f * num3),
			new Vector2(0.5f * num, 0.5f * num3),
			new Vector2(-0.5f * num, 0.5f * num3),
			new Vector2(-0.5f * num, -0.5f * num3),
			new Vector2(0.5f * num, -0.5f * num3),
			new Vector2(-0.5f * num, 0.5f * num3),
			new Vector2(0.5f * num, 0.5f * num3)
		};
	}
}
