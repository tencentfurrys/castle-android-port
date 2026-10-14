using DG.Tweening;
using Doozy.PlaymakerActions;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Rewinds and pauses all tweens with the given ID (meaning tweens that were not already rewinded)")]
[ActionCategory("DOTween")]
[HelpUrl("http://dotween.demigiant.com/documentation.php")]
public class DOTweenControlMethodsRewindById : FsmStateAction
{
	[Tooltip("Select the tween ID type to use")]
	public DOTweenActionsEnums.TweenId tweenIdType;

	[Tooltip("Use a String as the tween ID")]
	[UIHint(UIHint.FsmString)]
	public FsmString stringAsId;

	[UIHint(UIHint.Tag)]
	[Tooltip("Use a Tag as the tween ID")]
	public FsmString tagAsId;

	[UIHint(UIHint.FsmGameObject)]
	[Tooltip("Use a GameObject as the tween ID")]
	public FsmGameObject gameObjectAsId;

	[UIHint(UIHint.FsmBool)]
	[Tooltip("If TRUE includes the eventual tween delay, otherwise skips it.")]
	public FsmBool includeDelay;

	[ActionSection("Debug Options")]
	[UIHint(UIHint.FsmBool)]
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
		includeDelay = new FsmBool
		{
			UseVariable = false,
			Value = true
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
				num = DOTween.Rewind(stringAsId.Value, includeDelay.Value);
			}
			break;
		case DOTweenActionsEnums.TweenId.UseTag:
			if (!string.IsNullOrEmpty(tagAsId.Value))
			{
				num = DOTween.Rewind(tagAsId.Value, includeDelay.Value);
			}
			break;
		case DOTweenActionsEnums.TweenId.UseGameObject:
			if (gameObjectAsId.Value != null)
			{
				num = DOTween.Rewind(gameObjectAsId.Value, includeDelay.Value);
			}
			break;
		}
		if (debugThis.Value)
		{
			Debug.Log("GameObject [" + State.Fsm.GameObjectName + "] FSM [" + State.Fsm.Name + "]  State [" + State.Name + "] - DOTween Control Methods Rewind By Id - SUCCESS! - Rewinded and paused " + num + " tweens");
		}
		Finish();
	}
}
