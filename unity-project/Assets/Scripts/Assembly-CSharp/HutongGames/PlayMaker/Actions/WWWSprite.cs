using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Sprite")]
	[Tooltip("Gets a Sprite from a url and store it in variables.")]
	public class WWWSprite : FsmStateAction
	{
		[Tooltip("Url to download Sprite from.")]
		[RequiredField]
		public FsmString url;

		[Tooltip("Pivot of the Sprite. Default is 0,0.")]
		public FsmVector2 pivot;

		[Tooltip("Pixels to Unit of the Sprite. Default is 1.")]
		public FsmInt pixelsToUnit;

		[Tooltip("Gets a Sprite from the url.")]
		[UIHint(UIHint.Variable)]
		[ObjectType(typeof(Sprite))]
		[ActionSection("Results")]
		public FsmObject storeSprite;

		[UIHint(UIHint.Variable)]
		[Tooltip("Error message if there was an error during the download.")]
		public FsmString errorString;

		[Tooltip("How far the download progressed (0-1).")]
		[UIHint(UIHint.Variable)]
		public FsmFloat progress;

		[Tooltip("Event to send when the sprite has finished loading (progress = 1).")]
		[ActionSection("Events")]
		public FsmEvent isDone;

		[Tooltip("Event to send if there was an error.")]
		public FsmEvent isError;

		private WWW wwwObject;

		public override void Reset()
		{
			url = null;
			pivot = null;
			pixelsToUnit = 1;
			storeSprite = null;
			errorString = null;
			progress = null;
			isDone = null;
		}

		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(url.Value))
			{
				Finish();
			}
			else
			{
				wwwObject = new WWW(url.Value);
			}
		}

		public override void OnUpdate()
		{
			if (wwwObject == null)
			{
				errorString.Value = "WWW Object is Null!";
				Finish();
				return;
			}
			errorString.Value = wwwObject.error;
			if (!string.IsNullOrEmpty(wwwObject.error))
			{
				Finish();
				base.Fsm.Event(isError);
				return;
			}
			progress.Value = wwwObject.progress;
			if (progress.Value.Equals(1f))
			{
				Texture2D texture = wwwObject.texture;
				if (texture != null)
				{
					Rect rect = new Rect(0f, 0f, texture.width, texture.height);
					storeSprite.Value = Sprite.Create(texture, rect, pivot.Value, pixelsToUnit.IsNone ? 1 : pixelsToUnit.Value);
					errorString.Value = wwwObject.error;
				}
				else
				{
					errorString.Value = "Data is not a Texture";
				}
				base.Fsm.Event((!string.IsNullOrEmpty(errorString.Value)) ? isError : isDone);
				Finish();
			}
		}
	}
}
