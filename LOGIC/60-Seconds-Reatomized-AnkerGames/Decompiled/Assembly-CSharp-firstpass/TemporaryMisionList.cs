using System.Collections.Generic;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using RG.Parsecs.Survival;
using UnityEngine;
using UnityEngine.UI;

public class TemporaryMisionList : MonoBehaviour
{
	[SerializeField]
	private MissionList _missionList;

	[SerializeField]
	private GameObject _buttonPrefab;

	[SerializeField]
	private GameObject _container;

	[SerializeField]
	private SaveEvent _gameData;

	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private GlobalBoolVariable _onlySurvival;

	private void Awake()
	{
		Dictionary<string, int> medalsStats = MissionManager.Instance.GetMedalsStats();
		for (int i = 0; i < _missionList.Missions.Count; i++)
		{
			GameObject gameObject = Object.Instantiate(_buttonPrefab);
			gameObject.transform.SetParent(_container.transform);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			Mission mission = _missionList.Missions[i];
			gameObject.GetComponent<Button>().enabled = mission.IsUnlocked(medalsStats);
			LoadMission component = gameObject.GetComponent<LoadMission>();
			component.Mission = _missionList.Missions[i];
			component._gameData = _gameData;
			component._characterList = _characterList;
			component._onlySurvival = _onlySurvival;
		}
	}
}
