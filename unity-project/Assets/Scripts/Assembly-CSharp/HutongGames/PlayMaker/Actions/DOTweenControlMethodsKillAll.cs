using DG.Tweening;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Kills all tweens. A tween is killed automatically when it reaches completion (unless you prevent it using SetAutoKill(false)), but you can use this method to kill it sooner if you don't need it anymore.")]
	[ActionCategory("DOTween")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	public class DOTweenControlMethodsKillAll : FsmStateAction
	{
		[UIHint(UIHint.FsmBool)]
		[Tooltip("If TRUE instantly completes the tween before killing it.")]
		public FsmBool complete;

		[Tooltip("KillAll only > Eventual ids to exclude from the operation.")]
		public FsmString[] idsToExclude;

		[ActionSection("Debug Options")]
		[UIHint(UIHint.FsmBool)]
		public FsmBool debugThis;

		public override void Reset()
		{
			base.Reset();
			complete = new FsmBool
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
			if (idsToExclude.Length > 0)
			{
				string[] array = new string[idsToExclude.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = idsToExclude[i].Value;
				}
				num = DOTween.KillAll(complete.Value, array);
			}
			else
			{
				num = DOTween.KillAll(complete.Value);
			}
			if (debugThis.Value)
			{
				Debug.Log("GameObject [" + base.State.Fsm.GameObjectName + "] FSM [" + base.State.Fsm.Name + "]  State [" + base.State.Name + "] - DOTween Control Methods Kill All - SUCCESS! - Killed " + num + " tweens");
			}
			Finish();
		}
	}
}
