using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.RenderSettings)]
	[Tooltip("Sets the RenderQueue of an game object's materials. The default render queue for Transparent objects is 3000. If you want something to draw on top, choose a higher queue number")]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=11274.0")]
	public class SetRenderQueue : FsmStateAction
	{
		[Tooltip("The GameObject that owns the Renderer.")]
		[ActionSection("Setup")]
		public FsmOwnerDefault gameObject;

		[Title("RenderQueue Order")]
		[Tooltip("Render queue")]
		public FsmInt[] m_queues;

		public override void Reset()
		{
			gameObject = null;
			m_queues = new FsmInt[1] { 3000 };
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				Debug.LogWarning("missing gameObject: " + ownerDefaultTarget.name);
				Finish();
			}
			RenderSetupObj();
			Finish();
		}

		private void RenderSetupObj()
		{
			Renderer component = base.Fsm.GetOwnerDefaultTarget(gameObject).gameObject.GetComponent<Renderer>();
			if (component == null)
			{
				Debug.LogWarning("missing renderer: " + base.Fsm.GetOwnerDefaultTarget(gameObject).name);
				Finish();
			}
			if (m_queues == null)
			{
				Debug.LogWarning("missing queues data: " + base.Fsm.GetOwnerDefaultTarget(gameObject).name);
				Finish();
			}
			Material[] materials = component.materials;
			for (int i = 0; i < materials.Length && i < m_queues.Length; i++)
			{
				materials[i].renderQueue = m_queues[i].Value;
			}
			Finish();
		}
	}
}
