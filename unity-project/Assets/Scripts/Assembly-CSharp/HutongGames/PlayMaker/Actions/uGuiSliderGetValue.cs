using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the value of a UGui Slider component.")]
	[ActionCategory("uGui")]
	public class uGuiSliderGetValue : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Slider))]
		[Tooltip("The GameObject with the slider UGui component.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("The value of the UGui slider component.")]
		[RequiredField]
		public FsmFloat value;

		[Tooltip("Repeats every frame")]
		public bool everyFrame;

		private Slider _slider;

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
				_slider = ownerDefaultTarget.GetComponent<Slider>();
			}
			DoGetValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetValue();
		}

		private void DoGetValue()
		{
			if (_slider != null)
			{
				value.Value = _slider.value;
			}
		}
	}
}
