using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Material)]
[Tooltip("Sets the material on a game object.")]
public class SetMaterial : ComponentAction<Renderer>
{
	[CheckForComponent(typeof(Renderer))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	public FsmInt materialIndex;

	[RequiredField]
	public FsmMaterial material;

	public override void Reset()
	{
		gameObject = null;
		material = null;
		materialIndex = 0;
	}

	public override void OnEnter()
	{
		DoSetMaterial();
		Finish();
	}

	private void DoSetMaterial()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			if (materialIndex.Value == 0)
			{
				renderer.material = material.Value;
			}
			else if (renderer.materials.Length > materialIndex.Value)
			{
				Material[] materials = renderer.materials;
				materials[materialIndex.Value] = material.Value;
				renderer.materials = materials;
			}
		}
	}
}
