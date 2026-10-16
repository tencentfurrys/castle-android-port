using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the constraints of a rigidBody")]
	[ActionCategory(ActionCategory.Physics)]
	public class SetRigidBodyConstraints : ComponentAction<Rigidbody>
	{
		[Tooltip("The Rigidbody GameObject")]
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("freezeAll option. Leave to none for no effect")]
		public FsmBool freezeAll;

		[Tooltip("The all Positions constraint. Leave to none for no effect")]
		public FsmBool freezePosition;

		[Tooltip("The all Rotations constraint. Leave to none for no effect")]
		public FsmBool freezeRotation;

		[Tooltip("The X Position constraint. Leave to none for no effect")]
		public FsmBool freezePositionX;

		[Tooltip("The Y Position constraint. Leave to none for no effect")]
		public FsmBool freezePositionY;

		[Tooltip("The Z Position constraint. Leave to none for no effect")]
		public FsmBool freezePositionZ;

		[Tooltip("The X Rotation constraint. Leave to none for no effect")]
		public FsmBool freezeRotationX;

		[Tooltip("The Y Rotation constraint. Leave to none for no effect")]
		public FsmBool freezeRotationY;

		[Tooltip("The Z Rotation constraint. Leave to none for no effect")]
		public FsmBool freezeRotationZ;

		[Tooltip("repeats every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			freezeAll = new FsmBool
			{
				UseVariable = true
			};
			freezeRotation = new FsmBool
			{
				UseVariable = true
			};
			freezePosition = new FsmBool
			{
				UseVariable = true
			};
			freezePositionX = new FsmBool
			{
				UseVariable = true
			};
			freezePositionY = new FsmBool
			{
				UseVariable = true
			};
			freezePositionZ = new FsmBool
			{
				UseVariable = true
			};
			freezeRotationX = new FsmBool
			{
				UseVariable = true
			};
			freezeRotationY = new FsmBool
			{
				UseVariable = true
			};
			freezeRotationZ = new FsmBool
			{
				UseVariable = true
			};
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetConstraints();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetConstraints();
		}

		private void DoSetConstraints()
		{
			if (!UpdateCache(base.Fsm.GetOwnerDefaultTarget(gameObject)))
			{
				return;
			}
			if (!freezePositionX.IsNone)
			{
				if (freezePositionX.Value)
				{
					base.rigidbody.constraints = base.rigidbody.constraints | RigidbodyConstraints.FreezePositionX;
				}
				else
				{
					base.rigidbody.constraints = base.rigidbody.constraints & (RigidbodyConstraints)(-3);
				}
			}
			if (!freezePositionY.IsNone)
			{
				if (freezePositionY.Value)
				{
					base.rigidbody.constraints = base.rigidbody.constraints | RigidbodyConstraints.FreezePositionY;
				}
				else
				{
					base.rigidbody.constraints = base.rigidbody.constraints & (RigidbodyConstraints)(-5);
				}
			}
			if (!freezePositionZ.IsNone)
			{
				if (freezePositionZ.Value)
				{
					base.rigidbody.constraints = base.rigidbody.constraints | RigidbodyConstraints.FreezePositionZ;
				}
				else
				{
					base.rigidbody.constraints = base.rigidbody.constraints & (RigidbodyConstraints)(-9);
				}
			}
			if (!freezeRotationX.IsNone)
			{
				if (freezeRotationX.Value)
				{
					base.rigidbody.constraints = base.rigidbody.constraints | RigidbodyConstraints.FreezeRotationX;
				}
				else
				{
					base.rigidbody.constraints = base.rigidbody.constraints & (RigidbodyConstraints)(-17);
				}
			}
			if (!freezeRotationY.IsNone)
			{
				if (freezeRotationY.Value)
				{
					base.rigidbody.constraints = base.rigidbody.constraints | RigidbodyConstraints.FreezeRotationY;
				}
				else
				{
					base.rigidbody.constraints = base.rigidbody.constraints & (RigidbodyConstraints)(-33);
				}
			}
			if (!freezeRotationZ.IsNone)
			{
				if (freezeRotationZ.Value)
				{
					base.rigidbody.constraints = base.rigidbody.constraints | RigidbodyConstraints.FreezeRotationZ;
				}
				else
				{
					base.rigidbody.constraints = base.rigidbody.constraints & (RigidbodyConstraints)(-65);
				}
			}
			if (!freezeAll.IsNone)
			{
				if (freezeAll.Value)
				{
					base.rigidbody.constraints = base.rigidbody.constraints | RigidbodyConstraints.FreezeAll;
				}
				else
				{
					base.rigidbody.constraints = base.rigidbody.constraints & (RigidbodyConstraints)(-127);
				}
			}
			if (!freezePosition.IsNone)
			{
				if (freezePosition.Value)
				{
					base.rigidbody.constraints = base.rigidbody.constraints | RigidbodyConstraints.FreezePosition;
				}
				else
				{
					base.rigidbody.constraints = base.rigidbody.constraints & (RigidbodyConstraints)(-15);
				}
			}
			if (!freezeRotation.IsNone)
			{
				if (freezeRotation.Value)
				{
					base.rigidbody.constraints = base.rigidbody.constraints | RigidbodyConstraints.FreezeRotation;
				}
				else
				{
					base.rigidbody.constraints = base.rigidbody.constraints & (RigidbodyConstraints)(-113);
				}
			}
		}
	}
}
