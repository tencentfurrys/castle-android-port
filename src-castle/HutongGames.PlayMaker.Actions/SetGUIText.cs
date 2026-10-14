using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.GUIElement)]
[Tooltip("Sets the Text used by the GUIText Component attached to a Game Object.")]
public class SetGUIText : ComponentAction<GUIText>
{
	[CheckForComponent(typeof(GUIText))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	public FsmString text;

	public bool everyFrame;

	public override void Reset()
	{
		gameObject = null;
		text = string.Empty;
	}

	public override void OnEnter()
	{
		DoSetGUIText();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoSetGUIText();
	}

	private void DoSetGUIText()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (UpdateCache(ownerDefaultTarget))
		{
			guiText.text = text.Value;
		}
	}
}
