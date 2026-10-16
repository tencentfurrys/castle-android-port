using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Forces a Game Object's Rigid Body to wake up.")]
	[ActionCategory(ActionCategory.Physics)]
	public class WakeUp : ComponentAction<Rigidbody>
	{
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			DoWakeUp();
			Finish();
		}

		private void DoWakeUp()
		{
			GameObject go = ((gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? gameObject.GameObject.Value : base.Owner);
			if (UpdateCache(go))
			{
				base.rigidbody.WakeUp();
			}
		}
	}
}
