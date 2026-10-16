using DG.Tweening;
using Doozy.PlaymakerActions;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Plays forward all tweens with the given ID (meaning tweens that were not already playing forward or complete)")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[ActionCategory("DOTween")]
	public class DOTweenControlMethodsPlayForwardById : FsmStateAction
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
					num = DOTween.PlayForward(stringAsId.Value);
				}
				break;
			case DOTweenActionsEnums.TweenId.UseTag:
				if (!string.IsNullOrEmpty(tagAsId.Value))
				{
					num = DOTween.PlayForward(tagAsId.Value);
				}
				break;
			case DOTweenActionsEnums.TweenId.UseGameObject:
				if (gameObjectAsId.Value != null)
				{
					num = DOTween.PlayForward(gameObjectAsId.Value);
				}
				break;
			}
			if (debugThis.Value)
			{
				Debug.Log("GameObject [" + base.State.Fsm.GameObjectName + "] FSM [" + base.State.Fsm.Name + "]  State [" + base.State.Name + "] - DOTween Control Methods Play Forward By Id - SUCCESS! - Played " + num + " tweens");
			}
			Finish();
		}
	}
}
