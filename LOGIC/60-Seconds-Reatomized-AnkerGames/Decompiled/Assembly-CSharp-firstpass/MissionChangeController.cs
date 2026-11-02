using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;

public class MissionChangeController : MonoBehaviour
{
	[SerializeField]
	private MissionDescriptionPanelController _missionDescriptionPanel;

	[SerializeField]
	private MissionManagerData _missionManagerData;

	private int _currentIndex;

	private List<Mission> _allMissions;

	private void Awake()
	{
		_allMissions = MissionManager.Instance.GetAllMissions();
	}

	private void OnEnable()
	{
		SetCurrentMissionIndex();
	}

	private void SetCurrentMissionIndex()
	{
		Mission currentMission = MissionManager.Instance.GetCurrentMission();
		for (int i = 0; i < _allMissions.Count; i++)
		{
			if (_allMissions[i] == currentMission)
			{
				_currentIndex = i;
				return;
			}
		}
		_currentIndex = 0;
	}

	private void SetCurrentMission()
	{
		_missionManagerData.CurrentMission = _allMissions[_currentIndex];
		_missionDescriptionPanel.SetCurrentMissionDescription();
	}

	public void CycleMissionForth()
	{
		_currentIndex++;
		if (_currentIndex >= _allMissions.Count)
		{
			_currentIndex = 0;
		}
		SetCurrentMission();
	}

	public void CycleMissionBack()
	{
		_currentIndex--;
		if (_currentIndex < 0)
		{
			_currentIndex = _allMissions.Count - 1;
		}
		SetCurrentMission();
	}
}
