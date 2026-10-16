using System;
using UnityEngine;

namespace Parabox.InteractivePrimitives
{
	public class InteractiveCylinder : InteractivePrimitive
	{
		public int m_subdivisions = 12;

		public override bool hasSubdivisions
		{
			get
			{
				return true;
			}
		}

		public override void SetSubdivisions(int subdiv)
		{
			m_subdivisions = Mathf.Clamp(subdiv, 3, 64);
		}

		public override int GetSubdivisions()
		{
			return m_subdivisions;
		}

		public override void ToMesh()
		{
			Mesh mesh = new Mesh();
			int subdivisions = m_subdivisions;
			mesh.name = "Cylinder";
			float num = 0.5f;
			Vector3[] array = new Vector3[subdivisions];
			for (int i = 0; i < subdivisions; i++)
			{
				float f = 360f / (float)subdivisions * (float)i * ((float)Math.PI / 180f);
				float x = Mathf.Cos(f) * num;
				float z = Mathf.Sin(f) * num;
				array[i] = new Vector3(x, 0f, z);
			}
			subdivisions++;
			int num2 = subdivisions * 2 + 2 + (subdivisions - 1) * 2;
			Vector3[] array2 = new Vector3[num2];
			int num3 = 0;
			for (int j = 0; j < subdivisions; j++)
			{
				array2[num3++] = array[j % (subdivisions - 1)] - Vector3.up * 0.5f;
				array2[num3++] = array[j % (subdivisions - 1)] + Vector3.up * 0.5f;
			}
			for (int k = 0; k < subdivisions - 1; k++)
			{
				array2[num3 + subdivisions - 1] = array[k] - Vector3.up * 0.5f;
				array2[num3++] = array[k] + Vector3.up * 0.5f;
			}
			array2[num2 - 2] = -Vector3.up * 0.5f;
			array2[num2 - 1] = Vector3.up * 0.5f;
			int[] array3 = new int[subdivisions * 6 + subdivisions * 2 * 3];
			num3 = 0;
			int num4 = subdivisions * 2 - 1;
			for (int l = 0; l < num4; l += 2)
			{
				array3[num3] = l;
				array3[num3 + 1] = l + 1;
				array3[num3 + 2] = l + 2;
				array3[num3 + 3] = l + 1;
				array3[num3 + 4] = l + 3;
				array3[num3 + 5] = l + 2;
				num3 += 6;
			}
			num3 = subdivisions * 6;
			num4 = subdivisions * 2;
			subdivisions--;
			for (int m = 0; m < subdivisions; m++)
			{
				array3[num3] = m + num4;
				array3[num3 + 1] = num2 - 1;
				array3[num3 + 2] = ((m < subdivisions - 1) ? (m + num4 + 1) : num4);
				array3[num3 + subdivisions * 3] = ((m < subdivisions - 1) ? (m + num4 + subdivisions + 1) : (num4 + subdivisions));
				array3[num3 + subdivisions * 3 + 1] = num2 - 2;
				array3[num3 + subdivisions * 3 + 2] = m + num4 + subdivisions;
				num3 += 3;
			}
			LocalScale.x = Mathf.Abs(LocalScale.x);
			LocalScale.y = Mathf.Abs(LocalScale.y);
			LocalScale.z = Mathf.Abs(LocalScale.z);
			for (int n = 0; n < array2.Length; n++)
			{
				array2[n] = Vector3.Scale(array2[n], LocalScale);
			}
			mesh.vertices = array2;
			mesh.triangles = array3;
			mesh.uv = new Vector2[mesh.vertices.Length];
			mesh.RecalculateNormals();
			;
			GetComponent<MeshFilter>().sharedMesh = mesh;
			if (GetComponent<MeshRenderer>().sharedMaterial == null)
			{
				GetComponent<MeshRenderer>().sharedMaterial = base.DefaultDiffuse;
			}
		}

		public override void OnFinishDragSizing()
		{
			FreezeTransform();
			Mesh sharedMesh = GetComponent<MeshFilter>().sharedMesh;
			Vector3[] vertices = sharedMesh.vertices;
			int num = vertices.Length;
			Vector2[] array = new Vector2[num];
			float y = Vector3.Distance(vertices[0], vertices[1]);
			float num2 = 0f;
			int num3 = 0;
			for (int i = 0; i < m_subdivisions + 1; i++)
			{
				array[num3] = new Vector2(num2, 0f);
				array[num3 + 1] = new Vector2(num2, y);
				num2 += Vector3.Distance(vertices[num3], vertices[num3 + 2]);
				num3 += 2;
			}
			for (int j = (m_subdivisions + 1) * 2; j < num; j++)
			{
				array[j] = new Vector2(vertices[j].x, vertices[j].z);
			}
			Vector3[] normals = sharedMesh.normals;
			Vector3 vector = (normals[0] + normals[m_subdivisions * 2]) / 2f;
			normals[0] = vector;
			normals[1] = vector;
			normals[m_subdivisions * 2] = vector;
			normals[m_subdivisions * 2 + 1] = vector;
			sharedMesh.normals = normals;
			sharedMesh.uv = array;
		}
	}
}
