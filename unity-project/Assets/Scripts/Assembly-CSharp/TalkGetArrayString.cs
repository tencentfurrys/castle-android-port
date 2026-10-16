using UnityEngine;

public class TalkGetArrayString : MonoBehaviour
{
	public string GetNowTalk;

	public GameObject Target;

	public int Index;

	public bool error;

	public bool Done;

	private Talk_Database data;

	private void LoadTalk()
	{
		int num = Target.GetComponent<Talk_Database>().MyTextArea.Length;
		if (Index >= num)
		{
			error = true;
			return;
		}
		GetNowTalk = Target.GetComponent<Talk_Database>().MyTextArea[Index];
		Done = true;
	}
}
