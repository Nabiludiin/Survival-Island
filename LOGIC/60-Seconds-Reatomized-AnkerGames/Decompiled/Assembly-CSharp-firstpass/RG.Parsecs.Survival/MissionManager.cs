using System.Collections.Generic;
using RG.Core.Base;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class MissionManager : RGMonoBehaviour
{
	[SerializeField]
	private MissionManagerData _missionManagerData;

	[SerializeField]
	private Scheduler _scheduler;

	private Dictionary<string, int> _medalStats;

	private Dictionary<string, int> _allMedalsCount;

	private List<Mission> _allMissions;

	private static MissionManager _instance;

	public static MissionManager Instance => _instance;

	public bool IsMissionOngoing()
	{
		if (_missionManagerData.CurrentMission != null)
		{
			return _missionManagerData.CurrentMission != _missionManagerData.SurvivalMission;
		}
		return false;
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			Debug.LogWarning("There was a second try to initiate MissionManager");
		}
	}

	public void UnlockMedal(Goal goal)
	{
		_missionManagerData.CurrentMission.UnlockMedal(goal);
		_medalStats = _missionManagerData.GetMedalsStats();
	}

	public void SetActualMission(Mission mission)
	{
		_missionManagerData.CurrentMission = mission;
		_scheduler.SurvivalConfigureEvent = mission.ConfigureEvent;
		_scheduler.InitialEvent = mission.InitialEvent;
		_scheduler.StartDecks = mission.StartDecks;
		if (mission.SetInitialDay)
		{
			_scheduler.SkipDays(mission.InitialDay - 1);
		}
		mission.CheckMissionMedalStatus();
	}

	public int GetNumberOfMedal(Medal medal)
	{
		if (!GetMedalsStats().ContainsKey(medal.name))
		{
			return 0;
		}
		return _medalStats[medal.name];
	}

	public int GetTotalMedalCount(Medal medal)
	{
		if (_allMedalsCount == null)
		{
			_allMedalsCount = _missionManagerData.GetTotalMedalsCount();
		}
		if (!_allMedalsCount.ContainsKey(medal.name))
		{
			return 0;
		}
		return _allMedalsCount[medal.name];
	}

	public Dictionary<string, int> GetMedalsStats()
	{
		if (_medalStats == null)
		{
			_medalStats = _missionManagerData.GetMedalsStats();
		}
		return _medalStats;
	}

	public List<Mission> GetAllMissions()
	{
		if (_allMissions == null)
		{
			_allMissions = _missionManagerData.GetAllMissions();
		}
		return _allMissions;
	}

	public Mission GetCurrentMission()
	{
		return _missionManagerData.CurrentMission;
	}

	public int GetMissionWithAnyMedalsCount()
	{
		GetAllMissions();
		int num = 0;
		for (int i = 0; i < _allMissions.Count; i++)
		{
			List<GoalMedal> medals = _allMissions[i].RuntimeData.Medals;
			for (int j = 0; j < medals.Count; j++)
			{
				if (medals[j].IsAchieved)
				{
					num++;
					break;
				}
			}
		}
		return num;
	}

	public int GetMissionWithAllMedalsCount()
	{
		GetAllMissions();
		int num = 0;
		for (int i = 0; i < _allMissions.Count; i++)
		{
			bool flag = true;
			List<GoalMedal> medals = _allMissions[i].RuntimeData.Medals;
			for (int j = 0; j < medals.Count; j++)
			{
				if (!medals[j].IsAchieved)
				{
					flag = false;
				}
			}
			if (flag)
			{
				num++;
			}
		}
		return num;
	}
}
