using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Add multiple options to the options of the Dropdown uGui Component")]
	[ActionCategory("uGui")]
	public class uGuiDropDownAddOptions : FsmStateAction
	{
		[CheckForComponent(typeof(Dropdown))]
		[Tooltip("The GameObject with the DropDown ui component.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The Options.")]
		[CompoundArray("Options", "Text", "Image")]
		public FsmString[] optionText;

		[ObjectType(typeof(Sprite))]
		public FsmObject[] optionImage;

		private Dropdown _dropDown;

		private List<Dropdown.OptionData> _options;

		public override void Reset()
		{
			gameObject = null;
			optionText = new FsmString[1];
			optionImage = new FsmObject[1];
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_dropDown = ownerDefaultTarget.GetComponent<Dropdown>();
			}
			DoAddOptions();
			Finish();
		}

		private void DoAddOptions()
		{
			if (!(_dropDown == null))
			{
				_options = new List<Dropdown.OptionData>();
				int num = 0;
				FsmString[] array = optionText;
				foreach (FsmString fsmString in array)
				{
					_options.Add(new Dropdown.OptionData
					{
						text = fsmString.Value
					});
					num++;
				}
				_dropDown.AddOptions(_options);
			}
		}
	}
}
