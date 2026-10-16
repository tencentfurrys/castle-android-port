using DG.Tweening;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Restarts all tweens")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[ActionCategory("DOTween")]
	public class DOTweenControlMethodsRestartAll : FsmStateAction
	{
		[UIHint(UIHint.FsmBool)]
		[Tooltip("If TRUE includes the eventual tween delay, otherwise skips it.")]
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
			int num = DOTween.RestartAll(includeDelay.Value);
			if (debugThis.Value)
			{
				Debug.Log("GameObject [" + base.State.Fsm.GameObjectName + "] FSM [" + base.State.Fsm.Name + "]  State [" + base.State.Name + "] - DOTween Control Methods Restart All - SUCCESS! - Restarted " + num + " tweens");
			}
			Finish();
		}
	}
}
