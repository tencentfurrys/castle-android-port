using DG.Tweening;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[Tooltip("Plays forward all tweens (meaning tweens that were not already playing forward or complete)")]
	[ActionCategory("DOTween")]
	public class DOTweenControlMethodsPlayForwardAll : FsmStateAction
	{
		[UIHint(UIHint.FsmBool)]
		[ActionSection("Debug Options")]
		public FsmBool debugThis;

		public override void Reset()
		{
			base.Reset();
			debugThis = new FsmBool
			{
				Value = false
			};
		}

		public override void OnEnter()
		{
			int num = DOTween.PlayForwardAll();
			if (debugThis.Value)
			{
				Debug.Log("GameObject [" + base.State.Fsm.GameObjectName + "] FSM [" + base.State.Fsm.Name + "]  State [" + base.State.Name + "] - DOTween Control Methods Play Forward All - SUCCESS! - Played " + num + " tweens");
			}
			Finish();
		}
	}
}
