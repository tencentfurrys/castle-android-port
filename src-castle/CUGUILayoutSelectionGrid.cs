using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

[Title("GUILayout Selection Grid")]
[ActionCategory(ActionCategory.GUILayout)]
[HutongGames.PlayMaker.Tooltip("A GUILayout selection grid.")]
public class CUGUILayoutSelectionGrid : GUILayoutAction
{
	[HutongGames.PlayMaker.Tooltip("Event to be sent when something is selected.")]
	public FsmEvent sendEvent;

	[UIHint(UIHint.Variable)]
	[RequiredField]
	[HutongGames.PlayMaker.Tooltip("Variable where the selected value should be stored.")]
	public FsmString storeResult;

	[RequiredField]
	[HutongGames.PlayMaker.Tooltip("Labels for all options.")]
	public FsmString[] optionsLabels;

	[RequiredField]
	[HutongGames.PlayMaker.Tooltip("Values for all options.")]
	public FsmString[] optionsValues;

	[HutongGames.PlayMaker.Tooltip("The GUI style to be used.")]
	public FsmString style;

	[RequiredField]
	[HutongGames.PlayMaker.Tooltip("The number of columns in the grid")]
	public FsmInt columns = new FsmInt(1);

	private string[] labels = new string[0];

	public override void Reset()
	{
		sendEvent = null;
		storeResult = null;
		optionsLabels = null;
		optionsValues = null;
		style = null;
		columns = new FsmInt(1);
		labels = new string[0];
	}

	public override string ErrorCheck()
	{
		if (optionsLabels.Length != optionsValues.Length)
		{
			return "You did not specify a value for each option.";
		}
		return null;
	}

	private void UpdateLabels()
	{
		if (labels.Length != optionsLabels.Length)
		{
			labels = new string[optionsLabels.Length];
		}
		for (int i = 0; i < optionsLabels.Length; i++)
		{
			if (optionsLabels[i] != null)
			{
				labels[i] = optionsLabels[i].Value;
			}
			else
			{
				labels[i] = "???";
			}
		}
	}

	public override void OnGUI()
	{
		int num = -1;
		if (storeResult.Value != null)
		{
			for (int i = 0; i < optionsValues.Length; i++)
			{
				if (optionsValues[i].Value == storeResult.Value)
				{
					num = i;
					break;
				}
			}
		}
		UpdateLabels();
		int num2 = -1;
		int num3 = columns.Value;
		if (num3 <= 0)
		{
			num3 = 1;
		}
		num2 = ((!string.IsNullOrEmpty(style.Value)) ? GUILayout.SelectionGrid(num, labels, num3, style.Value, LayoutOptions) : GUILayout.SelectionGrid(num, labels, num3, LayoutOptions));
		if (num2 != -1 && num2 != num)
		{
			storeResult.Value = optionsValues[num2].Value;
			Fsm.Event(sendEvent);
		}
	}
}
