using UnityEngine.EventSystems;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("uGui")]
	[Tooltip("Sets the EventSystem's currently select GameObject.")]
	public class uGuiSetSelectedGameObject : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			DoSetSelectedGameObject();
			Finish();
		}

		private void DoSetSelectedGameObject()
		{
			EventSystem.current.SetSelectedGameObject(gameObject.Value);
		}
	}
}
