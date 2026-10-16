using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).")]
	[ActionCategory("Quaternion")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1094")]
	public class QuaternionEuler : QuaternionBaseAction
	{
		[Tooltip("The Euler angles.")]
		[RequiredField]
		public FsmVector3 eulerAngles;

		[RequiredField]
		[Tooltip("Store the euler angles of this quaternion variable.")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion result;

		public override void Reset()
		{
			eulerAngles = null;
			result = null;
			everyFrame = true;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			DoQuatEuler();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				DoQuatEuler();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				DoQuatEuler();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				DoQuatEuler();
			}
		}

		private void DoQuatEuler()
		{
			result.Value = Quaternion.Euler(eulerAngles.Value);
		}
	}
}
