namespace HutongGames.PlayMaker.Actions;

[Tooltip("Set a color based on HSV values")]
[ActionCategory(ActionCategory.Color)]
[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1147")]
public class SetColorHSV : FsmStateAction
{
	[Tooltip("The hsv color source")]
	public FsmVector3 HSV;

	[Tooltip("The Hue value source, overrides HSV Hue if set")]
	public FsmFloat hue;

	[Tooltip("The Saturation value source, overrides HSV Saturation if set")]
	public FsmFloat saturation;

	[Tooltip("The value value source, overrides HSV Value if set")]
	public FsmFloat value;

	[Tooltip("The alpha value source")]
	public FsmFloat alpha;

	[Tooltip("The resulting color")]
	[UIHint(UIHint.Variable)]
	public FsmColor colorResult;

	[Tooltip("Repeat every frame")]
	public bool everyframe;

	public override void Reset()
	{
		HSV = new FsmVector3
		{
			UseVariable = true
		};
		hue = new FsmFloat
		{
			UseVariable = true
		};
		saturation = new FsmFloat
		{
			UseVariable = true
		};
		value = new FsmFloat
		{
			UseVariable = true
		};
		alpha = new FsmFloat
		{
			UseVariable = true
		};
		colorResult = null;
		everyframe = false;
	}

	public override void OnEnter()
	{
		SetHSVColor();
		if (!everyframe)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		SetHSVColor();
	}

	private void SetHSVColor()
	{
		ColorHSV colorHSV = new ColorHSV(HSV.Value.x, HSV.Value.y, HSV.Value.z, alpha.Value);
		if (!hue.IsNone)
		{
			colorHSV.h = hue.Value;
		}
		if (!saturation.IsNone)
		{
			colorHSV.s = saturation.Value;
		}
		if (!value.IsNone)
		{
			colorHSV.v = value.Value;
		}
		colorResult.Value = colorHSV.ToColor();
	}
}
