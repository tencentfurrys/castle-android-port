using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("ArrayMaker/ArrayList")]
[Tooltip("Check if a GameObject ( by name and/or tag) is within an arrayList.")]
public class ArrayListContainsGameObject : ArrayListActions
{
	[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
	[ActionSection("Set up")]
	[RequiredField]
	[CheckForComponent(typeof(PlayMakerArrayListProxy))]
	public FsmOwnerDefault gameObject;

	[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
	public FsmString reference;

	[Tooltip("The name of the GameObject to find in the arrayList. You can leave this empty if you specify a Tag.")]
	public FsmString gameObjectName;

	[UIHint(UIHint.Tag)]
	[Tooltip("Find a GameObject in this arrayList with this tag. If GameObject Name is specified then both name and Tag must match.")]
	public FsmString withTag;

	[Tooltip("Store the result in a GameObject variable.")]
	[ActionSection("Result")]
	[UIHint(UIHint.Variable)]
	public FsmGameObject result;

	[UIHint(UIHint.Variable)]
	public FsmInt resultIndex;

	[Tooltip("Store in a bool wether it contains or not that GameObject")]
	[UIHint(UIHint.Variable)]
	public FsmBool isContained;

	[Tooltip("Event sent if this arraList contains that GameObject")]
	[UIHint(UIHint.FsmEvent)]
	public FsmEvent isContainedEvent;

	[UIHint(UIHint.FsmEvent)]
	[Tooltip("Event sent if this arraList does not contains that GameObject")]
	public FsmEvent isNotContainedEvent;

	public override void Reset()
	{
		gameObject = null;
		reference = null;
		gameObjectName = null;
		result = null;
		resultIndex = null;
		isContained = null;
		isContainedEvent = null;
		isNotContainedEvent = null;
	}

	public override void OnEnter()
	{
		if (!SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
		{
			Finish();
		}
		int num = DoContainsGo();
		if (num >= 0)
		{
			isContained.Value = true;
			result.Value = (GameObject)proxy.arrayList[num];
			resultIndex.Value = num;
			Fsm.Event(isContainedEvent);
		}
		else
		{
			isContained.Value = false;
			Fsm.Event(isNotContainedEvent);
		}
		Finish();
	}

	private int DoContainsGo()
	{
		if (!isProxyValid())
		{
			return -1;
		}
		int num = 0;
		string value = gameObjectName.Value;
		string value2 = withTag.Value;
		foreach (GameObject array in proxy.arrayList)
		{
			if (array != null)
			{
				if (value2 == "Untagged" || withTag.IsNone)
				{
					if (array.name.Equals(value))
					{
						return num;
					}
				}
				else if (string.IsNullOrEmpty(value))
				{
					if (array.tag.Equals(value2))
					{
						return num;
					}
				}
				else if (array.name.Equals(value) && array.tag.Equals(value2))
				{
					return num;
				}
			}
			num++;
		}
		return -1;
	}
}
