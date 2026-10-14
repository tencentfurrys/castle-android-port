namespace HutongGames.PlayMaker.Actions;

[Tooltip("Sets the value of many GameObject Variable.")]
[ActionCategory(ActionCategory.GameObject)]
[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=9989.0")]
public class SetMultiGameObjectValue : FsmStateAction
{
	[RequiredField]
	[UIHint(UIHint.Variable)]
	[CompoundArray("Count", "GameObject Variable", "Value")]
	public FsmGameObject[] gameobjectVariable;

	public FsmGameObject[] values;

	public bool everyFrame;

	public override void Reset()
	{
		gameobjectVariable = new FsmGameObject[1];
		values = new FsmGameObject[1];
		everyFrame = false;
	}

	public override void OnEnter()
	{
		DoSetGameObjectValue();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		DoSetGameObjectValue();
	}

	public void DoSetGameObjectValue()
	{
		for (int i = 0; i < gameobjectVariable.Length; i++)
		{
			if (!gameobjectVariable[i].IsNone || !gameobjectVariable[i].Value.Equals(string.Empty))
			{
				gameobjectVariable[i].Value = values[i].Value;
			}
		}
	}
}
