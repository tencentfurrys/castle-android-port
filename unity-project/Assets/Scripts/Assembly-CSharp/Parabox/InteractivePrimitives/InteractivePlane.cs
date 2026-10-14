using UnityEngine;

namespace Parabox.InteractivePrimitives
{
	public class InteractivePlane : InteractivePrimitive
	{
		public override bool baseOnly
		{
			get
			{
				return true;
			}
		}

		public override void ToMesh()
		{
			Mesh mesh = new Mesh();
			mesh.vertices = new Vector3[4]
			{
				new Vector3(-0.5f, 0f, 0.5f),
				new Vector3(0.5f, 0f, 0.5f),
				new Vector3(-0.5f, 0f, -0.5f),
				new Vector3(0.5f, 0f, -0.5f)
			};
			mesh.uv = new Vector2[4]
			{
				Vector2.zero,
				Vector2.right,
				Vector2.up,
				Vector2.one
			};
			mesh.normals = new Vector3[4]
			{
				Vector3.up,
				Vector3.up,
				Vector3.up,
				Vector3.up
			};
			mesh.triangles = new int[6] { 0, 1, 2, 1, 3, 2 };
			GetComponent<MeshFilter>().sharedMesh = mesh;
			GetComponent<MeshRenderer>().sharedMaterial = base.DefaultDiffuse;
		}

		public override void SetMeshDimensions(Vector3 baseStart, Vector3 baseEnd, float height)
		{
			base.transform.position = baseStart + (baseEnd - baseStart) / 2f;
			base.transform.localScale = new Vector3(baseEnd.x - baseStart.x, 1f, baseEnd.z - baseStart.z);
		}

		public override void OnFinishDragSizing()
		{
			base.OnFinishDragSizing();
			Vector3[] vertices = GetComponent<MeshFilter>().sharedMesh.vertices;
			GetComponent<MeshFilter>().sharedMesh.uv = new Vector2[4]
			{
				new Vector2(vertices[0].x, vertices[0].z),
				new Vector2(vertices[1].x, vertices[1].z),
				new Vector2(vertices[2].x, vertices[2].z),
				new Vector2(vertices[3].x, vertices[3].z)
			};
		}
	}
}
