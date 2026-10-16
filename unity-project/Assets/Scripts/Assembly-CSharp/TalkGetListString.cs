using UnityEngine;

public class TalkGetListString : MonoBehaviour
{
	public string GetNowTalk;

	public GameObject Target;

	public int Index;

	public bool error;

	public bool Done;

	public int AllIndex;

	private Talk_Data_List data;

	private void LoadTalk()
	{
		int num = (AllIndex = Target.GetComponent<Talk_Data_List>().database.Count);
		if (Index >= num)
		{
			error = true;
			return;
		}
		GetNowTalk = Target.GetComponent<Talk_Data_List>().database[Index];
		Done = true;
	}
}
