using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Converts a Quaternion to a string. Format is flexible, [x] [y] [z] and [w] are placeholders")]
[ActionCategory(ActionCategory.Convert)]
public class ConvertQuaternionToString : FsmStateAction
{
	[RequiredField]
	[Tooltip("The value itself")]
	public FsmQuaternion quaternion;

	[Tooltip("The string pattern to express the Quaternion value.")]
	public FsmString format;

	[Tooltip("The c# ToString() format to express each vector float components as strings. Useful if you want to optimized the generated string.")]
	public FsmString floatConversion;

	[RequiredField]
	[UIHint(UIHint.Variable)]
	[Tooltip("The result")]
	public FsmString storeResult;

	[Tooltip("Repeats every frame")]
	public bool everyFrame;

	public override void Reset()
	{
		quaternion = null;
		format = "Quaternion([x],[y],[z],[w])";
		floatConversion = "G";
		storeResult = null;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		DoToString();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoToString();
	}

	private void DoToString()
	{
		string text = ((!string.IsNullOrEmpty(floatConversion.Value)) ? floatConversion.Value : "G");
		string value = format.Value;
		Quaternion value2 = quaternion.Value;
		value = value.Replace("[x]", value2.x.ToString(text));
		value = value.Replace("[y]", value2.y.ToString(text));
		value = value.Replace("[z]", value2.z.ToString(text));
		value = value.Replace("[w]", value2.w.ToString(text));
		storeResult.Value = value;
	}
}
