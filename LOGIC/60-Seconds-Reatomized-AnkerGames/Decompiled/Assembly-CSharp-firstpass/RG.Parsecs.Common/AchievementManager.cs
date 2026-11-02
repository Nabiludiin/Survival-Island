using System;
using System.Collections.Generic;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using Steamworks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace RG.Parsecs.Common;

public class AchievementManager : MonoBehaviour
{
	[Serializable]
	internal struct GoalAchievementConnection
	{
		public Goal Goal;

		public Achievement Achievement;
	}

	public static bool ShouldUseMacAppStoreAchievements;

	[SerializeField]
	private List<Achievement> _achievementsList;

	[SerializeField]
	private List<GoalAchievementConnection> _goalAchievementConnections;

	[SerializeField]
	private GlobalBoolVariable _shouldUseMacAppStoreAchievements;

	private bool _checkedMacAppStoreAchievements;

	private void Start()
	{
		ShouldUseMacAppStoreAchievements = _shouldUseMacAppStoreAchievements.Value;
		for (int i = 0; i < _goalAchievementConnections.Count; i++)
		{
			if (_goalAchievementConnections[i].Goal.IsAchieved && !_goalAchievementConnections[i].Achievement.IsAchieved)
			{
				_goalAchievementConnections[i].Achievement.SetAsAchieved();
			}
		}
		if (SteamManager.Initialized && SteamAPI.IsSteamRunning() && !DemoManager.IS_DEMO_VERSION)
		{
			for (int j = 0; j < _achievementsList.Count; j++)
			{
				if (_achievementsList[j].IsAchieved && !AchievementsSystem.IsAchievementUnlocked(_achievementsList[j]))
				{
					AchievementsSystem.UnlockAchievement(_achievementsList[j]);
				}
				else if (!_achievementsList[j].IsAchieved && AchievementsSystem.IsAchievementUnlocked(_achievementsList[j]))
				{
					_achievementsList[j].SetAsAchieved();
				}
			}
		}
		if ((bool)_shouldUseMacAppStoreAchievements)
		{
			LoadMacAppStoreAchievements();
		}
	}

	public void LoadMacAppStoreAchievements()
	{
		if (!_checkedMacAppStoreAchievements && IsAuthenticated())
		{
			Social.LoadAchievements(ProcessLoadedAchievements);
		}
	}

	private void ProcessLoadedAchievements(IAchievement[] achievements)
	{
		CheckMacAppStoreAchievementsUnlockedLocal(achievements);
		if (achievements.Length != 0)
		{
			_checkedMacAppStoreAchievements = true;
		}
	}

	private void CheckMacAppStoreAchievementsUnlockedLocal(IAchievement[] achievements)
	{
		if (!IsAuthenticated())
		{
			return;
		}
		for (int i = 0; i < _achievementsList.Count; i++)
		{
			bool flag = IsMacAppStoreAchievementInAchievementsArray(_achievementsList[i], achievements);
			if (_achievementsList[i].IsAchieved)
			{
				if (!flag)
				{
					AchievementsSystem.UnlockAchievement(_achievementsList[i]);
				}
			}
			else if (flag)
			{
				_achievementsList[i].SetAsAchieved();
			}
		}
	}

	private bool IsAuthenticated()
	{
		if (Social.Active == null)
		{
			return false;
		}
		if (Social.Active.localUser == null)
		{
			return false;
		}
		bool result = false;
		try
		{
			result = Social.Active.localUser.authenticated;
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
		return result;
	}

	private bool IsMacAppStoreAchievementInAchievementsArray(Achievement localAchievement, IAchievement[] achievements)
	{
		for (int i = 0; i < achievements.Length; i++)
		{
			if (achievements[i].id == localAchievement.MacAppStoreID)
			{
				return true;
			}
		}
		return false;
	}
}
