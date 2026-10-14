using UnityEngine;

namespace Parabox.InteractivePrimitives;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public abstract class InteractivePrimitive : MonoBehaviour
{
	public Vector3 LocalScale = Vector3.one;

	public Vector3 baseStart;

	public Vector3 baseEnd;

	public float height;

	public bool sizing = true;

	public Material DefaultDiffuse
	{
		get
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Material sharedMaterial = gameObject.GetComponent<Renderer>().sharedMaterial;
			Object.DestroyImmediate(gameObject);
			return sharedMaterial;
		}
	}

	public virtual bool baseOnly => false;

	public virtual bool hasSubdivisions => false;

	public virtual int MinSubdivisions => 3;

	public virtual int MaxSubdivisions => 64;

	public virtual void Init()
	{
		ToMesh();
	}

	public abstract void ToMesh();

	public virtual void SetMeshDimensions(Vector3 baseStart, Vector3 baseEnd, float height)
	{
		this.baseStart = baseStart;
		this.baseEnd = baseEnd;
		this.height = height;
		transform.position = baseStart + (baseEnd - baseStart) / 2f + Vector3.up * (height / 2f);
		transform.localScale = new Vector3(baseEnd.x - baseStart.x, height, baseEnd.z - baseStart.z);
	}

	public virtual void OnFinishDragSizing()
	{
		FreezeTransform();
	}

	public virtual void SetSubdivisions(int subdiv)
	{
	}

	public virtual int GetSubdivisions()
	{
		return 0;
	}

	internal void FreezeTransform()
	{
		Mesh sharedMesh = transform.GetComponent<MeshFilter>().sharedMesh;
		Vector3 localScale = transform.localScale;
		localScale.x = Mathf.Abs(localScale.x);
		localScale.y = Mathf.Abs(localScale.y);
		localScale.z = Mathf.Abs(localScale.z);
		Vector3[] vertices = sharedMesh.vertices;
		for (int i = 0; i < vertices.Length; i++)
		{
			ref Vector3 reference = ref vertices[i];
			reference = Vector3.Scale(vertices[i], localScale);
		}
		sharedMesh.vertices = vertices;
		LocalScale = Vector3.one;
		sharedMesh.RecalculateNormals();
		sharedMesh.Optimize();
		transform.localScale = Vector3.one;
	}

	private void OnDestroy()
	{
		Object.DestroyImmediate(GetComponent<MeshFilter>().sharedMesh);
	}
}
