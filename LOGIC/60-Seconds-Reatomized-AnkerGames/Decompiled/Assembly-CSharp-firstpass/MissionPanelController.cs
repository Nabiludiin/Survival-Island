using System.Collections.Generic;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;

public class MissionPanelController : MonoBehaviour
{
	[SerializeField]
	private MissionSlotController[] _missionSlotControllers;

	[SerializeField]
	private TextMeshProUGUI _missionTitle;

	[SerializeField]
	private MissionManagerData _missionManagerData;

	private int _currentIndex;

	private List<Mission> _allMissions;

	private const int CENTER_INDEX = 2;

	private int Modulo(int a, int b)
	{
		if (a < 0)
		{
			return b + a % b;
		}
		return a % b;
	}

	public void SetMissionOnCenter(MissionSlotController missionSlot)
	{
		for (int i = 0; i < _allMissions.Count; i++)
		{
			if (missionSlot.Mission == _allMissions[i])
			{
				_currentIndex = Modulo(i - 2, _allMissions.Count);
				UpdateMissionDisplay();
				break;
			}
		}
	}

	public void SetCurrentMission()
	{
		_missionManagerData.CurrentMission = _allMissions[GetCycledIndexOfNumber(2)];
	}

	private void OnEnable()
	{
		if (_allMissions == null)
		{
			_allMissions = MissionManager.Instance.GetAllMissions();
		}
		if (_missionManagerData.CurrentMission == null || _missionManagerData.CurrentMission == _missionManagerData.SurvivalMission)
		{
			SetIndexOnFirstActiveMission();
		}
		else
		{
			SetIndexOnActiveMission();
		}
		UpdateMissionDisplay();
	}

	public void SetIndexOnFirstActiveMission()
	{
		for (int i = 0; i < _allMissions.Count; i++)
		{
			if (_allMissions[i].IsUnlocked(MissionManager.Instance.GetMedalsStats()))
			{
				_currentIndex = Modulo(i - 2, _allMissions.Count);
				return;
			}
		}
		_currentIndex = 0;
	}

	public void SetIndexOnActiveMission()
	{
		Mission currentMission = MissionManager.Instance.GetCurrentMission();
		for (int i = 0; i < _allMissions.Count; i++)
		{
			if (_allMissions[i] == currentMission)
			{
				_currentIndex = Modulo(i - 2, _allMissions.Count);
				return;
			}
		}
		_currentIndex = 0;
	}

	public int GetCycledIndexOfNumber(int number)
	{
		return (_currentIndex + number) % _allMissions.Count;
	}

	public void CycleMissionForth()
	{
		_currentIndex++;
		if (_currentIndex >= _allMissions.Count)
		{
			_currentIndex = 0;
		}
		UpdateMissionDisplay();
	}

	public void CycleMissionBack()
	{
		_currentIndex--;
		if (_currentIndex < 0)
		{
			_currentIndex = _allMissions.Count - 1;
		}
		UpdateMissionDisplay();
	}

	public void UpdateMissionDisplay()
	{
		_missionSlotControllers[0].SetMissionData(_allMissions[GetCycledIndexOfNumber(0)]);
		_missionSlotControllers[1].SetMissionData(_allMissions[GetCycledIndexOfNumber(1)]);
		_missionSlotControllers[2].SetMissionData(_allMissions[GetCycledIndexOfNumber(2)]);
		_missionSlotControllers[3].SetMissionData(_allMissions[GetCycledIndexOfNumber(3)]);
		_missionSlotControllers[4].SetMissionData(_allMissions[GetCycledIndexOfNumber(4)]);
		_missionTitle.text = _allMissions[GetCycledIndexOfNumber(2)].Name;
	}
}
