using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Tests if the value of a Bool Variable has changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
	[ActionCategory(ActionCategory.Logic)]
	public class SpriteChanged : FsmStateAction
	{
		[RequiredField]
		[ObjectType(typeof(Sprite))]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Sprite to set")]
		public FsmObject sprite;

		[Tooltip("Event to send if the variable changes.")]
		public FsmEvent changedEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Set to True if changed.")]
		public FsmBool storeResult;

		private Sprite previousValue;

		public override void Reset()
		{
			sprite = null;
			changedEvent = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			if (sprite.IsNone)
			{
				Finish();
			}
			else
			{
				previousValue = sprite.Value as Sprite;
			}
		}

		public override void OnUpdate()
		{
			storeResult.Value = false;
			if (sprite.Value as Sprite != previousValue)
			{
				storeResult.Value = true;
				base.Fsm.Event(changedEvent);
			}
		}
	}
}
