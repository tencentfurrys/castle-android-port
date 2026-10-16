using UnityEngine;
using UnityEngine.EventSystems;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns the EventSystem's currently select GameObject.")]
	[ActionCategory("uGui")]
	public class uGuiGetSelectedGameObject : FsmStateAction
	{
		[Tooltip("The currently selected GameObject")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject StoreGameObject;

		[Tooltip("Event when the selected GameObject changes")]
		[UIHint(UIHint.Variable)]
		public FsmEvent ObjectChangedEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("If true, each frame will check the currently selected GameObject")]
		public bool everyFrame;

		private GameObject lastGameObject;

		public override void Reset()
		{
			StoreGameObject = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GetCurrentSelectedGameObject();
			lastGameObject = StoreGameObject.Value;
		}

		public override void OnUpdate()
		{
			GetCurrentSelectedGameObject();
			if (StoreGameObject.Value != lastGameObject && ObjectChangedEvent != null)
			{
				base.Fsm.Event(ObjectChangedEvent);
			}
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void GetCurrentSelectedGameObject()
		{
			StoreGameObject.Value = EventSystem.current.currentSelectedGameObject;
		}
	}
}
