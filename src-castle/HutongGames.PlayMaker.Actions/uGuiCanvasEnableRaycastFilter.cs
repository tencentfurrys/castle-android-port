using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Enable or disable the raycasting of events via PlayMakerUGuiCanvasRaycastFilterEventsProxy component. Optionally reset on state exit")]
[ActionCategory("uGui")]
public class uGuiCanvasEnableRaycastFilter : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(PlayMakerUGuiCanvasRaycastFilterEventsProxy))]
	[Tooltip("The GameObject with the PlayMakerUGuiCanvasRaycastFilterEventsProxy component.")]
	public FsmOwnerDefault gameObject;

	public FsmBool enableRaycasting;

	[Tooltip("Reset when exiting this state.")]
	public FsmBool resetOnExit;

	public bool everyFrame;

	private PlayMakerUGuiCanvasRaycastFilterEventsProxy _comp;

	private bool _originalValue;

	public override void Reset()
	{
		gameObject = null;
		enableRaycasting = false;
		resetOnExit = null;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_comp = ownerDefaultTarget.GetComponent<PlayMakerUGuiCanvasRaycastFilterEventsProxy>();
			_originalValue = _comp.RayCastingEnabled;
		}
		DoAction();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoAction();
	}

	private void DoAction()
	{
		if (_comp != null)
		{
			_comp.RayCastingEnabled = enableRaycasting.Value;
		}
	}

	public override void OnExit()
	{
		if (!(_comp == null) && resetOnExit.Value)
		{
			_comp.RayCastingEnabled = _originalValue;
		}
	}
}
