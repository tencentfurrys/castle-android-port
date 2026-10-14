using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Sets the Background Color used by the Camera.")]
[ActionCategory(ActionCategory.Camera)]
public class SetBackgroundColor : ComponentAction<Camera>
{
	[RequiredField]
	[CheckForComponent(typeof(Camera))]
	public FsmOwnerDefault gameObject;

	[RequiredField]
	public FsmColor backgroundColor;

	public bool everyFrame;

	public override void Reset()
	{
		gameObject = null;
		backgroundColor = Color.black;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		DoSetBackgroundColor();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoSetBackgroundColor();
	}

	private void DoSetBackgroundColor()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			camera.backgroundColor = backgroundColor.Value;
		}
	}
}
