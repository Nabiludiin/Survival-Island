using System.Collections.Generic;
using RG.Core.Base;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Missions/New Mission Manager Data")]
public class MissionManagerData : RGScriptableObject, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	private const string PREFIX = "mmd";

	[SerializeField]
	private List<MissionList> _missionsLists;

	[SerializeField]
	private Sprite _loadingScreen;

	[Tooltip("Field is required for backward compatibility")]
	[SerializeField]
	private Mission _survivalMission;

	[SerializeField]
	private Mission _currentMission;

	private string _id;

	public Sprite LoadingScreen
	{
		get
		{
			return _loadingScreen;
		}
		set
		{
			_loadingScreen = value;
		}
	}

	public Mission CurrentMission
	{
		get
		{
			return _currentMission;
		}
		set
		{
			_currentMission = value;
		}
	}

	public string ID => Guid;

	public Mission SurvivalMission => _survivalMission;

	public override string GuidPrefix => "mmd";

	public Dictionary<string, int> GetMedalsStats()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < _missionsLists.Count; i++)
		{
			for (int j = 0; j < _missionsLists[i].Missions.Count; j++)
			{
				foreach (KeyValuePair<string, int> numberOfAchievedMedal in _missionsLists[i].Missions[j].GetNumberOfAchievedMedals())
				{
					if (dictionary.ContainsKey(numberOfAchievedMedal.Key))
					{
						dictionary[numberOfAchievedMedal.Key] += numberOfAchievedMedal.Value;
					}
					else
					{
						dictionary.Add(numberOfAchievedMedal.Key, numberOfAchievedMedal.Value);
					}
				}
			}
		}
		return dictionary;
	}

	public Dictionary<string, int> GetTotalMedalsCount()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < _missionsLists.Count; i++)
		{
			for (int j = 0; j < _missionsLists[i].Missions.Count; j++)
			{
				Mission mission = _missionsLists[i].Missions[j];
				for (int k = 0; k < mission.Goals.Count; k++)
				{
					if (dictionary.ContainsKey(mission.Goals[k].Medal.name))
					{
						dictionary[mission.Goals[k].Medal.name]++;
					}
					else
					{
						dictionary.Add(mission.Goals[k].Medal.name, 1);
					}
				}
			}
		}
		return dictionary;
	}

	public List<Mission> GetAllMissions()
	{
		List<Mission> list = new List<Mission>();
		for (int i = 0; i < _missionsLists.Count; i++)
		{
			for (int j = 0; j < _missionsLists[i].Missions.Count; j++)
			{
				list.Add(_missionsLists[i].Missions[j]);
			}
		}
		return list;
	}

	private void OnEnable()
	{
		_currentMission = _survivalMission;
		Register();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(new StringWrapper
		{
			Value = ((_currentMission != null) ? _currentMission.ID : string.Empty)
		});
	}

	public void Deserialize(string jsonData)
	{
		StringWrapper stringWrapper = JsonUtility.FromJson<StringWrapper>(jsonData);
		if (!string.IsNullOrEmpty(stringWrapper.Value))
		{
			_currentMission = (Mission)_saveEvent.GetReferenceObjectByID(stringWrapper.Value);
		}
	}

	public void Register()
	{
		_saveEvent.RegisterListener(this);
	}

	public void Unregister()
	{
		_saveEvent.UnregisterListener(this);
	}

	public void ResetData()
	{
		_currentMission = ((_saveEvent.SaveVersion <= 2) ? _survivalMission : null);
	}
}
