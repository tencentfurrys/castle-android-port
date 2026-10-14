using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("uGui")]
[Tooltip("Sets properties of a UGui Canvas Group component.")]
public class uGuiCanvasGroupSetProperties : FsmStateAction
{
	[CheckForComponent(typeof(CanvasGroup))]
	[Tooltip("The GameObject with the Canvas Group component.")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("Canvas group alpha. Ranges from 0.0 to 1.0.")]
	[HasFloatSlider(0f, 1f)]
	public FsmFloat alpha;

	[Tooltip("Is the group interactable (are the elements beneath the group enabled). Leave to none for no effect")]
	public FsmBool interactable;

	[Tooltip("Does this group block raycasting (allow collision). Leave to none for no effect")]
	public FsmBool blocksRaycasts;

	[Tooltip("Should the group ignore parent groups? Leave to none for no effect")]
	public FsmBool ignoreParentGroup;

	[Tooltip("Reset when exiting this state. Leave to none for no effect")]
	public FsmBool resetOnExit;

	public bool everyFrame;

	private CanvasGroup _comp;

	private float _originalAlpha;

	private bool _originalInteractable;

	private bool _originalBlocksRaycasts;

	private bool _originalIgnoreParentGroup;

	public override void Reset()
	{
		gameObject = null;
		alpha = new FsmFloat
		{
			UseVariable = true
		};
		interactable = new FsmBool
		{
			UseVariable = true
		};
		blocksRaycasts = new FsmBool
		{
			UseVariable = true
		};
		ignoreParentGroup = new FsmBool
		{
			UseVariable = true
		};
		resetOnExit = null;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_comp = ownerDefaultTarget.GetComponent<CanvasGroup>();
			_originalAlpha = _comp.alpha;
			_originalInteractable = _comp.interactable;
			_originalBlocksRaycasts = _comp.blocksRaycasts;
			_originalIgnoreParentGroup = _comp.ignoreParentGroups;
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
			if (!alpha.IsNone)
			{
				_comp.alpha = alpha.Value;
			}
			if (!interactable.IsNone)
			{
				_comp.interactable = interactable.Value;
			}
			if (!blocksRaycasts.IsNone)
			{
				_comp.blocksRaycasts = blocksRaycasts.Value;
			}
			if (!ignoreParentGroup.IsNone)
			{
				_comp.ignoreParentGroups = ignoreParentGroup.Value;
			}
		}
	}

	public override void OnExit()
	{
		if (!(_comp == null) && resetOnExit.Value)
		{
			if (!alpha.IsNone)
			{
				_comp.alpha = _originalAlpha;
			}
			if (!interactable.IsNone)
			{
				_comp.interactable = _originalInteractable;
			}
			if (!blocksRaycasts.IsNone)
			{
				_comp.blocksRaycasts = _originalBlocksRaycasts;
			}
			if (!ignoreParentGroup.IsNone)
			{
				_comp.ignoreParentGroups = _originalIgnoreParentGroup;
			}
		}
	}
}
