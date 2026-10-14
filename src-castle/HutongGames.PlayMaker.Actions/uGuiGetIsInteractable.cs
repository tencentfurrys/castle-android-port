using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Gets the interactable flag of a Selectable Ugui component.")]
[ActionCategory("uGui")]
public class uGuiGetIsInteractable : FsmStateAction
{
	[CheckForComponent(typeof(Selectable))]
	[RequiredField]
	[Tooltip("The GameObject with the Selectable ui component.")]
	public FsmOwnerDefault gameObject;

	[UIHint(UIHint.Variable)]
	[Tooltip("The Interactable value")]
	public FsmBool isInteractable;

	[Tooltip("Event sent if Component is Interactable")]
	public FsmEvent isInteractableEvent;

	[Tooltip("Event sent if Component is not Interactable")]
	public FsmEvent isNotInteractableEvent;

	private Selectable _selectable;

	private bool _originalState;

	public override void Reset()
	{
		gameObject = null;
		isInteractable = null;
		isInteractableEvent = null;
		isNotInteractableEvent = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_selectable = ownerDefaultTarget.GetComponent<Selectable>();
		}
		DoGetValue();
		Finish();
	}

	private void DoGetValue()
	{
		if (!(_selectable == null))
		{
			bool flag = _selectable.IsInteractable();
			isInteractable.Value = flag;
			if (flag)
			{
				Fsm.Event(isInteractableEvent);
			}
			else
			{
				Fsm.Event(isNotInteractableEvent);
			}
		}
	}
}
