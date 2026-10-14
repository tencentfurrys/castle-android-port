using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Sets the Color of a Light.")]
[ActionCategory(ActionCategory.Lights)]
public class SetLightColor : ComponentAction<Light>
{
	[RequiredField]
	[CheckForComponent(typeof(Light))]
	public FsmOwnerDefault gameObject;

	[RequiredField]
	public FsmColor lightColor;

	public bool everyFrame;

	public override void Reset()
	{
		gameObject = null;
		lightColor = Color.white;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		DoSetLightColor();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoSetLightColor();
	}

	private void DoSetLightColor()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			light.color = lightColor.Value;
		}
	}
}
