using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the minimum and maximum limits for the value of a UGui Slider component.")]
	[ActionCategory("uGui")]
	public class uGuiSliderGetMinMax : FsmStateAction
	{
		[CheckForComponent(typeof(Slider))]
		[Tooltip("The GameObject with the slider UGui component.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The minimum value of the UGui slider component. Leave to none for no effect")]
		[UIHint(UIHint.Variable)]
		public FsmFloat minValue;

		[Tooltip("The maximum value of the UGui slider component. Leave to none for no effect")]
		[UIHint(UIHint.Variable)]
		public FsmFloat maxValue;

		private Slider _slider;

		public override void Reset()
		{
			gameObject = null;
			minValue = null;
			maxValue = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_slider = ownerDefaultTarget.GetComponent<Slider>();
			}
			DoGetValue();
		}

		private void DoGetValue()
		{
			if (_slider != null)
			{
				if (!minValue.IsNone)
				{
					minValue.Value = _slider.minValue;
				}
				if (!maxValue.IsNone)
				{
					maxValue.Value = _slider.maxValue;
				}
			}
		}
	}
}
