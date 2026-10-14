using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Count if an item is contained in a particular PlayMaker ArrayList Proxy component. Can be also be used to find duplicates in a 'dyi' manner")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class ArrayListContainsCount : ArrayListActions
	{
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[ActionSection("Setup")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		public FsmString reference;

		[ActionSection("Data")]
		[CompoundArray("Size", "Variable", "count")]
		[RequiredField]
		[Tooltip("The variable to check.")]
		public FsmVar[] variable;

		[Tooltip("Store the count value")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt[] count;

		[UIHint(UIHint.FsmInt)]
		[Tooltip("Store the total count value")]
		[ActionSection("Other info")]
		public FsmInt totalArrayCount;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		[ActionSection("Event")]
		public FsmEvent failureEvent;

		private int atIndex;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			variable = new FsmVar[1];
			count = new FsmInt[1];
			totalArrayCount = 0;
			atIndex = -1;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				doesArrayListContainsCount();
			}
			Finish();
		}

		public void doesArrayListContainsCount()
		{
			if (!isProxyValid())
			{
				return;
			}
			int num = proxy.arrayList.Count;
			totalArrayCount.Value = num;
			FsmVar fsmVar = variable[0];
			for (int i = 0; i < variable.Length; i++)
			{
				count[i].Value = 0;
				atIndex = -1;
				for (int j = 0; j < num; j++)
				{
					atIndex++;
					bool flag = false;
					object obj = null;
					try
					{
						obj = proxy.arrayList[atIndex];
					}
					catch (Exception ex)
					{
						Debug.Log(ex.Message);
						base.Fsm.Event(failureEvent);
						return;
					}
					switch (fsmVar.Type)
					{
					case VariableType.Int:
					{
						FsmInt fsmInt = Convert.ToInt32(obj);
						int num3 = Convert.ToInt32(PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]));
						flag = fsmInt.Value == num3;
						break;
					}
					case VariableType.Float:
					{
						FsmFloat fsmFloat = (float)obj;
						float num2 = (float)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = fsmFloat.Value == num2;
						break;
					}
					case VariableType.Bool:
					{
						FsmBool fsmBool = (bool)obj;
						bool flag2 = (bool)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = fsmBool.Value == flag2;
						break;
					}
					case VariableType.Color:
					{
						FsmColor fsmColor = (Color)obj;
						Color color = (Color)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = fsmColor.Value == color;
						break;
					}
					case VariableType.Quaternion:
					{
						FsmQuaternion fsmQuaternion = (Quaternion)obj;
						Quaternion quaternion = (Quaternion)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = fsmQuaternion.Value == quaternion;
						break;
					}
					case VariableType.Rect:
					{
						FsmRect fsmRect = (Rect)obj;
						Rect rect = (Rect)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = fsmRect.Value == rect;
						break;
					}
					case VariableType.Vector2:
					{
						FsmVector2 fsmVector2 = (Vector2)obj;
						Vector2 vector2 = (Vector2)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = fsmVector2.Value == vector2;
						break;
					}
					case VariableType.Vector3:
					{
						FsmVector3 fsmVector = (Vector3)obj;
						Vector3 vector = (Vector3)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = fsmVector.Value == vector;
						break;
					}
					case VariableType.String:
					{
						FsmString fsmString = (string)obj;
						string text = (string)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = fsmString.Value == text;
						break;
					}
					case VariableType.GameObject:
					{
						FsmGameObject fsmGameObject = (GameObject)obj;
						GameObject gameObject = (GameObject)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = fsmGameObject.Value == gameObject;
						break;
					}
					case VariableType.Material:
					{
						Material material = (Material)obj;
						Material material2 = (Material)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = material == material2;
						break;
					}
					case VariableType.Texture:
					{
						Texture texture = (Texture)obj;
						Texture texture2 = (Texture)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = texture == texture2;
						break;
					}
					case VariableType.Unknown:
						Debug.Log("ERROR");
						break;
					case VariableType.Object:
					{
						object obj2 = obj;
						object valueFromFsmVar = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable[i]);
						flag = obj2 == valueFromFsmVar;
						break;
					}
					default:
						Debug.Log("ERROR");
						break;
					}
					if (flag)
					{
						count[i].Value++;
					}
				}
			}
			Finish();
		}
	}
}
