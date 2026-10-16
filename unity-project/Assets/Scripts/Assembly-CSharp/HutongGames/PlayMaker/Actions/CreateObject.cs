using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Creates a Game Object, usually from a Prefab.")]
	public class CreateObject : FsmStateAction
	{
		[RequiredField]
		[Tooltip("GameObject to create. Usually a Prefab.")]
		public FsmGameObject gameObject;

		[Tooltip("Optional Spawn Point.")]
		public FsmGameObject spawnPoint;

		[Tooltip("Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
		public FsmVector3 position;

		[Tooltip("Rotation. NOTE: Overrides the rotation of the Spawn Point.")]
		public FsmVector3 rotation;

		[Tooltip("Optionally store the created object.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeObject;

		[Tooltip("Use Network.Instantiate to create a Game Object on all clients in a networked game.")]
		public FsmBool networkInstantiate;

		[Tooltip("Usually 0. The group number allows you to group together network messages which allows you to filter them if so desired.")]
		public FsmInt networkGroup;

		private static int callCounter;

		public override void Reset()
		{
			gameObject = null;
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
			networkInstantiate = false;
			networkGroup = 0;
		}

		private static bool HasRectTransformInTree(GameObject go)
		{
			if (go.transform is RectTransform)
			{
				return true;
			}
			return go.GetComponentInChildren<RectTransform>(true) != null;
		}

		public override void OnEnter()
		{
			GameObject value = gameObject.Value;
			if (value != null)
			{
				Vector3 vector = Vector3.zero;
				Vector3 euler = Vector3.up;
				if (spawnPoint.Value != null)
				{
					vector = spawnPoint.Value.transform.position;
					if (!position.IsNone)
					{
						vector += position.Value;
					}
					euler = (rotation.IsNone ? spawnPoint.Value.transform.eulerAngles : rotation.Value);
				}
				else
				{
					if (!position.IsNone)
					{
						vector = position.Value;
					}
					if (!rotation.IsNone)
					{
						euler = rotation.Value;
					}
				}
				callCounter++;
				if (networkInstantiate.Value)
				{
					GameObject value2n = (GameObject)Network.Instantiate(value, vector, Quaternion.Euler(euler), networkGroup.Value);
					storeObject.Value = value2n;
				}
				else if (HasRectTransformInTree(value))
				{
					// 5.6 fix: Object.Instantiate(prefab, pos, rot) on a prefab that contains
					// RectTransforms crashes 5.6.7f1 in UI::RectTransform::CalculateLocalPosition3
					// (native access violation during the transform-change notification of the
					// not-yet-registered clone). Workaround: instantiate without position/rotation,
					// then apply them. The clone is parentless here, so local == world.
					Debug.Log(string.Concat("[CreateObject] #", callCounter, " ui-bare ", value.name, " pos ", vector.ToString()));
					GameObject value2 = (GameObject)Object.Instantiate((Object)value);
					value2.transform.localPosition = vector;
					value2.transform.localRotation = Quaternion.Euler(euler);
					Debug.Log(string.Concat("[CreateObject] #", callCounter, " ui-done ", value.name));
					storeObject.Value = value2;
				}
				else
				{
					GameObject value2p = (GameObject)Object.Instantiate((Object)value, vector, Quaternion.Euler(euler));
					storeObject.Value = value2p;
				}
			}
			Finish();
		}
	}
}
