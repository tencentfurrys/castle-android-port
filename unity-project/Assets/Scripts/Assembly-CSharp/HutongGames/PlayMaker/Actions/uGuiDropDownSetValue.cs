using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("uGui")]
	[Tooltip("set the value (zero based index) of the Dropdown uGui Component")]
	public class uGuiDropDownSetValue : FsmStateAction
	{
		[Tooltip("The GameObject with the DropDown ui component.")]
		[RequiredField]
		[CheckForComponent(typeof(Dropdown))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The selected index of the dropdown (zero based index).")]
		public FsmInt value;

		[Tooltip("Repeats every frame")]
		public bool everyFrame;

		private Dropdown _dropDown;

		public override void Reset()
		{
			gameObject = null;
			value = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_dropDown = ownerDefaultTarget.GetComponent<Dropdown>();
			}
			SetValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			SetValue();
		}

		private void SetValue()
		{
			if (!(_dropDown == null) && _dropDown.value != value.Value)
			{
				_dropDown.value = value.Value;
			}
		}
	}
}
