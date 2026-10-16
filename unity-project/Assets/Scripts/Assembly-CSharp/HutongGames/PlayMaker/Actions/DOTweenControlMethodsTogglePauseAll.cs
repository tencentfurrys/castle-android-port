using DG.Tweening;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[Tooltip("Toggles the play state of all tweens (meaning tweens that could be played or paused, depending on the toggle state)")]
	[ActionCategory("DOTween")]
	public class DOTweenControlMethodsTogglePauseAll : FsmStateAction
	{
		[ActionSection("Debug Options")]
		[UIHint(UIHint.FsmBool)]
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
			int num = DOTween.TogglePauseAll();
			if (debugThis.Value)
			{
				Debug.Log("GameObject [" + base.State.Fsm.GameObjectName + "] FSM [" + base.State.Fsm.Name + "]  State [" + base.State.Name + "] - DOTween Control Methods TogglePause All - SUCCESS! - Toggled " + num + " tweens");
			}
			Finish();
		}
	}
}
