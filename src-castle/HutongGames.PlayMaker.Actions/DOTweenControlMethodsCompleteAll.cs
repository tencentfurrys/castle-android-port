using DG.Tweening;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Sends all tweens to their end position (has no effect with tweens that have infinite loops).")]
[HelpUrl("http://dotween.demigiant.com/documentation.php")]
[ActionCategory("DOTween")]
public class DOTweenControlMethodsCompleteAll : FsmStateAction
{
	[UIHint(UIHint.FsmBool)]
	[Tooltip("For Sequences only: if TRUE internal Sequence callbacks will be fired, otherwise they will be ignored.")]
	public FsmBool withCallbacks;

	[UIHint(UIHint.FsmBool)]
	[ActionSection("Debug Options")]
	public FsmBool debugThis;

	public override void Reset()
	{
		base.Reset();
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
		int num = DOTween.CompleteAll(withCallbacks.Value);
		if (debugThis.Value)
		{
			Debug.Log("GameObject [" + State.Fsm.GameObjectName + "] FSM [" + State.Fsm.Name + "]  State [" + State.Name + "] - DOTween Control Methods Complete All - SUCCESS! - Completed " + num + " tweens");
		}
		Finish();
	}
}
