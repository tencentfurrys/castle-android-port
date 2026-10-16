using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1095")]
	[Tooltip("Creates a rotation which rotates angle degrees around axis.")]
	[ActionCategory("Quaternion")]
	public class QuaternionAngleAxis : QuaternionBaseAction
	{
		[RequiredField]
		[Tooltip("The angle.")]
		public FsmFloat angle;

		[RequiredField]
		[Tooltip("The rotation axis.")]
		public FsmVector3 axis;

		[RequiredField]
		[Tooltip("Store the rotation of this quaternion variable.")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion result;

		public override void Reset()
		{
			angle = null;
			axis = null;
			result = null;
			everyFrame = true;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			DoQuatAngleAxis();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				DoQuatAngleAxis();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				DoQuatAngleAxis();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				DoQuatAngleAxis();
			}
		}

		private void DoQuatAngleAxis()
		{
			result.Value = Quaternion.AngleAxis(angle.Value, axis.Value);
		}
	}
}
