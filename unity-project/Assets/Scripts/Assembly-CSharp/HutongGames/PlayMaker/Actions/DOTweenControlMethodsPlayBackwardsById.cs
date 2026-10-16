using DG.Tweening;
using Doozy.PlaymakerActions;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[Tooltip("Plays backwards all tweens with the given ID (meaning the tweens that were not already started, playing backwards or rewinded)")]
	[ActionCategory("DOTween")]
	public class DOTweenControlMethodsPlayBackwardsById : FsmStateAction
	{
		[Tooltip("Select the tween ID type to use")]
		public DOTweenActionsEnums.TweenId tweenIdType;

		[Tooltip("Use a String as the tween ID")]
		[UIHint(UIHint.FsmString)]
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
					num = DOTween.PlayBackwards(stringAsId.Value);
				}
				break;
			case DOTweenActionsEnums.TweenId.UseTag:
				if (!string.IsNullOrEmpty(tagAsId.Value))
				{
					num = DOTween.PlayBackwards(tagAsId.Value);
				}
				break;
			case DOTweenActionsEnums.TweenId.UseGameObject:
				if (gameObjectAsId.Value != null)
				{
					num = DOTween.PlayBackwards(gameObjectAsId.Value);
				}
				break;
			}
			if (debugThis.Value)
			{
				Debug.Log("GameObject [" + base.State.Fsm.GameObjectName + "] FSM [" + base.State.Fsm.Name + "]  State [" + base.State.Name + "] - DOTween Control Methods Play Backwards By Id - SUCCESS! - Played " + num + " tweens");
			}
			Finish();
		}
	}
}
