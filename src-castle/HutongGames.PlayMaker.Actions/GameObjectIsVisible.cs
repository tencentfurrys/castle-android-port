using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Logic)]
[Tooltip("Tests if a Game Object is visible.")]
public class GameObjectIsVisible : ComponentAction<Renderer>
{
	[Tooltip("The GameObject to test.")]
	[CheckForComponent(typeof(Renderer))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("Event to send if the GameObject is visible.")]
	public FsmEvent trueEvent;

	[Tooltip("Event to send if the GameObject is NOT visible.")]
	public FsmEvent falseEvent;

	[Tooltip("Store the result in a bool variable.")]
	[UIHint(UIHint.Variable)]
	public FsmBool storeResult;

	public bool everyFrame;

	public override void Reset()
	{
		gameObject = null;
		trueEvent = null;
		falseEvent = null;
		storeResult = null;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		DoIsVisible();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoIsVisible();
	}

	private void DoIsVisible()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			bool isVisible = renderer.isVisible;
			storeResult.Value = isVisible;
			Fsm.Event((!isVisible) ? falseEvent : trueEvent);
		}
	}
}
