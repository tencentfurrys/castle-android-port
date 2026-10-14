using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Gets the color of a UGui graphic component.")]
[ActionCategory("uGui")]
public class uGuiGraphicGetColor : FsmStateAction
{
	[Tooltip("The GameObject with the ui component.")]
	[RequiredField]
	[CheckForComponent(typeof(Graphic))]
	public FsmOwnerDefault gameObject;

	[UIHint(UIHint.Variable)]
	[RequiredField]
	[Tooltip("The Color of the UI component")]
	public FsmColor color;

	[Tooltip("Repeats every frame")]
	public bool everyFrame;

	private Graphic _component;

	public override void Reset()
	{
		gameObject = null;
		color = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_component = ownerDefaultTarget.GetComponent<Graphic>();
		}
		DoGetColorValue();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoGetColorValue();
	}

	private void DoGetColorValue()
	{
		if (_component != null)
		{
			color.Value = _component.color;
		}
	}
}
