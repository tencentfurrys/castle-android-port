using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Spherically interpolates between from and to by t.")]
	[ActionCategory("Quaternion")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1093")]
	public class QuaternionSlerp : QuaternionBaseAction
	{
		[RequiredField]
		[Tooltip("From Quaternion.")]
		public FsmQuaternion fromQuaternion;

		[Tooltip("To Quaternion.")]
		[RequiredField]
		public FsmQuaternion toQuaternion;

		[RequiredField]
		[HasFloatSlider(0f, 1f)]
		[Tooltip("Interpolate between fromQuaternion and toQuaternion by this amount. Value is clamped to 0-1 range. 0 = fromQuaternion; 1 = toQuaternion; 0.5 = half way between.")]
		public FsmFloat amount;

		[Tooltip("Amount is multiplied by the deltatime")]
		public bool lerpAgainstDeltaTime;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in this quaternion variable.")]
		[RequiredField]
		public FsmQuaternion storeResult;

		public override void Reset()
		{
			fromQuaternion = new FsmQuaternion
			{
				UseVariable = true
			};
			toQuaternion = new FsmQuaternion
			{
				UseVariable = true
			};
			amount = 0.1f;
			lerpAgainstDeltaTime = false;
			storeResult = null;
			everyFrame = true;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			DoQuatSlerp();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				DoQuatSlerp();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				DoQuatSlerp();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				DoQuatSlerp();
			}
		}

		private void DoQuatSlerp()
		{
			float t = ((!lerpAgainstDeltaTime) ? amount.Value : (Time.deltaTime * amount.Value));
			storeResult.Value = Quaternion.Slerp(fromQuaternion.Value, toQuaternion.Value, t);
		}
	}
}
