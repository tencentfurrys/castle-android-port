using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("get the value (zero based index) sprite and text  the Dropdown uGui Component")]
[ActionCategory("uGui")]
public class uGuiDropDownGetSelectedData : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(Dropdown))]
	[Tooltip("The GameObject with the DropDown ui component.")]
	public FsmOwnerDefault gameObject;

	[Tooltip("The selected index of the dropdown (zero based index).")]
	[UIHint(UIHint.Variable)]
	public FsmInt value;

	[Tooltip("The selected text.")]
	[UIHint(UIHint.Variable)]
	public FsmString text;

	[ObjectType(typeof(Sprite))]
	[UIHint(UIHint.Variable)]
	public FsmObject image;

	[Tooltip("Repeats every frame")]
	public bool everyFrame;

	private Dropdown _dropDown;

	private List<Dropdown.OptionData> _options;

	private int sizeOfList;

	public FsmInt MaxIndex;

	private int mIndex;

	public override void Reset()
	{
		gameObject = null;
		value = null;
		text = null;
		image = null;
		everyFrame = false;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_dropDown = ownerDefaultTarget.GetComponent<Dropdown>();
			_options = ownerDefaultTarget.GetComponent<Dropdown>().options;
		}
		sizeOfList = _options.Count;
		mIndex = sizeOfList - 1;
		MaxIndex.Value = mIndex;
		GetValue();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		GetValue();
	}

	private void GetValue()
	{
		if (_dropDown == null)
		{
			return;
		}
		if (!value.IsNone)
		{
			value.Value = _dropDown.value;
		}
		if (!text.IsNone)
		{
			if (mIndex >= _dropDown.value)
			{
				text.Value = _dropDown.options[_dropDown.value].text;
			}
			else
			{
				text.Value = null;
			}
		}
		if (!image.IsNone)
		{
			image.Value = _dropDown.options[_dropDown.value].image;
		}
	}
}
