using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Set any renderer Sorting Layer (Sprite, Particle, Mesh, etc).")]
[ActionCategory(ActionCategory.Renderer)]
public class SetSortingRendererLayer : FsmStateAction
{
	[ActionSection("Setup")]
	[Tooltip("The GameObject that owns the Renderer.")]
	public FsmOwnerDefault gameObject;

	[Title("or select Renderer Object")]
	[Tooltip("If more than one renderer on gameobject then select Renderer Object.")]
	public Renderer gameObjectRenderer;

	[Tooltip("Select Sorting layer name.")]
	[Title("Renderer Layer Name")]
	[UIHint(UIHint.FsmString)]
	[ActionSection("Sorting Layer Setup")]
	public FsmString layerString;

	[Tooltip("Edit Sorting Order in Layer")]
	[Title("Edit Sorting Order")]
	[UIHint(UIHint.FsmInt)]
	public FsmInt layerOrder;

	public override void Reset()
	{
		gameObject = null;
		layerOrder = 0;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget == null)
		{
			Debug.LogWarning("missing gameObject: " + ownerDefaultTarget.name);
			return;
		}
		if (gameObjectRenderer != null)
		{
			RenderSetupObj();
		}
		if (gameObjectRenderer == null)
		{
			RenderSetup();
		}
		Finish();
	}

	private void RenderSetup()
	{
		Renderer component = Fsm.GetOwnerDefaultTarget(gameObject).gameObject.GetComponent<Renderer>();
		if (component == null)
		{
			Debug.LogWarning("missing renderer: " + Fsm.GetOwnerDefaultTarget(gameObject).name);
			Finish();
			return;
		}
		if (layerString.IsNone || string.IsNullOrEmpty(layerString.Value) || layerString.Value == null)
		{
			component.sortingOrder = layerOrder.Value;
		}
		else
		{
			component.sortingLayerName = layerString.Value;
		}
		component.sortingOrder = layerOrder.Value;
		Finish();
	}

	private void RenderSetupObj()
	{
		if (gameObjectRenderer == null)
		{
			Debug.LogWarning("missing renderer: " + gameObjectRenderer.name);
			Finish();
			return;
		}
		if (layerString.IsNone || string.IsNullOrEmpty(layerString.Value) || layerString.Value == null)
		{
			gameObjectRenderer.sortingOrder = layerOrder.Value;
		}
		else
		{
			gameObjectRenderer.sortingLayerName = layerString.Value;
		}
		gameObjectRenderer.sortingOrder = layerOrder.Value;
		Finish();
	}
}
