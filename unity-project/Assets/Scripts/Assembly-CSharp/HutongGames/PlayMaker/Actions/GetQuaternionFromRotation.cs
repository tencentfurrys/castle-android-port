using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W968")]
	[Tooltip("Creates a rotation which rotates from fromDirection to toDirection. Usually you use this to rotate a transform so that one of its axes eg. the y-axis - follows a target direction toDirection in world space.")]
	[ActionCategory("Quaternion")]
	public class GetQuaternionFromRotation : QuaternionBaseAction
	{
		[Tooltip("the 'from' direction")]
		[RequiredField]
		public FsmVector3 fromDirection;

		[RequiredField]
		[Tooltip("the 'to' direction")]
		public FsmVector3 toDirection;

		[RequiredField]
		[Tooltip("the resulting quaternion")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion result;

		public override void Reset()
		{
			fromDirection = null;
			toDirection = null;
			result = null;
			everyFrame = false;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			DoQuatFromRotation();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				DoQuatFromRotation();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				DoQuatFromRotation();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				DoQuatFromRotation();
			}
		}

		private void DoQuatFromRotation()
		{
			result.Value = Quaternion.FromToRotation(fromDirection.Value, toDirection.Value);
		}
	}
}
