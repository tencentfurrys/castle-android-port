using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Replaces a single Sprite on any GameObject. Object must have a Sprite Renderer.")]
[ActionCategory("sprite")]
public class SwapSprite : FsmStateAction
{
	[CheckForComponent(typeof(SpriteRenderer))]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[ObjectType(typeof(Sprite))]
	public FsmObject spriteToSwap;

	public FsmBool resetOnExit;

	private SpriteRenderer spriteRenderer;

	private Sprite _orig;

	public override void Reset()
	{
		gameObject = null;
		resetOnExit = false;
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
			_orig = spriteRenderer.sprite;
			SwapSprites();
			Finish();
		}
	}

	public override void OnExit()
	{
		if (resetOnExit.Value)
		{
			spriteRenderer.sprite = _orig;
		}
	}

	private void SwapSprites()
	{
		spriteRenderer.sprite = spriteToSwap.Value as Sprite;
	}
}
