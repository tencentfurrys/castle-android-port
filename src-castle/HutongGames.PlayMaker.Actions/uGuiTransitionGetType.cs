using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Gets the transition type of a Selectable Ugui component.")]
[ActionCategory("uGui")]
public class uGuiTransitionGetType : FsmStateAction
{
	[CheckForComponent(typeof(Selectable))]
	[Tooltip("The GameObject with the Selectable ui component.")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("The transition value")]
	public FsmString transition;

	[Tooltip("Event sent if transition is ColorTint")]
	public FsmEvent colorTintEvent;

	[Tooltip("Event sent if transition is SpriteSwap")]
	public FsmEvent spriteSwapEvent;

	[Tooltip("Event sent if transition is Animation")]
	public FsmEvent animationEvent;

	[Tooltip("Event sent if transition is none")]
	public FsmEvent noTransitionEvent;

	private Selectable _selectable;

	private Selectable.Transition _originalTransition;

	public override void Reset()
	{
		gameObject = null;
		transition = null;
		colorTintEvent = null;
		spriteSwapEvent = null;
		animationEvent = null;
		noTransitionEvent = null;
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
			transition.Value = _selectable.transition.ToString();
			if (_selectable.transition == Selectable.Transition.None)
			{
				Fsm.Event(noTransitionEvent);
			}
			else if (_selectable.transition == Selectable.Transition.ColorTint)
			{
				Fsm.Event(colorTintEvent);
			}
			else if (_selectable.transition == Selectable.Transition.SpriteSwap)
			{
				Fsm.Event(spriteSwapEvent);
			}
			else if (_selectable.transition == Selectable.Transition.Animation)
			{
				Fsm.Event(animationEvent);
			}
		}
	}
}
