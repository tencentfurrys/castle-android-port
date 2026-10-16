using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the text value of a UGui Text component.")]
	[ActionCategory("uGui")]
	public class uGuiTextGetText : FsmStateAction
	{
		[CheckForComponent(typeof(Text))]
		[RequiredField]
		[Tooltip("The GameObject with the text ui component.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The text value of the UGui Text component.")]
		public FsmString text;

		[Tooltip("Runs everyframe. Useful to animate values over time.")]
		public bool everyFrame;

		private Text _text;

		public override void Reset()
		{
			text = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_text = ownerDefaultTarget.GetComponent<Text>();
			}
			DoGetTextValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetTextValue();
		}

		private void DoGetTextValue()
		{
			if (_text != null)
			{
				text.Value = _text.text;
			}
		}
	}
}
