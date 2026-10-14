using System;
using UnityEngine;

namespace Parabox.InteractivePrimitives;

public class InteractiveCone : InteractivePrimitive
{
	private int m_subdivisions = 24;

	public override bool hasSubdivisions => true;

	public override void SetSubdivisions(int subdiv)
	{
		m_subdivisions = subdiv;
	}

	public override int GetSubdivisions()
	{
		return m_subdivisions;
	}

	public override void ToMesh()
	{
		int subdivisions = m_subdivisions;
		Mesh mesh = new Mesh();
		mesh.name = "Cone";
		float num = 0.5f;
		subdivisions++;
		Vector3[] array = new Vector3[subdivisions];
		for (int i = 0; i < subdivisions - 1; i++)
		{
			float f = 360f / (float)(subdivisions - 1) * (float)i * ((float)Math.PI / 180f);
			float x = Mathf.Cos(f) * num;
			float z = Mathf.Sin(f) * num;
			ref Vector3 reference = ref array[i];
			reference = new Vector3(x, -0.5f, z);
		}
		ref Vector3 reference2 = ref array[subdivisions - 1];
		reference2 = array[0];
		int num2 = subdivisions * 2 + 2;
		Vector3[] array2 = new Vector3[num2];
		for (int j = 0; j < subdivisions; j++)
		{
			ref Vector3 reference3 = ref array2[j];
			reference3 = array[j];
			ref Vector3 reference4 = ref array2[j + subdivisions];
			reference4 = array[j];
		}
		ref Vector3 reference5 = ref array2[num2 - 2];
		reference5 = Vector3.up * -0.5f;
		ref Vector3 reference6 = ref array2[num2 - 1];
		reference6 = Vector3.up * 0.5f;
		int[] array3 = new int[subdivisions * 3 * 2];
		int num3 = 0;
		int num4 = subdivisions * 3;
		for (int k = 0; k < subdivisions - 1; k++)
		{
			array3[num3] = k;
			array3[num3 + 1] = num2 - 1;
			array3[num3 + 2] = k + 1;
			array3[num3 + num4] = k + subdivisions + 1;
			array3[num3 + num4 + 1] = num2 - 2;
			array3[num3 + num4 + 2] = k + subdivisions;
			num3 += 3;
		}
		LocalScale.x = Mathf.Abs(LocalScale.x);
		LocalScale.y = Mathf.Abs(LocalScale.y);
		LocalScale.z = Mathf.Abs(LocalScale.z);
		for (int l = 0; l < array2.Length; l++)
		{
			ref Vector3 reference7 = ref array2[l];
			reference7 = Vector3.Scale(array2[l], LocalScale);
		}
		mesh.vertices = array2;
		mesh.triangles = array3;
		mesh.uv = new Vector2[mesh.vertices.Length];
		GetComponent<MeshFilter>().sharedMesh = mesh;
		if (GetComponent<MeshRenderer>().sharedMaterial == null)
		{
			GetComponent<MeshRenderer>().sharedMaterial = DefaultDiffuse;
		}
		mesh.RecalculateNormals();
	}

	public override void OnFinishDragSizing()
	{
		FreezeTransform();
		Mesh sharedMesh = GetComponent<MeshFilter>().sharedMesh;
		Vector2[] uv = sharedMesh.uv;
		Vector3[] vertices = sharedMesh.vertices;
		int vertexCount = sharedMesh.vertexCount;
		ref Vector2 reference = ref uv[vertexCount - 1];
		reference = Vector2.zero;
		ref Vector2 reference2 = ref uv[vertexCount - 2];
		reference2 = Vector2.zero;
		for (int i = 0; i < sharedMesh.vertices.Length; i++)
		{
			ref Vector2 reference3 = ref uv[i];
			reference3 = new Vector2(vertices[i].x, vertices[i].z);
		}
		sharedMesh.uv = uv;
	}
}
