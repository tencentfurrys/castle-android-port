using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Finds a Game Object by Name and/or Tag. If searching by tags defined as FsmString, optional event to catch when tag doesn't exists")]
	[ActionCategory(ActionCategory.GameObject)]
	public class FindGameObject2 : FsmStateAction
	{
		[Tooltip("The name of the GameObject to find. You can leave this empty if you specify a Tag.")]
		public FsmString objectName;

		[Tooltip("Find a GameObject with this tag. If Object Name is specified then both name and Tag must match.")]
		[UIHint(UIHint.Tag)]
		public FsmString withTag;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a GameObject variable.")]
		public FsmGameObject store;

		[Tooltip("Event fired if tag is not found, typically possible if you are using a FsmString to define the tag")]
		public FsmEvent TagDoesntExistEvent;

		public override void Reset()
		{
			objectName = string.Empty;
			withTag = "Untagged";
			store = null;
			TagDoesntExistEvent = null;
		}

		public override void OnEnter()
		{
			Find();
			Finish();
		}

		private void Find()
		{
			if (withTag.Value != "Untagged")
			{
				if (string.IsNullOrEmpty(objectName.Value))
				{
					try
					{
						store.Value = GameObject.FindGameObjectWithTag(withTag.Value);
						return;
					}
					catch (UnityException)
					{
						base.Fsm.Event(TagDoesntExistEvent);
						return;
					}
				}
				GameObject[] array = GameObject.FindGameObjectsWithTag(withTag.Value);
				GameObject[] array2 = array;
				foreach (GameObject gameObject in array2)
				{
					if (gameObject.name == objectName.Value)
					{
						store.Value = gameObject;
						return;
					}
				}
				store.Value = null;
			}
			else
			{
				store.Value = GameObject.Find(objectName.Value);
			}
		}

		public override string ErrorCheck()
		{
			if (string.IsNullOrEmpty(objectName.Value) && string.IsNullOrEmpty(withTag.Value))
			{
				return "Specify Name, Tag, or both.";
			}
			return null;
		}
	}
}
