using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Transform)]
[Tooltip("Moves a Game Object towards a Target. Optionally sends an event when successful. The Target can be specified as a Game Object or a world Position. If you specify both, then the Position is used as a local offset from the Object's Position.\n Advanced features allows selection of update type.")]
public class MoveTowardsAdvanced : FsmStateAction
{
	[RequiredField]
	[Tooltip("The GameObject to Move")]
	public FsmOwnerDefault gameObject;

	[Tooltip("A target GameObject to move towards. Or use a world Target Position below.")]
	public FsmGameObject targetObject;

	[Tooltip("A world position if no Target Object. Otherwise used as a local offset from the Target Object.")]
	public FsmVector3 targetPosition;

	public FsmBool ignoreX;

	[Tooltip("Ignore any height difference in the target.")]
	public FsmBool ignoreY;

	public FsmBool ignoreZ;

	[Tooltip("The maximum movement speed. HINT: You can make this a variable to change it over time.")]
	[HasFloatSlider(0f, 20f)]
	public FsmFloat maxSpeed;

	[Tooltip("Distance at which the move is considered finished, and the Finish Event is sent.")]
	[HasFloatSlider(0f, 5f)]
	public FsmFloat finishDistance;

	[Tooltip("Event to send when the Finish Distance is reached.")]
	public FsmEvent finishEvent;

	public PlayMakerActionsUtils.EveryFrameUpdateSelector updateType;

	private GameObject go;

	private GameObject goTarget;

	private Vector3 targetPos;

	private Vector3 targetPosWithVertical;

	private Rigidbody _rb;

	public override void Reset()
	{
		gameObject = null;
		targetObject = null;
		maxSpeed = 10f;
		finishDistance = 1f;
		finishEvent = null;
		updateType = PlayMakerActionsUtils.EveryFrameUpdateSelector.OnUpdate;
	}

	public override void Awake()
	{
		if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnFixedUpdate)
		{
			Fsm.HandleFixedUpdate = true;
		}
	}

	public override void OnUpdate()
	{
		if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnUpdate)
		{
			DoMoveTowards();
		}
	}

	public override void OnLateUpdate()
	{
		if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnLateUpdate)
		{
			DoMoveTowards();
		}
	}

	public override void OnFixedUpdate()
	{
		if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnFixedUpdate)
		{
			DoMoveTowards();
		}
	}

	private void DoMoveTowards()
	{
		if (UpdateTargetPos())
		{
			if (_rb != null)
			{
				go.GetComponent<Rigidbody>().position = Vector3.MoveTowards(go.GetComponent<Rigidbody>().position, targetPos, maxSpeed.Value * Time.deltaTime);
			}
			else
			{
				go.transform.position = Vector3.MoveTowards(go.transform.position, targetPos, maxSpeed.Value * Time.deltaTime);
			}
			float magnitude = (go.transform.position - targetPos).magnitude;
			if (magnitude < finishDistance.Value)
			{
				Fsm.Event(finishEvent);
				Finish();
			}
		}
	}

	public bool UpdateTargetPos()
	{
		go = Fsm.GetOwnerDefaultTarget(gameObject);
		if (go == null)
		{
			return false;
		}
		if (go.GetComponent<Rigidbody>() != null)
		{
			_rb = go.GetComponent<Rigidbody>();
		}
		goTarget = targetObject.Value;
		if (goTarget == null && targetPosition.IsNone)
		{
			return false;
		}
		if (goTarget != null)
		{
			targetPos = (targetPosition.IsNone ? goTarget.transform.position : goTarget.transform.TransformPoint(targetPosition.Value));
		}
		else
		{
			targetPos = targetPosition.Value;
		}
		targetPosWithVertical = targetPos;
		if (ignoreX.Value)
		{
			targetPos.x = go.transform.position.x;
		}
		if (ignoreY.Value)
		{
			targetPos.y = go.transform.position.y;
		}
		if (ignoreZ.Value)
		{
			targetPos.z = go.transform.position.z;
		}
		return true;
	}

	public Vector3 GetTargetPos()
	{
		return targetPos;
	}

	public Vector3 GetTargetPosWithVertical()
	{
		return targetPosWithVertical;
	}
}
