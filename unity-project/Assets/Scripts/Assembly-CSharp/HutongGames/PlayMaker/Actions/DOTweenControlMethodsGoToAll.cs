using DG.Tweening;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends all tweens to the given position (calculating also eventual loop cycles)")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[ActionCategory("DOTween")]
	public class DOTweenControlMethodsGoToAll : FsmStateAction
	{
		[UIHint(UIHint.FsmFloat)]
		[Tooltip("Time position to reach (if higher than the whole tween duration the tween will simply reach its end).")]
		[RequiredField]
		public FsmFloat to;

		[UIHint(UIHint.FsmBool)]
		[Tooltip("If TRUE the tween will play after reaching the given position, otherwise it will be paused.")]
		public FsmBool andPlay;

		[ActionSection("Debug Options")]
		[UIHint(UIHint.FsmBool)]
		public FsmBool debugThis;

		public override void Reset()
		{
			base.Reset();
			to = new FsmFloat
			{
				UseVariable = false
			};
			andPlay = new FsmBool
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
			int num = DOTween.GotoAll(to.Value, andPlay.Value);
			if (debugThis.Value)
			{
				Debug.Log("GameObject [" + base.State.Fsm.GameObjectName + "] FSM [" + base.State.Fsm.Name + "]  State [" + base.State.Name + "] - DOTween Control Methods Go To All - SUCCESS! - " + num + " tweens involved");
			}
			Finish();
		}
	}
}
