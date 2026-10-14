using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Save values with Easy Save 2")]
	[ActionCategory("Easy Save 2")]
	public class ES2SaveValue : FsmStateAction
	{
		[Tooltip("The prefix tag for the data we want to save. Not mandatory, as values below must have tags anyway")]
		public FsmString prefixTag = string.Empty;

		[RequiredField]
		[Tooltip("The name of the file our data is stored in.")]
		public FsmString saveFile = "defaultES2File.txt";

		[RequiredField]
		[Tooltip("The unique tag for referencing.")]
		[CompoundArray("Count", "Tag", "Value")]
		public FsmString[] tags;

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
			string text = saveFile.Value + "?tag=" + prefixTag.Value;
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
						ES2.Save((!fsmVar.IsNone) ? ((int)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, fsmVar)) : 0, text2);
						break;
					case VariableType.Float:
						ES2.Save((!fsmVar.IsNone) ? ((float)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, fsmVar)) : 0f, text2);
						break;
					case VariableType.Bool:
						ES2.Save(!fsmVar.IsNone && (bool)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, fsmVar), text2);
						break;
					case VariableType.Color:
						ES2.Save((!fsmVar.IsNone) ? ((Color)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, fsmVar)) : Color.black, text2);
						break;
					case VariableType.Quaternion:
						ES2.Save((!fsmVar.IsNone) ? ((Quaternion)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, fsmVar)) : Quaternion.identity, text2);
						break;
					case VariableType.Rect:
						ES2.Save((!fsmVar.IsNone) ? ((Rect)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, fsmVar)) : new Rect(0f, 0f, 0f, 0f), text2);
						break;
					case VariableType.Vector2:
						ES2.Save((!fsmVar.IsNone) ? ((Vector2)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, fsmVar)) : Vector2.zero, text2);
						break;
					case VariableType.Vector3:
						ES2.Save((!fsmVar.IsNone) ? ((Vector3)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, fsmVar)) : Vector3.zero, text2);
						break;
					case VariableType.String:
						ES2.Save((!fsmVar.IsNone) ? ((string)PlayMakerUtils.GetValueFromFsmVar(base.Fsm, fsmVar)) : string.Empty, text2);
						break;
					default:
						LogError("EasySave2 does not support saving " + fsmVar.Type);
						break;
					}
				}
			}
			Finish();
		}
	}
}
