using System;
using Steamworks;
using UnityEngine;

namespace RG.Parsecs.Common;

public class AchievementsSystem
{
	public static void UnlockAchievement(Achievement achievement)
	{
		if (SteamManager.Initialized && SteamAPI.IsSteamRunning() && !DemoManager.IS_DEMO_VERSION)
		{
			SteamUserStats.SetAchievement(achievement.SteamId);
			SteamUserStats.StoreStats();
			bool pbAchieved = false;
			SteamUserStats.GetAchievement(achievement.SteamId, out pbAchieved);
			Debug.LogFormat("Achievement Unlocked! Achievement already achieved?: {0}", pbAchieved);
		}
		if (AchievementManager.ShouldUseMacAppStoreAchievements)
		{
			ProgressMacAppStoreAchievement(achievement, 100, 100);
		}
		achievement.SetAsAchieved();
	}

	public static void ProgressAchievement(Achievement achievement, int val, int max)
	{
		if (!DemoManager.IS_DEMO_VERSION)
		{
			SteamProgressAchievement(achievement, val, max);
		}
	}

	private static void ProgressMacAppStoreAchievement(Achievement achievement, int val, int max)
	{
		int num = Mathf.Min(val, max);
		if (IsAuthenticated() && val >= max)
		{
			ReportMacAppStoreAchievement(achievement.MacAppStoreID, num);
		}
	}

	private static void ReportMacAppStoreAchievement(string achievementId, float progress)
	{
		if (IsAuthenticated())
		{
			Social.ReportProgress(achievementId, progress, (bool result) =>
			{
				Debug.Log(result ? ("Reported achievement " + achievementId) : ("Failed to report achievement " + achievementId));
			});
		}
	}

	private static bool IsAuthenticated()
	{
		return Social.localUser.authenticated;
	}

	private static void SteamProgressAchievement(Achievement achievement, int val, int max)
	{
		int value = Mathf.Min(val, max);
		if (SteamManager.Initialized && SteamAPI.IsSteamRunning())
		{
			SteamUserStats.IndicateAchievementProgress(achievement.SteamId, Convert.ToUInt32(value), Convert.ToUInt32(max));
		}
		if (val >= max)
		{
			if (SteamManager.Initialized && SteamAPI.IsSteamRunning())
			{
				SteamUserStats.SetAchievement(achievement.SteamId);
			}
			achievement.SetAsAchieved();
		}
		if (SteamManager.Initialized && SteamAPI.IsSteamRunning())
		{
			SteamUserStats.StoreStats();
		}
	}

	public static bool IsAchievementUnlocked(Achievement achievement)
	{
		bool pbAchieved = false;
		if (SteamManager.Initialized && SteamAPI.IsSteamRunning())
		{
			SteamUserStats.GetAchievement(achievement.SteamId, out pbAchieved);
		}
		return pbAchieved;
	}
}
