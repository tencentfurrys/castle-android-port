using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Material)]
[Tooltip("Get a material at index on a gameObject and store it in a variable")]
public class GetMaterial : ComponentAction<Renderer>
{
	[RequiredField]
	[Tooltip("The GameObject the Material is applied to.")]
	[CheckForComponent(typeof(Renderer))]
	public FsmOwnerDefault gameObject;

	[Tooltip("The index of the Material in the Materials array.")]
	public FsmInt materialIndex;

	[UIHint(UIHint.Variable)]
	[Tooltip("Store the material in a variable.")]
	[RequiredField]
	public FsmMaterial material;

	[Tooltip("Get the shared material of this object. NOTE: Modifying the shared material will change the appearance of all objects using this material, and change material settings that are stored in the project too.")]
	public bool getSharedMaterial;

	public override void Reset()
	{
		gameObject = null;
		material = null;
		materialIndex = 0;
		getSharedMaterial = false;
	}

	public override void OnEnter()
	{
		DoGetMaterial();
		Finish();
	}

	private void DoGetMaterial()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			if (materialIndex.Value == 0 && !getSharedMaterial)
			{
				material.Value = renderer.material;
			}
			else if (materialIndex.Value == 0 && getSharedMaterial)
			{
				material.Value = renderer.sharedMaterial;
			}
			else if (renderer.materials.Length > materialIndex.Value && !getSharedMaterial)
			{
				Material[] materials = renderer.materials;
				material.Value = materials[materialIndex.Value];
				renderer.materials = materials;
			}
			else if (renderer.materials.Length > materialIndex.Value && getSharedMaterial)
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				material.Value = sharedMaterials[materialIndex.Value];
				renderer.sharedMaterials = sharedMaterials;
			}
		}
	}
}
