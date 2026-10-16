namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1132")]
	[Tooltip("Gets a quaternion as euler angles.")]
	[ActionCategory("Quaternion")]
	public class GetQuaternionEulerAngles : QuaternionBaseAction
	{
		[Tooltip("The rotation")]
		[RequiredField]
		public FsmQuaternion quaternion;

		[Tooltip("The euler angles of the quaternion.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 eulerAngles;

		public override void Reset()
		{
			quaternion = null;
			eulerAngles = null;
			everyFrame = true;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			GetQuatEuler();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				GetQuatEuler();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				GetQuatEuler();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				GetQuatEuler();
			}
		}

		private void GetQuatEuler()
		{
			eulerAngles.Value = quaternion.Value.eulerAngles;
		}
	}
}
