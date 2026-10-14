using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Measures the Distance betweens 2 Game Objects or vectors and stores the result in a Float Variable. Vector3 are offseted to GameObjects if declared")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetDistance2 : FsmStateAction
	{
		public FsmOwnerDefault gameObject;

		public FsmVector3 orVector3;

		public FsmGameObject target;

		public FsmVector3 orVector3Target;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			orVector3 = null;
			target = null;
			orVector3Target = null;
			storeResult = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoGetDistance();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetDistance();
		}

		private void DoGetDistance()
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (storeResult != null)
			{
				Vector3 a = Vector3.zero;
				if (gameObject != null)
				{
					a = gameObject.transform.position;
				}
				a += orVector3.Value;
				Vector3 b = Vector3.zero;
				if (target.Value != null)
				{
					b = target.Value.transform.position;
				}
				b += orVector3Target.Value;
				storeResult.Value = Vector3.Distance(a, b);
			}
		}
	}
}
