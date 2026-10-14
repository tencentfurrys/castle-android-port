using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("uGui")]
[Tooltip("Sets the source image sprite of a UGui Image component.")]
public class uGuiImageSetSprite : FsmStateAction
{
	[CheckForComponent(typeof(Image))]
	[Tooltip("The GameObject with the Image ui component.")]
	[RequiredField]
	public FsmOwnerDefault gameObject;

	[ObjectType(typeof(Sprite))]
	[Tooltip("The source sprite of the UGui Image component.")]
	[RequiredField]
	public FsmObject sprite;

	[Tooltip("Reset when exiting this state.")]
	public FsmBool resetOnExit;

	private Image _image;

	private Sprite _originalSprite;

	public override void Reset()
	{
		gameObject = null;
		resetOnExit = false;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(gameObject);
		if (ownerDefaultTarget != null)
		{
			_image = ownerDefaultTarget.GetComponent<Image>();
		}
		DoSetImageSourceValue();
		Finish();
	}

	private void DoSetImageSourceValue()
	{
		if (!(_image == null))
		{
			if (resetOnExit.Value)
			{
				_originalSprite = _image.sprite;
			}
			_image.sprite = (Sprite)sprite.Value;
		}
	}

	public override void OnExit()
	{
		if (!(_image == null) && resetOnExit.Value)
		{
			_image.sprite = _originalSprite;
		}
	}
}
