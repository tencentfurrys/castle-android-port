using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Rotation of a Game Object. To leave any axis unchanged, set variable to 'None'.\n Advanced features allows selection of update type.")]
	[ActionCategory(ActionCategory.Transform)]
	public class SetRotationAdvanced : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to rotate.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Use a stored quaternion, or vector angles below.")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion quaternion;

		[Title("Euler Angles")]
		[Tooltip("Use euler angles stored in a Vector3 variable, and/or set each axis below.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		public FsmFloat xAngle;

		public FsmFloat yAngle;

		public FsmFloat zAngle;

		[Tooltip("Use local or world space.")]
		public Space space;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public PlayMakerActionsUtils.EveryFrameUpdateSelector updateType;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			quaternion = null;
			vector = null;
			xAngle = new FsmFloat
			{
				UseVariable = true
			};
			yAngle = new FsmFloat
			{
				UseVariable = true
			};
			zAngle = new FsmFloat
			{
				UseVariable = true
			};
			space = Space.World;
			everyFrame = false;
			updateType = PlayMakerActionsUtils.EveryFrameUpdateSelector.OnUpdate;
		}

		public override void Awake()
		{
			if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnFixedUpdate)
			{
				base.Fsm.HandleFixedUpdate = true;
			}
		}

		public override void OnUpdate()
		{
			if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnUpdate)
			{
				DoSetRotation();
			}
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnLateUpdate()
		{
			if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnLateUpdate)
			{
				DoSetRotation();
			}
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnFixedUpdate()
		{
			if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnFixedUpdate)
			{
				DoSetRotation();
			}
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetRotation()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				Vector3 vector = ((!quaternion.IsNone) ? quaternion.Value.eulerAngles : (this.vector.IsNone ? ((space != Space.Self) ? ownerDefaultTarget.transform.eulerAngles : ownerDefaultTarget.transform.localEulerAngles) : this.vector.Value));
				if (!xAngle.IsNone)
				{
					vector.x = xAngle.Value;
				}
				if (!yAngle.IsNone)
				{
					vector.y = yAngle.Value;
				}
				if (!zAngle.IsNone)
				{
					vector.z = zAngle.Value;
				}
				if (space == Space.Self)
				{
					ownerDefaultTarget.transform.localEulerAngles = vector;
				}
				else
				{
					ownerDefaultTarget.transform.eulerAngles = vector;
				}
			}
		}
	}
}
