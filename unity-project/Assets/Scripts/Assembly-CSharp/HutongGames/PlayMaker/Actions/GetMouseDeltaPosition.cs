using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the delta Position of the mouse as a Vector3, vector2, or individual axis.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseDeltaPosition : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmVector3 mouseDeltaPosition;

		[UIHint(UIHint.Variable)]
		public FsmVector2 mousePositionDeltaVector2;

		[UIHint(UIHint.Variable)]
		public FsmFloat mouseDeltaPositionX;

		[UIHint(UIHint.Variable)]
		public FsmFloat mouseDeltaPositionY;

		public bool normalize;

		private Vector3 _lastPosition;

		public override void Reset()
		{
			mouseDeltaPosition = null;
			mousePositionDeltaVector2 = null;
			mouseDeltaPositionX = null;
			mouseDeltaPositionY = null;
			normalize = false;
		}

		public override void OnEnter()
		{
			_lastPosition = Input.mousePosition;
			DoGetMouseDelta();
		}

		public override void OnUpdate()
		{
			DoGetMouseDelta();
		}

		private void DoGetMouseDelta()
		{
			Vector3 mousePosition = Input.mousePosition;
			Vector3 value = mousePosition - _lastPosition;
			if (normalize)
			{
				value.x /= Screen.width;
				value.y /= Screen.height;
			}
			if (!mouseDeltaPosition.IsNone)
			{
				mouseDeltaPosition.Value = value;
			}
			if (!mousePositionDeltaVector2.IsNone)
			{
				mousePositionDeltaVector2.Value = new Vector2(value.x, value.y);
			}
			if (!mouseDeltaPositionX.IsNone)
			{
				mouseDeltaPositionX.Value = value.x;
			}
			if (!mouseDeltaPositionY.IsNone)
			{
				mouseDeltaPositionY.Value = value.y;
			}
			_lastPosition = mousePosition;
		}
	}
}
