using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Have an object follow the mouse.")]
	public class FollowMouse : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to Follow the mouse")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Show the object a specific distance on the X axis, away from the mouse.")]
		public FsmFloat offsetX;

		[Tooltip("Show the object a specific distance on the X axis, away from the mouse.")]
		public FsmFloat offsetY;

		[Tooltip("Select this if you don't want the object to move off screen with the mouse.")]
		public FsmBool keepOnScreen;

		[Tooltip("This determines how close to the edge of the screen the object can get IF Keep On Screen is used.")]
		public FsmFloat screenOffset;

		[Tooltip("The distance to the camera")]
		public FsmFloat distanceToCamera;

		private float screenHeight;

		private float screenWidth;

		public override void Reset()
		{
			gameObject = null;
			offsetX = null;
			offsetY = null;
			keepOnScreen = true;
			screenOffset = null;
			distanceToCamera = 4f;
		}

		public override void OnEnter()
		{
			GetScreenSize();
			DoSetPosition();
		}

		public override void OnUpdate()
		{
			DoSetPosition();
		}

		private void GetScreenSize()
		{
			screenHeight = Screen.height;
			screenWidth = Screen.width;
		}

		private void DoSetPosition()
		{
			if (Camera.main == null)
			{
				LogError("No MainCamera defined!");
				Finish();
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 zero = Vector3.zero;
			zero.x = Input.mousePosition.x + offsetX.Value;
			zero.y = Input.mousePosition.y + offsetY.Value;
			if (keepOnScreen.Value)
			{
				float num = 0f + screenOffset.Value;
				float num2 = screenWidth - screenOffset.Value;
				float num3 = 0f + screenOffset.Value;
				float num4 = screenHeight - screenOffset.Value;
				if (zero.x < num)
				{
					zero.x = num;
				}
				if (zero.x > num2)
				{
					zero.x = num2;
				}
				if (zero.y < num3)
				{
					zero.y = num3;
				}
				if (zero.y > num4)
				{
					zero.y = num4;
				}
			}
			zero.z = distanceToCamera.Value;
			ownerDefaultTarget.transform.position = Camera.main.ScreenToWorldPoint(zero);
		}
	}
}
