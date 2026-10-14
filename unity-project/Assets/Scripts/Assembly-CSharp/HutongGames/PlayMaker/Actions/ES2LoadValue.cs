using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Load values with Easy Save 2")]
	public class ES2LoadValue : FsmStateAction
	{
		[Tooltip("The prefix tag for the data we want to load. Not mandatory, as values below must have tags anyway")]
		public FsmString prefixTag = string.Empty;

		[RequiredField]
		[Tooltip("The name of the file our data is stored in.")]
		public FsmString saveFile = "defaultES2File.txt";

		[RequiredField]
		[Tooltip("The unique tag for referencing.")]
		[CompoundArray("Count", "Tag", "Value")]
		public FsmString[] tags;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVar[] values;

		public override void Reset()
		{
			tags = new FsmString[1];
			FsmString fsmString = new FsmString();
			fsmString.Value = "My Value";
			tags[0] = fsmString;
			values = new FsmVar[1];
		}

		public override string ErrorCheck()
		{
			for (int i = 0; i < tags.Length; i++)
			{
				FsmVar fsmVar = values[i];
				switch (fsmVar.Type)
				{
				case VariableType.Float:
				case VariableType.Int:
				case VariableType.Bool:
				case VariableType.String:
				case VariableType.Vector2:
				case VariableType.Vector3:
				case VariableType.Color:
				case VariableType.Rect:
				case VariableType.Quaternion:
					continue;
				}
				return "EasySave2 does not support " + fsmVar.Type;
			}
			return string.Empty;
		}

		public override void OnEnter()
		{
			string text = saveFile.Value + "?tag=" + prefixTag;
			for (int i = 0; i < tags.Length; i++)
			{
				if (!tags[i].IsNone || !tags[i].Value.Equals(string.Empty))
				{
					string value = tags[i].Value;
					FsmVar fsmVar = values[i];
					string text2 = text;
					if (!tags[i].IsNone)
					{
						text2 += value;
					}
					switch (fsmVar.Type)
					{
					case VariableType.Int:
						PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, fsmVar, ES2.Load<int>(text2));
						break;
					case VariableType.Float:
						PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, fsmVar, ES2.Load<float>(text2));
						break;
					case VariableType.Bool:
						PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, fsmVar, ES2.Load<bool>(text2));
						break;
					case VariableType.Color:
						PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, fsmVar, ES2.Load<Color>(text2));
						break;
					case VariableType.Quaternion:
						PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, fsmVar, ES2.Load<Quaternion>(text2));
						break;
					case VariableType.Rect:
						PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, fsmVar, ES2.Load<Rect>(text2));
						break;
					case VariableType.Vector2:
						PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, fsmVar, ES2.Load<Vector2>(text2));
						break;
					case VariableType.Vector3:
						PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, fsmVar, ES2.Load<Vector3>(text2));
						break;
					case VariableType.String:
						PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, fsmVar, ES2.Load<string>(text2));
						break;
					default:
						LogError("PlayerPrefsx does not support saving " + fsmVar.Type);
						break;
					}
				}
			}
			Finish();
		}
	}
}
