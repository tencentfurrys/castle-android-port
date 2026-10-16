using DG.Tweening;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rewinds and pauses all tweens (meaning tweens that were not already rewinded)")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[ActionCategory("DOTween")]
	public class DOTweenControlMethodsRewindAll : FsmStateAction
	{
		[Tooltip("If TRUE includes the eventual tween delay, otherwise skips it.")]
		[UIHint(UIHint.FsmBool)]
		public FsmBool includeDelay;

		[UIHint(UIHint.FsmBool)]
		[ActionSection("Debug Options")]
		public FsmBool debugThis;

		public override void Reset()
		{
			base.Reset();
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
			int num = DOTween.RewindAll(includeDelay.Value);
			if (debugThis.Value)
			{
				Debug.Log("GameObject [" + base.State.Fsm.GameObjectName + "] FSM [" + base.State.Fsm.Name + "]  State [" + base.State.Name + "] - DOTween Control Methods Rewind All - SUCCESS! - Rewinded and paused " + num + " tweens");
			}
			Finish();
		}
	}
}
