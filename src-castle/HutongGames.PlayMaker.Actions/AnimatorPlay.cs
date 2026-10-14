using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Plays a state. This could be used to synchronize your animation with audio or synchronize an Animator over the network.")]
[ActionCategory("Animator")]
public class AnimatorPlay : FsmStateAction
{
	[CheckForComponent(typeof(Animator))]
	[Tooltip("The target. An Animator component is required")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[Tooltip("The name of the state that will be played.")]
	public FsmString stateName;

	[Tooltip("The layer where the state is.")]
	public FsmInt layer;

	[Tooltip("The normalized time at which the state will play")]
	public FsmFloat normalizedTime;

	private Animator _animator;

	public override void Reset()
	{
		gameObject = null;
		stateName = null;
		layer = new FsmInt
		{
			UseVariable = true
		};
		normalizedTime = new FsmFloat
		{
			UseVariable = true
		};
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget == null)
		{
			Finish();
			return;
		}
		_animator = ownerDefaultTarget.GetComponent<Animator>();
		if (_animator != null)
		{
			int num = ((!layer.IsNone) ? layer.Value : (-1));
			float num2 = ((!normalizedTime.IsNone) ? normalizedTime.Value : float.NegativeInfinity);
			_animator.Play(stateName.Value, num, num2);
		}
		Finish();
	}
}
