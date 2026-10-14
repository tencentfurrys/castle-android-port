using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.GameObject)]
[Tooltip("Creates a Game Object, usually from a Prefab. Let you define parent and name")]
public class CreateObjectAdvanced : FsmStateAction
{
	[Tooltip("GameObject to create. Usually a Prefab.")]
	[RequiredField]
	public FsmGameObject gameObject;

	[Tooltip("GameObject  parent. Leave to null or none for no parenting")]
	public FsmOwnerDefault parent;

	[Tooltip("If True, parenting will keep the objecr created in place")]
	public FsmBool worldPositionStays;

	[Tooltip("GameObject name. Leave to null or none for default")]
	public FsmString name;

	[Tooltip("Activate the created instance or not")]
	public FsmBool setActive;

	[Tooltip("Optional Spawn Point.")]
	public FsmGameObject spawnPoint;

	[Tooltip("Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
	public FsmVector3 position;

	[Tooltip("Rotation. NOTE: Overrides the rotation of the Spawn Point if set.")]
	public FsmVector3 rotation;

	[UIHint(UIHint.Variable)]
	[Tooltip("Optionally store the created object.")]
	public FsmGameObject storeObject;

	private GameObject newObject;

	public override void Reset()
	{
		gameObject = null;
		parent = new FsmOwnerDefault();
		parent.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
		parent.GameObject = new FsmGameObject
		{
			UseVariable = true
		};
		worldPositionStays = true;
		name = new FsmString
		{
			UseVariable = true
		};
		setActive = true;
		spawnPoint = null;
		position = new FsmVector3
		{
			UseVariable = true
		};
		rotation = new FsmVector3
		{
			UseVariable = true
		};
		storeObject = null;
	}

	public override void OnEnter()
	{
		GameObject value = gameObject.Value;
		if (value != null)
		{
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.up;
			if (spawnPoint.Value != null)
			{
				vector = spawnPoint.Value.transform.position;
				if (!position.IsNone)
				{
					vector += position.Value;
				}
				vector2 = (rotation.IsNone ? spawnPoint.Value.transform.eulerAngles : rotation.Value);
			}
			else
			{
				if (!position.IsNone)
				{
					vector = position.Value;
				}
				if (!rotation.IsNone)
				{
					vector2 = rotation.Value;
				}
			}
			newObject = (GameObject)Object.Instantiate(value, vector, Quaternion.Euler(vector2));
			storeObject.Value = newObject;
			newObject.SetActive(setActive.Value);
			newObject.transform.position = vector;
			newObject.transform.eulerAngles = vector2;
			GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(parent);
			if (ownerDefaultTarget != null)
			{
				newObject.transform.SetParent(ownerDefaultTarget.transform, worldPositionStays.Value);
			}
			if (!string.IsNullOrEmpty(name.Value))
			{
				newObject.name = name.Value;
			}
		}
		Finish();
	}
}
