using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Drag Camera with mouse or touch.")]
[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=10163")]
[ActionCategory(ActionCategory.Camera)]
public class DragCamera : FsmStateAction
{
	public enum ClickVariable
	{
		Left,
		Right,
		Middle
	}

	[ActionSection("Setup")]
	[RequiredField]
	[CheckForComponent(typeof(Camera))]
	[Tooltip("The main camera (should have a Camera component).")]
	[Title("Camera")]
	public FsmGameObject cameraObj;

	[RequiredField]
	[Tooltip("Movement speed.")]
	public FsmFloat speed;

	[RequiredField]
	[Title("Smoothness")]
	[Tooltip("How smooth it should be. Highest number = Higher smoothness.")]
	public FsmInt smoothControl;

	[Tooltip("Use touch and not mouse")]
	[ActionSection("Options")]
	public bool touchInput;

	[Tooltip("Only use Z axis")]
	[Title("Z Axis Only")]
	public bool zoomOnly;

	[Tooltip("Use a key. None = Off")]
	[ActionSection("Mouse Input Options")]
	public KeyCode key;

	public ClickVariable mouseButton;

	[ActionSection("")]
	public FsmBool cameraMoving;

	[Tooltip("Disable Action")]
	public FsmBool disable;

	[Title("Disable and Finish Action")]
	public FsmBool disableExit;

	private bool active;

	private Vector3 swipePos;

	private Vector3 dragPos;

	private int mouseInt;

	private FsmBool storeResult;

	private bool useKey;

	public override void Reset()
	{
		cameraObj = null;
		speed = 1.2f;
		smoothControl = 25;
		active = false;
		touchInput = true;
		disable = false;
		disableExit = false;
		cameraMoving = false;
		zoomOnly = false;
		mouseInt = 0;
		key = KeyCode.None;
		storeResult = false;
	}

	public override void OnEnter()
	{
		setKey();
		switch (mouseButton)
		{
		case ClickVariable.Left:
			mouseInt = 0;
			break;
		case ClickVariable.Right:
			mouseInt = 1;
			break;
		case ClickVariable.Middle:
			mouseInt = 2;
			break;
		}
	}

	public override void OnLateUpdate()
	{
		if (useKey)
		{
			storeResult.Value = Input.GetKey(key);
		}
		if (!disable.Value & !disableExit.Value)
		{
			if (!touchInput)
			{
				MouseControl();
			}
			else
			{
				TouchControl();
			}
		}
		if (disableExit.Value)
		{
			Finish();
		}
	}

	private void MouseControl()
	{
		if (useKey)
		{
			if (Input.GetMouseButtonDown(mouseInt) && !active && storeResult.Value)
			{
				swipePos = Input.mousePosition;
				active = true;
				cameraMoving.Value = false;
			}
			if (Input.GetMouseButtonUp(mouseInt) || !storeResult.Value)
			{
				active = false;
				cameraMoving.Value = false;
				dragPos = swipePos;
			}
			dragCameraAction();
		}
		if (!useKey)
		{
			if (Input.GetMouseButtonDown(mouseInt) && !active)
			{
				swipePos = Input.mousePosition;
				active = true;
				cameraMoving.Value = false;
			}
			if (Input.GetMouseButtonUp(mouseInt))
			{
				active = false;
				dragPos = swipePos;
				cameraMoving.Value = false;
			}
			dragCameraAction();
		}
	}

	private void TouchControl()
	{
		if (Input.touchCount > 0 && !active)
		{
			swipePos = Input.GetTouch(0).position;
			active = true;
			cameraMoving.Value = false;
		}
		if (Input.touchCount == 0)
		{
			active = false;
			dragPos = swipePos;
			cameraMoving.Value = false;
		}
		dragCameraAction();
	}

	private void dragCameraAction()
	{
		if (active && !zoomOnly)
		{
			if (touchInput)
			{
				dragPos = Input.GetTouch(0).position;
			}
			else
			{
				dragPos = Input.mousePosition;
			}
			if (swipePos.x < dragPos.x - (float)(Screen.width / smoothControl.Value))
			{
				cameraMoving.Value = true;
				cameraObj.Value.transform.position -= new Vector3((dragPos.x - swipePos.x) / ((float)Screen.width / speed.Value), 0f, 0f);
			}
			if (swipePos.x > dragPos.x + (float)(Screen.width / smoothControl.Value))
			{
				cameraMoving.Value = true;
				cameraObj.Value.transform.position += new Vector3((swipePos.x - dragPos.x) / ((float)Screen.width / speed.Value), 0f, 0f);
			}
			if (swipePos.y > dragPos.y + (float)(Screen.width / smoothControl.Value))
			{
				cameraMoving.Value = true;
				cameraObj.Value.transform.position += new Vector3(0f, (swipePos.y - dragPos.y) / ((float)Screen.width / speed.Value), 0f);
			}
			if (swipePos.y < dragPos.y - (float)(Screen.width / smoothControl.Value))
			{
				cameraMoving.Value = true;
				cameraObj.Value.transform.position -= new Vector3(0f, (dragPos.y - swipePos.y) / ((float)Screen.width / speed.Value), 0f);
			}
		}
		else
		{
			if (swipePos.y > dragPos.y + (float)(Screen.width / smoothControl.Value))
			{
				cameraMoving.Value = true;
				cameraObj.Value.transform.position += new Vector3(0f, 0f, (swipePos.y - dragPos.y) / ((float)Screen.width / speed.Value));
			}
			if (swipePos.y < dragPos.y - (float)(Screen.width / smoothControl.Value))
			{
				cameraMoving.Value = true;
				cameraObj.Value.transform.position -= new Vector3(0f, 0f, (dragPos.y - swipePos.y) / ((float)Screen.width / speed.Value));
			}
		}
	}

	private void setKey()
	{
		useKey = true;
		if (key == KeyCode.None)
		{
			useKey = false;
		}
	}
}
