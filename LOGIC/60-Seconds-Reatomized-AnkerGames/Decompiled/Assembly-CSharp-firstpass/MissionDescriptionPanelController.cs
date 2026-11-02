using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using I2.Loc;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionDescriptionPanelController : MonoBehaviour
{
	[SerializeField]
	private MissionSlotController _missionSlot;

	[SerializeField]
	private TextMeshProUGUI _missionTitle;

	[SerializeField]
	private TextMeshProUGUI _missionDescription;

	[SerializeField]
	private Transform _missionGoalsHolder;

	[SerializeField]
	private GameObject _missionGoalPrefab;

	[SerializeField]
	private Button _startButton;

	[SerializeField]
	public SaveEvent _survivalSaveEvent;

	[SerializeField]
	public CharacterList _characterList;

	[SerializeField]
	public GlobalBoolVariable _onlySurvival;

	[SerializeField]
	private LocalizedString _mainObjectiveTerm;

	[SerializeField]
	private LocalizedString _challengeObjectiveTerm;

	[SerializeField]
	private GlobalIntVariable _previousCaptain;

	private List<MissionGoalController> _missionGoals;

	[SerializeField]
	private string _masterBus = "bus:/Master";

	private void OnEnable()
	{
		SetCurrentMissionDescription();
	}

	public void SetCurrentMissionDescription()
	{
		Mission currentMission = MissionManager.Instance.GetCurrentMission();
		_startButton.interactable = currentMission.IsUnlocked(MissionManager.Instance.GetMedalsStats());
		_missionSlot.SetMissionData(currentMission);
		_missionTitle.text = currentMission.Name;
		_missionDescription.text = currentMission.Description;
		if (_missionGoals == null)
		{
			_missionGoals = new List<MissionGoalController>();
		}
		for (int i = 0; i < currentMission.Goals.Count; i++)
		{
			if (i < _missionGoals.Count)
			{
				_missionGoals[i].gameObject.SetActive(value: true);
				_missionGoals[i].SetGoal(currentMission.Goals[i], currentMission.RuntimeData.Medals[i].IsAchieved, _mainObjectiveTerm, _challengeObjectiveTerm);
			}
			else
			{
				_missionGoals.Add(Object.Instantiate(_missionGoalPrefab, _missionGoalsHolder).GetComponent<MissionGoalController>());
				_missionGoals[_missionGoals.Count - 1].SetGoal(currentMission.Goals[i], currentMission.RuntimeData.Medals[i].IsAchieved, _mainObjectiveTerm, _challengeObjectiveTerm);
			}
		}
		if (currentMission.Goals.Count < _missionGoals.Count)
		{
			for (int j = currentMission.Goals.Count; j < _missionGoals.Count; j++)
			{
				_missionGoals[j].gameObject.SetActive(value: false);
			}
		}
	}

	public void StartMission()
	{
		Mission currentMission = MissionManager.Instance.GetCurrentMission();
		StopMusic();
		_survivalSaveEvent.ResetGame();
		MissionManager.Instance.SetActualMission(currentMission);
		if (!currentMission.SetCaptainExternal)
		{
			_characterList.AddCharToList(currentMission.Captain);
			_previousCaptain.Value = _characterList.GetCharacterSet.GetCharacterId(currentMission.Captain);
		}
		_onlySurvival.Value = true;
		Singleton<GameManager>.Instance.StartSurvival();
	}

	private void StopMusic()
	{
		RuntimeManager.GetBus(_masterBus).stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}
}
