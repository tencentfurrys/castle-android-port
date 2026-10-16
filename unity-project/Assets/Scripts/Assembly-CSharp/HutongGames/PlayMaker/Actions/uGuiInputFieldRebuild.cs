using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rebuild a UGui Graphics component.")]
	[ActionCategory("uGui")]
	public class uGuiInputFieldRebuild : FsmStateAction
	{
		[Tooltip("The GameObject with the ui canvas Element component.")]
		[RequiredField]
		[CheckForComponent(typeof(Graphic))]
		public FsmOwnerDefault gameObject;

		public CanvasUpdate canvasUpdate;

		[Tooltip("Only Rebuild when state exits.")]
		public bool rebuildOnExit;

		private Graphic _graphic;

		public override void Reset()
		{
			gameObject = null;
			canvasUpdate = CanvasUpdate.LatePreRender;
			rebuildOnExit = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				_graphic = ownerDefaultTarget.GetComponent<Graphic>();
			}
			if (!rebuildOnExit)
			{
				DoAction();
			}
			Finish();
		}

		private void DoAction()
		{
			if (_graphic != null)
			{
				_graphic.Rebuild(canvasUpdate);
			}
		}

		public override void OnExit()
		{
			if (rebuildOnExit)
			{
				DoAction();
			}
		}
	}
}
