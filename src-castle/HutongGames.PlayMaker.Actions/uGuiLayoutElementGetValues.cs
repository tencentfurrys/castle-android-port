using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("uGui")]
[Tooltip("Gets various values of a UGui Layout Element component.")]
public class uGuiLayoutElementGetValues : FsmStateAction
{
	[CheckForComponent(typeof(LayoutElement))]
	[RequiredField]
	[Tooltip("The GameObject with the Layout Element component.")]
	public FsmOwnerDefault gameObject;

	[ActionSection("Values")]
	[UIHint(UIHint.Variable)]
	[Tooltip("Is this element use Layout constraints")]
	public FsmBool ignoreLayout;

	[Tooltip("The minimum width enabled state")]
	[UIHint(UIHint.Variable)]
	public FsmBool minWidthEnabled;

	[Tooltip("The minimum width this layout element should have.")]
	[UIHint(UIHint.Variable)]
	public FsmFloat minWidth;

	[UIHint(UIHint.Variable)]
	[Tooltip("The minimum height enabled state")]
	public FsmBool minHeightEnabled;

	[UIHint(UIHint.Variable)]
	[Tooltip("The minimum height this layout element should have.")]
	public FsmFloat minHeight;

	[UIHint(UIHint.Variable)]
	[Tooltip("The preferred width enabled state")]
	public FsmBool preferredWidthEnabled;

	[Tooltip("The preferred width this layout element should have before additional available width is allocated.")]
	[UIHint(UIHint.Variable)]
	public FsmFloat preferredWidth;

	[Tooltip("The preferred height enabled state")]
	[UIHint(UIHint.Variable)]
	public FsmBool preferredHeightEnabled;

	[Tooltip("The preferred height this layout element should have before additional available height is allocated.")]
	[UIHint(UIHint.Variable)]
	public FsmFloat preferredHeight;

	[Tooltip("The flexible width enabled state")]
	[UIHint(UIHint.Variable)]
	public FsmBool flexibleWidthEnabled;

	[UIHint(UIHint.Variable)]
	[Tooltip("The relative amount of additional available width this layout element should fill out relative to its siblings.")]
	public FsmFloat flexibleWidth;

	[UIHint(UIHint.Variable)]
	[Tooltip("The flexible height enabled state")]
	public FsmBool flexibleHeightEnabled;

	[Tooltip("The relative amount of additional available height this layout element should fill out relative to its siblings.")]
	[UIHint(UIHint.Variable)]
	public FsmFloat flexibleHeight;

	[Tooltip("Repeats every frame")]
	[ActionSection("Options")]
	public bool everyFrame;

	private LayoutElement _layoutElement;

	public override void Reset()
	{
		gameObject = null;
		ignoreLayout = null;
		minWidthEnabled = null;
		minHeightEnabled = null;
		preferredWidthEnabled = null;
		preferredHeightEnabled = null;
		flexibleWidthEnabled = null;
		flexibleHeightEnabled = null;
		minWidth = null;
		minHeight = null;
		preferredWidth = null;
		preferredHeight = null;
		flexibleWidth = null;
		flexibleHeight = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_layoutElement = ownerDefaultTarget.GetComponent<LayoutElement>();
		}
		DoGetValues();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoGetValues();
	}

	private void DoGetValues()
	{
		if (_layoutElement != null)
		{
			if (!ignoreLayout.IsNone)
			{
				ignoreLayout.Value = _layoutElement.ignoreLayout;
			}
			if (!minWidthEnabled.IsNone)
			{
				minWidthEnabled.Value = _layoutElement.minWidth == 0f;
			}
			if (!minWidth.IsNone)
			{
				minWidth.Value = _layoutElement.minWidth;
			}
			if (!minHeightEnabled.IsNone)
			{
				minHeightEnabled.Value = _layoutElement.minHeight == 0f;
			}
			if (!minHeight.IsNone)
			{
				minHeight.Value = _layoutElement.minHeight;
			}
			if (!preferredWidthEnabled.IsNone)
			{
				preferredWidthEnabled.Value = _layoutElement.preferredWidth == 0f;
			}
			if (!preferredWidth.IsNone)
			{
				preferredWidth.Value = _layoutElement.preferredWidth;
			}
			if (!preferredHeightEnabled.IsNone)
			{
				preferredHeightEnabled.Value = _layoutElement.preferredHeight == 0f;
			}
			if (!preferredHeight.IsNone)
			{
				preferredHeight.Value = _layoutElement.preferredHeight;
			}
			if (!flexibleWidthEnabled.IsNone)
			{
				flexibleWidthEnabled.Value = _layoutElement.flexibleWidth == 0f;
			}
			if (!flexibleWidth.IsNone)
			{
				flexibleWidth.Value = _layoutElement.flexibleWidth;
			}
			if (!flexibleHeightEnabled.IsNone)
			{
				flexibleHeightEnabled.Value = _layoutElement.flexibleHeight == 0f;
			}
			if (!flexibleHeight.IsNone)
			{
				flexibleHeight.Value = _layoutElement.flexibleHeight;
			}
		}
	}
}
