using DG.Tweening;
using Doozy.PlaymakerActions;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[HelpUrl("http://dotween.demigiant.com/documentation.php")]
[Tooltip("Sends all tweens with the given ID to their end position (has no effect with tweens that have infinite loops).")]
[ActionCategory("DOTween")]
public class DOTweenControlMethodsCompleteById : FsmStateAction
{
	[Tooltip("Select the tween ID type to use")]
	public DOTweenActionsEnums.TweenId tweenIdType;

	[Tooltip("Use a String as the tween ID")]
	[UIHint(UIHint.FsmString)]
	public FsmString stringAsId;

	[Tooltip("Use a Tag as the tween ID")]
	[UIHint(UIHint.Tag)]
	public FsmString tagAsId;

	[UIHint(UIHint.FsmGameObject)]
	[Tooltip("Use a GameObject as the tween ID")]
	public FsmGameObject gameObjectAsId;

	[Tooltip("For Sequences only: if TRUE internal Sequence callbacks will be fired, otherwise they will be ignored.")]
	[UIHint(UIHint.FsmBool)]
	public FsmBool withCallbacks;

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
		withCallbacks = new FsmBool
		{
			UseVariable = false,
			Value = false
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
				num = DOTween.Complete(stringAsId.Value, withCallbacks.Value);
			}
			break;
		case DOTweenActionsEnums.TweenId.UseTag:
			if (!string.IsNullOrEmpty(tagAsId.Value))
			{
				num = DOTween.Complete(tagAsId.Value, withCallbacks.Value);
			}
			break;
		case DOTweenActionsEnums.TweenId.UseGameObject:
			if (gameObjectAsId.Value != null)
			{
				num = DOTween.Complete(gameObjectAsId.Value, withCallbacks.Value);
			}
			break;
		}
		if (debugThis.Value)
		{
			Debug.Log("GameObject [" + State.Fsm.GameObjectName + "] FSM [" + State.Fsm.Name + "]  State [" + State.Name + "] - DOTween Control Methods Complete By Id - SUCCESS! - Completed " + num + " tweens");
		}
		Finish();
	}
}
