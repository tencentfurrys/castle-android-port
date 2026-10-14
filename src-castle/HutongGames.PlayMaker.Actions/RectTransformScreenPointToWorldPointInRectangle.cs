using UnityEngine;
using UnityEngine.EventSystems;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Transform a screen space point to a world position that is on the plane of the given RectTransform. Also check if the plane of the RectTransform is hit, regardless of whether the point is inside the rectangle.")]
[ActionCategory("RectTransform")]
public class RectTransformScreenPointToWorldPointInRectangle : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(RectTransform))]
	[Tooltip("The GameObject target.")]
	public FsmOwnerDefault gameObject;

	[Tooltip("The screenPoint as a Vector2. Leave to none if you want to use the Vector3 alternative")]
	public FsmVector2 screenPointVector2;

	[Tooltip("The screenPoint as a Vector3. Leave to none if you want to use the Vector2 alternative")]
	public FsmVector3 orScreenPointVector3;

	[Tooltip("Define if screenPoint are expressed as normalized screen coordinates (0-1). Otherwise coordinates are in pixels.")]
	public bool normalizedScreenPoint;

	[Tooltip("The Camera. For a RectTransform in a Canvas set to Screen Space - Overlay mode, the cam parameter should be set to null explicitly (default).\nLeave to none and the camera will be the one from EventSystem.current.camera")]
	[CheckForComponent(typeof(Camera))]
	public FsmGameObject camera;

	[Tooltip("Repeat every frame")]
	public bool everyFrame;

	[ActionSection("Result")]
	[UIHint(UIHint.Variable)]
	[Tooltip("Store the world Position of the screenPoint on the RectTransform Plane.")]
	public FsmVector3 worldPosition;

	[UIHint(UIHint.Variable)]
	[Tooltip("True if the plane of the RectTransform is hit, regardless of whether the point is inside the rectangle.")]
	public FsmBool isHit;

	[Tooltip("Event sent if the plane of the RectTransform is hit, regardless of whether the point is inside the rectangle.")]
	public FsmEvent hitEvent;

	[Tooltip("Event sent if the plane of the RectTransform is NOT hit, regardless of whether the point is inside the rectangle.")]
	public FsmEvent noHitEvent;

	private RectTransform _rt;

	private Camera _camera;

	public override void Reset()
	{
		gameObject = null;
		screenPointVector2 = null;
		orScreenPointVector3 = new FsmVector3
		{
			UseVariable = true
		};
		normalizedScreenPoint = false;
		camera = new FsmGameObject
		{
			UseVariable = true
		};
		everyFrame = false;
		worldPosition = null;
		isHit = null;
		hitEvent = null;
		noHitEvent = null;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_rt = ownerDefaultTarget.GetComponent<RectTransform>();
		}
		if (!camera.IsNone)
		{
			_camera = camera.Value.GetComponent<Camera>();
		}
		else
		{
			_camera = EventSystem.current.GetComponent<Camera>();
		}
		DoCheck();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoCheck();
	}

	private void DoCheck()
	{
		if (_rt == null)
		{
			return;
		}
		Vector2 value = screenPointVector2.Value;
		if (!orScreenPointVector3.IsNone)
		{
			value.x = orScreenPointVector3.Value.x;
			value.y = orScreenPointVector3.Value.y;
		}
		if (normalizedScreenPoint)
		{
			value.x *= Screen.width;
			value.y *= Screen.height;
		}
		bool flag = false;
		flag = RectTransformUtility.ScreenPointToWorldPointInRectangle(_rt, value, _camera, out var worldPoint);
		worldPosition.Value = worldPoint;
		if (!isHit.IsNone)
		{
			isHit.Value = flag;
		}
		if (flag)
		{
			if (hitEvent != null)
			{
				Fsm.Event(hitEvent);
			}
		}
		else if (noHitEvent != null)
		{
			Fsm.Event(noHitEvent);
		}
	}
}
