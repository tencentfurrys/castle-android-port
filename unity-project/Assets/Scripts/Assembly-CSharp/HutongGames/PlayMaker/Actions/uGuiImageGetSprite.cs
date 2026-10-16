using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("uGui")]
	[Tooltip("Gets the source image sprite of a UGui Image component.")]
	public class uGuiImageGetSprite : FsmStateAction
	{
		[CheckForComponent(typeof(Image))]
		[RequiredField]
		[Tooltip("The GameObject with the Image ui component.")]
		public FsmOwnerDefault gameObject;

		[ObjectType(typeof(Sprite))]
		[UIHint(UIHint.Variable)]
		[Tooltip("The source sprite of the UGui Image component.")]
		[RequiredField]
		public FsmObject sprite;

		private Image _image;

		public override void Reset()
		{
			gameObject = null;
			sprite = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_image = ownerDefaultTarget.GetComponent<Image>();
			}
			DoSetImageSourceValue();
			Finish();
		}

		private void DoSetImageSourceValue()
		{
			if (_image != null)
			{
				sprite.Value = _image.sprite;
			}
		}
	}
}
