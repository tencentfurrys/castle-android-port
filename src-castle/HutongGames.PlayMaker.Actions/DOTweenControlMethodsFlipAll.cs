using DG.Tweening;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Flips the direction of all the tweens (backwards if it was going forward or viceversa).")]
[HelpUrl("http://dotween.demigiant.com/documentation.php")]
[ActionCategory("DOTween")]
public class DOTweenControlMethodsFlipAll : FsmStateAction
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
		int num = DOTween.FlipAll();
		if (debugThis.Value)
		{
			Debug.Log("GameObject [" + State.Fsm.GameObjectName + "] FSM [" + State.Fsm.Name + "]  State [" + State.Name + "] - DOTween Control Methods Flip All - SUCCESS! - Flipped " + num + " tweens");
		}
		Finish();
	}
}
