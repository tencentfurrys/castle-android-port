using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.Animation)]
[Tooltip("Replaces a single Sprite on any GameObject. Object must have a Sprite Renderer.")]
public class SwapSingleSprite : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(SpriteRenderer))]
	public FsmOwnerDefault gameObject;

	public Sprite spriteToSwap;

	private SpriteRenderer spriteRenderer;

	public override void Reset()
	{
		gameObject = null;
		spriteToSwap = new Sprite();
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (!(ownerDefaultTarget == null))
		{
			spriteRenderer = ownerDefaultTarget.GetComponent<SpriteRenderer>();
			if (spriteRenderer == null)
			{
				LogError("SwapSingleSprite: Missing SpriteRenderer!");
				return;
			}
			SwapSprites();
			Finish();
		}
	}

	private void SwapSprites()
	{
		spriteRenderer.sprite = spriteToSwap;
	}
}
