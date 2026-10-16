using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Get Mouse Position in screen space.")]
	public class GetMousePositionAdvanced : FsmStateAction
	{
		[Tooltip("Store the position in this Vector3")]
		[RequiredField]
		public FsmVector3 storeMousePosition;

		[Tooltip("Do this every frame?")]
		public bool everyFrame;

		[Tooltip("Get the position relative to the center of the screen? (note: doesn't work with Invert Y checked)")]
		public bool fromScreenCenter;

		[Tooltip("Invery Y. For use with GUI space.")]
		public bool invertY;

		[Tooltip("Normalize the Vector3 result?")]
		public bool normalized;

		public override void Reset()
		{
			storeMousePosition = new FsmVector3
			{
				UseVariable = true
			};
			everyFrame = false;
			fromScreenCenter = false;
			normalized = false;
			invertY = false;
		}

		public override void OnEnter()
		{
			DoGetPosition();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetPosition();
		}

		private void DoGetPosition()
		{
			Vector3 mousePosition = Input.mousePosition;
			if (invertY)
			{
				mousePosition.y = (float)Screen.height - mousePosition.y;
			}
			if (fromScreenCenter)
			{
				mousePosition.x -= Screen.width / 2;
				mousePosition.y -= Screen.height / 2;
			}
			if (normalized)
			{
				mousePosition.x /= Screen.width;
				mousePosition.y /= Screen.height;
			}
			storeMousePosition.Value = mousePosition;
		}
	}
}
