using DG.Tweening;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[HelpUrl("http://dotween.demigiant.com/documentation.php")]
[ActionCategory("DOTween")]
[Tooltip("Clears all cached tween pools.")]
public class DOTweenAdditionalMethodsClearCatchedTweens : FsmStateAction
{
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
		DOTween.ClearCachedTweens();
		if (debugThis.Value)
		{
			Debug.Log("GameObject [" + State.Fsm.GameObjectName + "] FSM [" + State.Fsm.Name + "]  State [" + State.Name + "] - DOTween Additional Methods Clear Cached Tweens - SUCCESS!");
		}
		Finish();
	}
}
