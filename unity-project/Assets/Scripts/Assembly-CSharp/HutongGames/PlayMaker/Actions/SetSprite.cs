using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a Sprite on any GameObject. Object must have a Sprite Renderer.")]
	[ActionCategory("sprite")]
	public class SetSprite : FsmStateAction
	{
		[Tooltip("The GameObject with a Sprite Renderer.")]
		[RequiredField]
		[CheckForComponent(typeof(SpriteRenderer))]
		public FsmOwnerDefault gameObject;

		[Tooltip("The Sprite to set")]
		[ObjectType(typeof(Sprite))]
		public FsmObject sprite;

		private SpriteRenderer spriteRenderer;

		public override void Reset()
		{
			gameObject = null;
			sprite = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				spriteRenderer = ownerDefaultTarget.GetComponent<SpriteRenderer>();
				if (spriteRenderer == null)
				{
					LogError("SwapSingleSprite: Missing SpriteRenderer!");
					return;
				}
				DoSetSprite();
				Finish();
			}
		}

		private void DoSetSprite()
		{
			spriteRenderer.sprite = sprite.Value as Sprite;
		}
	}
}
