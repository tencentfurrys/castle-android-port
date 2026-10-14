using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.GameObject)]
[Tooltip("Finds the farthest object to the specified Game Object.\nOptionally filter by Tag, layer and Visibility.")]
public class FindFarthest : FsmStateAction
{
	[RequiredField]
	[Tooltip("The GameObject to measure from.")]
	public FsmOwnerDefault gameObject;

	[UIHint(UIHint.Tag)]
	[Tooltip("Only consider objects with this Tag. NOTE: It's generally a lot quicker to find objects with a Tag!")]
	[RequiredField]
	public FsmString withTag;

	[Tooltip("If checked, ignores the object that owns this FSM.")]
	public FsmBool ignoreOwner;

	[Tooltip("Only consider objects visible to the camera.")]
	public FsmBool mustBeVisible;

	[Tooltip("Only consider objects from these layers.")]
	[UIHint(UIHint.Layer)]
	public FsmInt[] layerMask;

	[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
	public FsmBool invertMask;

	[Tooltip("Store the closest object.")]
	[UIHint(UIHint.Variable)]
	public FsmGameObject storeObject;

	[Tooltip("Store the distance to the farthest object.")]
	[UIHint(UIHint.Variable)]
	public FsmFloat storeDistance;

	[Tooltip("Repeat every frame")]
	public bool everyFrame;

	private int _layerMask;

	public override void Reset()
	{
		gameObject = null;
		withTag = "Untagged";
		ignoreOwner = true;
		mustBeVisible = false;
		layerMask = new FsmInt[0];
		invertMask = false;
		storeObject = null;
		storeDistance = null;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		_layerMask = ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value);
		DoFindFarthest();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoFindFarthest();
	}

	private void DoFindFarthest()
	{
		GameObject gameObject = ((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : Owner);
		GameObject[] array = ((!string.IsNullOrEmpty(withTag.Value) && !(withTag.Value == "Untagged")) ? GameObject.FindGameObjectsWithTag(withTag.Value) : ((GameObject[])Object.FindObjectsOfType(typeof(GameObject))));
		GameObject value = null;
		float num = 0f;
		GameObject[] array2 = array;
		foreach (GameObject gameObject2 in array2)
		{
			if ((!ignoreOwner.Value || !(gameObject2 == Owner)) && (!mustBeVisible.Value || ActionHelpers.IsVisible(gameObject2)) && (layerMask.Length <= 0 || (_layerMask & (1 << gameObject2.layer)) != 0))
			{
				float sqrMagnitude = (gameObject.transform.position - gameObject2.transform.position).sqrMagnitude;
				if (sqrMagnitude > num)
				{
					num = sqrMagnitude;
					value = gameObject2;
				}
			}
		}
		storeObject.Value = value;
		if (!storeDistance.IsNone)
		{
			storeDistance.Value = Mathf.Sqrt(num);
		}
	}
}
