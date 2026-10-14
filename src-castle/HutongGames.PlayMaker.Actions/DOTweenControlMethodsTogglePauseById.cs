using DG.Tweening;
using Doozy.PlaymakerActions;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Toggles the play state of all tweens with the given ID (meaning tweens that could be played or paused, depending on the toggle state)")]
[ActionCategory("DOTween")]
[HelpUrl("http://dotween.demigiant.com/documentation.php")]
public class DOTweenControlMethodsTogglePauseById : FsmStateAction
{
	[Tooltip("Select the tween ID type to use")]
	public DOTweenActionsEnums.TweenId tweenIdType;

	[UIHint(UIHint.FsmString)]
	[Tooltip("Use a String as the tween ID")]
	public FsmString stringAsId;

	[Tooltip("Use a Tag as the tween ID")]
	[UIHint(UIHint.Tag)]
	public FsmString tagAsId;

	[Tooltip("Use a GameObject as the tween ID")]
	[UIHint(UIHint.FsmGameObject)]
	public FsmGameObject gameObjectAsId;

	[UIHint(UIHint.FsmBool)]
	[ActionSection("Debug Options")]
	public FsmBool debugThis;

	public override void Reset()
	{
		base.Reset();
		stringAsId = new FsmString
		{
			UseVariable = false
		};
		tagAsId = new FsmString
		{
			UseVariable = false
		};
		gameObjectAsId = new FsmGameObject
		{
			UseVariable = false,
			Value = null
		};
		debugThis = new FsmBool
		{
			Value = false
		};
	}

	public override void OnEnter()
	{
		int num = 0;
		switch (tweenIdType)
		{
		case DOTweenActionsEnums.TweenId.UseString:
			if (!string.IsNullOrEmpty(stringAsId.Value))
			{
				num = DOTween.TogglePause(stringAsId.Value);
			}
			break;
		case DOTweenActionsEnums.TweenId.UseTag:
			if (!string.IsNullOrEmpty(tagAsId.Value))
			{
				num = DOTween.TogglePause(tagAsId.Value);
			}
			break;
		case DOTweenActionsEnums.TweenId.UseGameObject:
			if (gameObjectAsId.Value != null)
			{
				num = DOTween.TogglePause(gameObjectAsId.Value);
			}
			break;
		}
		if (debugThis.Value)
		{
			Debug.Log("GameObject [" + State.Fsm.GameObjectName + "] FSM [" + State.Fsm.Name + "]  State [" + State.Name + "] - DOTween Control Methods TogglePause All - SUCCESS! - Toggled " + num + " tweens");
		}
		Finish();
	}
}
