using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Material)]
[Tooltip("Get a texture from a material on a GameObject")]
public class GetMaterialTexture : ComponentAction<Renderer>
{
	[CheckForComponent(typeof(Renderer))]
	[Tooltip("The GameObject the Material is applied to.")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("The index of the Material in the Materials array.")]
	public FsmInt materialIndex;

	[Tooltip("The texture to get. See Unity Shader docs for names.")]
	[UIHint(UIHint.NamedTexture)]
	public FsmString namedTexture;

	[Title("StoreTexture")]
	[RequiredField]
	[Tooltip("Store the texture in a variable.")]
	[UIHint(UIHint.Variable)]
	public FsmTexture storedTexture;

	[Tooltip("Get the shared version of the texture.")]
	public bool getFromSharedMaterial;

	public override void Reset()
	{
		gameObject = null;
		materialIndex = 0;
		namedTexture = "_MainTex";
		storedTexture = null;
		getFromSharedMaterial = false;
	}

	public override void OnEnter()
	{
		DoGetMaterialTexture();
		Finish();
	}

	private void DoGetMaterialTexture()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			string text = namedTexture.Value;
			if (text == string.Empty)
			{
				text = "_MainTex";
			}
			if (materialIndex.Value == 0 && !getFromSharedMaterial)
			{
				storedTexture.Value = renderer.material.GetTexture(text);
			}
			else if (materialIndex.Value == 0 && getFromSharedMaterial)
			{
				storedTexture.Value = renderer.sharedMaterial.GetTexture(text);
			}
			else if (renderer.materials.Length > materialIndex.Value && !getFromSharedMaterial)
			{
				Material[] materials = renderer.materials;
				storedTexture.Value = renderer.materials[materialIndex.Value].GetTexture(text);
				renderer.materials = materials;
			}
			else if (renderer.materials.Length > materialIndex.Value && getFromSharedMaterial)
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				storedTexture.Value = renderer.sharedMaterials[materialIndex.Value].GetTexture(text);
				renderer.materials = sharedMaterials;
			}
		}
	}
}
