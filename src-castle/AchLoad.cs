using Steamworks;
using UnityEngine;

public class AchLoad : MonoBehaviour
{
	public string AchName;

	public bool AchSet;

	public bool ReSetAllAch;

	private void Load()
	{
		if (AchSet)
		{
			SteamUserStats.SetAchievement(AchName);
			SteamUserStats.StoreStats();
		}
		if (ReSetAllAch)
		{
			SteamUserStats.ResetAllStats(bAchievementsToo: true);
			SteamUserStats.StoreStats();
		}
	}
}
