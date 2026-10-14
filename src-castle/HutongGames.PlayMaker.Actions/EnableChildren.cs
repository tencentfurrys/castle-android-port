using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.GameObject)]
[Tooltip("Enables/Disables all children from the specified parent.")]
[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=15458.0")]
public class EnableChildren : FsmStateAction
{
	[Tooltip("GameObject to change the children off of.")]
	[RequiredField]
	public FsmOwnerDefault parent;

	public bool enable;

	public override void Reset()
	{
		parent = null;
		enable = true;
	}

	public override void OnEnter()
	{
		GameObject ownerDefaultTarget = Fsm.GetOwnerDefaultTarget(parent);
		if (ownerDefaultTarget != null)
		{
			foreach (Transform item in ownerDefaultTarget.transform)
			{
				item.gameObject.SetActive(enable);
			}
		}
		Finish();
	}
}
