using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("uGui")]
	[Tooltip("Gets the explicit navigation properties of a Selectable Ugui component. ")]
	public class uGuiNavigationExplicitGetProperties : FsmStateAction
	{
		[CheckForComponent(typeof(Selectable))]
		[Tooltip("The GameObject with the Selectable ui component.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The down Selectable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject selectOnDown;

		[UIHint(UIHint.Variable)]
		[Tooltip("The up Selectable.")]
		public FsmGameObject selectOnUp;

		[UIHint(UIHint.Variable)]
		[Tooltip("The left Selectable.")]
		public FsmGameObject selectOnLeft;

		[Tooltip("The right Selectable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject selectOnRight;

		private Selectable _selectable;

		public override void Reset()
		{
			gameObject = null;
			selectOnDown = null;
			selectOnUp = null;
			selectOnLeft = null;
			selectOnRight = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_selectable = ownerDefaultTarget.GetComponent<Selectable>();
			}
			DoGetValue();
			Finish();
		}

		private void DoGetValue()
		{
			if (_selectable != null)
			{
				if (!selectOnUp.IsNone)
				{
					selectOnUp.Value = ((!(_selectable.navigation.selectOnUp == null)) ? _selectable.navigation.selectOnUp.gameObject : null);
				}
				if (!selectOnDown.IsNone)
				{
					selectOnDown.Value = ((!(_selectable.navigation.selectOnDown == null)) ? _selectable.navigation.selectOnDown.gameObject : null);
				}
				if (!selectOnLeft.IsNone)
				{
					selectOnLeft.Value = ((!(_selectable.navigation.selectOnLeft == null)) ? _selectable.navigation.selectOnLeft.gameObject : null);
				}
				if (!selectOnRight.IsNone)
				{
					selectOnRight.Value = ((!(_selectable.navigation.selectOnRight == null)) ? _selectable.navigation.selectOnRight.gameObject : null);
				}
			}
		}
	}
}
