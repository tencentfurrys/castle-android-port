using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("uGui")]
	[Tooltip("Clear options to the options of the Dropdown uGui Component")]
	public class uGuiDropDownClearOptions : FsmStateAction
	{
		[CheckForComponent(typeof(Dropdown))]
		[Tooltip("The GameObject with the DropDown ui component.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		private Dropdown _dropDown;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_dropDown = ownerDefaultTarget.GetComponent<Dropdown>();
			}
			if (_dropDown != null)
			{
				_dropDown.ClearOptions();
			}
			Finish();
		}
	}
}
