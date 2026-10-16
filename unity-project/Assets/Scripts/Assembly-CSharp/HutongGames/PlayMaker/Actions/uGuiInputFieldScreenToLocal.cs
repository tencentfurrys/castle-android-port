using System;
using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rebuild a UGui InputField component.")]
	[Obsolete("This action is obsolete; use 'RectTransformScreenPointToLocalPointInRectangle' instead.")]
	[ActionCategory("uGui")]
	public class uGuiInputFieldScreenToLocal : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(InputField))]
		[Tooltip("The GameObject with the InputField ui component.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The screen position")]
		public FsmVector2 screen;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The resulting local position")]
		public FsmVector2 local;

		[Tooltip("Repeats every frame")]
		public bool everyFrame;

		private InputField _inputField;

		public override void Reset()
		{
			gameObject = null;
			screen = null;
			local = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_inputField = ownerDefaultTarget.GetComponent<InputField>();
			}
			DoAction();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoAction();
		}

		private void DoAction()
		{
			if (_inputField != null)
			{
				Debug.LogError("uGuiInputFieldScreenToLocal is obsolete, please use 'RectTransformScreenPointToLocalPointInRectangle' instead");
			}
		}
	}
}
