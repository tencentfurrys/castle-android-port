using UnityEngine;

[ExecuteInEditMode]
public class CustomImageEffect : MonoBehaviour
{
	public Material EffectMaterial;

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (EffectMaterial != null)
		{
			Graphics.Blit(src, dst, EffectMaterial);
		}
	}
}
