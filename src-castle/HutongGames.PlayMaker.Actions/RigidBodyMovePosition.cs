using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Physics)]
[Tooltip("Moves a RigidBody to a particular position. It's more efficient then accessing the transform of the gameObject and interacts properly with colliders and physics materials")]
public class RigidBodyMovePosition : FsmStateAction
{
	[CheckForComponent(typeof(Rigidbody))]
	[RequiredField]
	[Tooltip("The rigid body to move.")]
	public FsmOwnerDefault gameObject;

	[UIHint(UIHint.Variable)]
	[Tooltip("The position to move to.")]
	public FsmVector3 position;

	[Tooltip("Repeat every frame.")]
	public bool everyFrame;

	private Rigidbody rigidBody;

	public override void Reset()
	{
		gameObject = null;
		position = null;
		everyFrame = true;
	}

	public override void Awake()
	{
		Fsm.HandleFixedUpdate = true;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget == null)
		{
			LogError("Missing gameObject target");
			return;
		}
		rigidBody = ownerDefaultTarget.GetComponent<Rigidbody>();
		if (rigidBody == null)
		{
			LogError("Missing Rigidbody component");
		}
	}

	public override void OnFixedUpdate()
	{
		DoPosition();
		if (!everyFrame)
		{
			Finish();
		}
	}

	private void DoPosition()
	{
		if (rigidBody != null)
		{
			rigidBody.MovePosition(position.Value);
		}
	}
}
