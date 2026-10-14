using Steamworks;
using UnityEngine;

public class AchLoad : MonoBehaviour
{
	public string AchName;

	public bool AchSet;

	public bool ReSetAllAch;

	private void Load()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			Debug.Log("[CastlePort] no achievements on Android.");
			return;
		}
		if (AchSet)
		{
			SteamUserStats.SetAchievement(AchName);
			SteamUserStats.StoreStats();
		}
		if (ReSetAllAch)
		{
			SteamUserStats.ResetAllStats(true);
			SteamUserStats.StoreStats();
		}
	}
}
