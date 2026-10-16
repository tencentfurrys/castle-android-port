using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.UnityObject)]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=11807.0")]
	[Tooltip("Sets the value from a script/behaviour/component/etc as an Object Variable.")]
	public class SetObjectValueAdvance : FsmStateAction
	{
		[ActionSection("Input")]
		public FsmOwnerDefault gameObject;

		[Title("Object Name")]
		[Tooltip("The name of the script/behaviour/component/etc to set as an object. Note: No space in script name.")]
		public FsmString objectName;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[ActionSection("Output")]
		public FsmObject objectVariable;

		[ActionSection("Options")]
		public FsmBool debugType;

		public FsmBool everyFrame;

		private Object componentTarget;

		private GameObject go;

		public override void Reset()
		{
			objectVariable = null;
			objectName = null;
			everyFrame = false;
			debugType = false;
		}

		public override void OnEnter()
		{
			if (objectName.Value.Contains(" "))
			{
				Debug.LogWarning("<color=#5F9EA0>Should not be any space. <i>Example: </i>'Sprite Renderer' must be 'SpriteRenderer' & Capital letters are mandatory.</color>", base.Owner);
			}
			doAction(base.Fsm.GetOwnerDefaultTarget(gameObject));
			if (!everyFrame.Value)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			doAction(base.Fsm.GetOwnerDefaultTarget(gameObject));
			if (!everyFrame.Value)
			{
				Finish();
			}
		}

		private void doAction(GameObject gos)
		{
			if (!string.IsNullOrEmpty(objectName.Value))
			{
				componentTarget = gos.gameObject.GetComponent(objectName.Value);
				if (debugType.Value)
				{
					doDebug();
				}
				objectVariable.Value = componentTarget;
			}
			else
			{
				Debug.LogWarning("<color=#A62A2A>Object Name is empty. Please correct</color>", base.Owner);
			}
		}

		public void doDebug()
		{
			Debug.Log("<color=#A62A2A>Type Input: </color>" + componentTarget.GetType().ToString() + "<color=#5F9EA0> Type Output: </color>" + objectVariable.TypeName, base.Owner);
		}
	}
}
