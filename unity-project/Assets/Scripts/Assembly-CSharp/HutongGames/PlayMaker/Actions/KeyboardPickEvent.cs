using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends Events based on keyboard stroke and current mouse position interactions with a Game Object. Use Ray Distance to set how close the camera must be to pick the object.")]
	[ActionCategory(ActionCategory.Input)]
	public class KeyboardPickEvent : FsmStateAction
	{
		[CheckForComponent(typeof(Collider))]
		public FsmOwnerDefault GameObject;

		[Tooltip("Length of the ray to cast from the camera.")]
		public FsmFloat rayDistance;

		[Tooltip("The key to test.")]
		[RequiredField]
		public KeyCode key;

		[Tooltip("Event to send when the mouse is over the GameObject.")]
		public FsmEvent mouseOver;

		[Tooltip("Event to send when the key is pressed while mouse over the GameObject.")]
		public FsmEvent keyDown;

		[Tooltip("Event to send when the key is released while over the GameObject.")]
		public FsmEvent keyUp;

		[Tooltip("Event to send when the mouse moves off the GameObject.")]
		public FsmEvent mouseOff;

		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			GameObject = null;
			rayDistance = 100f;
			key = KeyCode.Space;
			mouseOver = null;
			keyDown = null;
			keyUp = null;
			mouseOff = null;
			layerMask = new FsmInt[0];
			invertMask = false;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoMousePickEvent();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMousePickEvent();
		}

		private void DoMousePickEvent()
		{
			bool flag = DoRaycast();
			base.Fsm.RaycastHitInfo = ActionHelpers.mousePickInfo;
			if (flag)
			{
				if (keyDown != null && Input.GetKeyDown(key))
				{
					base.Fsm.Event(keyDown);
				}
				if (mouseOver != null)
				{
					base.Fsm.Event(mouseOver);
				}
				if (keyUp != null && Input.GetKeyUp(key))
				{
					base.Fsm.Event(keyUp);
				}
			}
			else if (mouseOff != null)
			{
				base.Fsm.Event(mouseOff);
			}
		}

		private bool DoRaycast()
		{
			GameObject gameObject = ((GameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? GameObject.GameObject.Value : base.Owner);
			return ActionHelpers.IsMouseOver(gameObject, rayDistance.Value, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));
		}

		public override string ErrorCheck()
		{
			string empty = string.Empty;
			empty += ActionHelpers.CheckRayDistance(rayDistance.Value);
			return empty + ActionHelpers.CheckPhysicsSetup(GameObject);
		}
	}
}
