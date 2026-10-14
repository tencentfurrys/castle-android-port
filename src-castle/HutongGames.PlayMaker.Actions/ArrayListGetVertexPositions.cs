using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Store mesh vertex positions into an arrayList")]
[ActionCategory("ArrayMaker/ArrayList")]
public class ArrayListGetVertexPositions : ArrayListActions
{
	[RequiredField]
	[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
	[CheckForComponent(typeof(PlayMakerArrayListProxy))]
	[ActionSection("Set up")]
	public FsmOwnerDefault gameObject;

	[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
	public FsmString reference;

	[Tooltip("the GameObject to get the mesh from")]
	[CheckForComponent(typeof(MeshFilter))]
	[ActionSection("Source")]
	public FsmGameObject mesh;

	public override void Reset()
	{
		gameObject = null;
		reference = null;
		mesh = null;
	}

	public override void OnEnter()
	{
		if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
		{
			getVertexPositions();
		}
		Finish();
	}

	public void getVertexPositions()
	{
		if (!isProxyValid())
		{
			return;
		}
		proxy.arrayList.Clear();
		GameObject value = mesh.Value;
		if (!(value == null))
		{
			MeshFilter component = value.GetComponent<MeshFilter>();
			if (!(component == null))
			{
				proxy.arrayList.InsertRange(0, component.mesh.vertices);
			}
		}
	}
}
