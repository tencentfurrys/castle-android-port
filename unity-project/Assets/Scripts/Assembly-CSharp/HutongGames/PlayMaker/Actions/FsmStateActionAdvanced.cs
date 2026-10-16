namespace HutongGames.PlayMaker.Actions
{
	public abstract class FsmStateActionAdvanced : FsmStateAction
	{
		public enum FrameUpdateSelector
		{
			OnUpdate = 0,
			OnLateUpdate = 1,
			OnFixedUpdate = 2
		}

		[Tooltip("Repeat every frame.")]
		[ActionSection("Update type")]
		public bool everyFrame;

		public FrameUpdateSelector updateType;

		public abstract void OnActionUpdate();

		public override void Reset()
		{
			everyFrame = false;
			updateType = FrameUpdateSelector.OnUpdate;
		}

		public override void Awake()
		{
			if (updateType == FrameUpdateSelector.OnFixedUpdate)
			{
				base.Fsm.HandleFixedUpdate = true;
			}
		}

		public override void OnUpdate()
		{
			if (updateType == FrameUpdateSelector.OnUpdate)
			{
				OnActionUpdate();
			}
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnLateUpdate()
		{
			if (updateType == FrameUpdateSelector.OnLateUpdate)
			{
				OnActionUpdate();
			}
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnFixedUpdate()
		{
			if (updateType == FrameUpdateSelector.OnFixedUpdate)
			{
				OnActionUpdate();
			}
			if (!everyFrame)
			{
				Finish();
			}
		}
	}
}
