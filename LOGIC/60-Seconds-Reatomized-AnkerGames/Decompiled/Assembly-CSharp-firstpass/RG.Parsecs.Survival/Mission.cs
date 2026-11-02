using System.Collections.Generic;
using I2.Loc;
using RG.Core.Base;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.Loading;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Missions/New Mission")]
public class Mission : RGScriptableObject, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	[SerializeField]
	private LocalizedString _name;

	[SerializeField]
	private LocalizedString _description;

	[SerializeField]
	private Sprite _image;

	[SerializeField]
	private List<MedalReward> _goals;

	[Tooltip("Goals for which player doesn't get medals, e.g. achievements")]
	[SerializeField]
	private List<Goal> _additionalGoals;

	[SerializeField]
	private List<MedalRequirement> _requirements;

	[SerializeField]
	private ReportEvent _configureEvent;

	[SerializeField]
	private ReportEvent _initialEvent;

	[SerializeField]
	private bool _setCaptainExternal;

	[SerializeField]
	private Character _captain;

	[SerializeField]
	private DeckList _startDecks;

	[SerializeField]
	private bool _setInitialDay;

	[SerializeField]
	private int _initialDay = 1;

	[SerializeField]
	private MissionRuntimeData _runtimeData;

	[Tooltip("If left null/none, the game will display a regular poster at random.")]
	[SerializeField]
	private Poster _poster;

	[SerializeField]
	private EndGameStatSet _statSet;

	private const string MISSION_PREFIX = "ms";

	public EndGameStatSet StatSet => _statSet;

	public LocalizedString Name => _name;

	public LocalizedString Description => _description;

	public Sprite Image => _image;

	public List<MedalReward> Goals => _goals;

	public List<MedalRequirement> Requirements => _requirements;

	public ReportEvent ConfigureEvent => _configureEvent;

	public ReportEvent InitialEvent => _initialEvent;

	public Character Captain => _captain;

	public DeckList StartDecks => _startDecks;

	public MissionRuntimeData RuntimeData => _runtimeData;

	public List<Goal> AdditionalGoals => _additionalGoals;

	public Poster Poster => _poster;

	public bool SetCaptainExternal => _setCaptainExternal;

	public bool SetInitialDay => _setInitialDay;

	public int InitialDay => _initialDay;

	public string ID => Guid;

	public override string GuidPrefix => "ms";

	public void UnlockMedal(Goal goal)
	{
		for (int i = 0; i < _runtimeData.Medals.Count; i++)
		{
			if (_runtimeData.Medals[i].GoalName.Equals(goal.name) && !_runtimeData.Medals[i].IsAchieved)
			{
				_runtimeData.Medals[i].IsAchieved = true;
			}
		}
	}

	public Dictionary<string, int> GetNumberOfAchievedMedals()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < _runtimeData.Medals.Count; i++)
		{
			if (_runtimeData.Medals[i].IsAchieved)
			{
				if (dictionary.ContainsKey(_goals[i].Medal.name))
				{
					dictionary[_goals[i].Medal.name]++;
				}
				else
				{
					dictionary.Add(_goals[i].Medal.name, 1);
				}
			}
		}
		for (int j = 0; j < _goals.Count; j++)
		{
			if (!(_goals[j].Medal == null) && !dictionary.ContainsKey(_goals[j].Medal.name))
			{
				dictionary.Add(_goals[j].Medal.name, 0);
			}
		}
		for (int k = 0; k < _requirements.Count; k++)
		{
			if (!(_requirements[k].Medal == null) && !dictionary.ContainsKey(_requirements[k].Medal.name))
			{
				dictionary.Add(_requirements[k].Medal.name, 0);
			}
		}
		return dictionary;
	}

	public bool IsUnlocked(Dictionary<string, int> allMedals)
	{
		for (int i = 0; i < _requirements.Count; i++)
		{
			if (allMedals[_requirements[i].Medal.name] < _requirements[i].Number)
			{
				return false;
			}
		}
		return true;
	}

	public void CheckMissionMedalStatus()
	{
		for (int i = 0; i < _runtimeData.Medals.Count; i++)
		{
			if (_runtimeData.Medals[i].IsAchieved && !_runtimeData.Medals[i].PreviouslyAchieved)
			{
				_runtimeData.Medals[i].PreviouslyAchieved = true;
			}
		}
	}

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(_runtimeData);
	}

	public void Deserialize(string jsonData)
	{
		MissionRuntimeData missionRuntimeData = JsonUtility.FromJson<MissionRuntimeData>(jsonData);
		if (missionRuntimeData.Medals.Count > 0)
		{
			_runtimeData = missionRuntimeData;
		}
	}

	public void Register()
	{
		_saveEvent.RegisterListener(this);
	}

	public void Unregister()
	{
		if (_saveEvent != null)
		{
			_saveEvent.UnregisterListener(this);
		}
	}

	public void ResetData()
	{
		_runtimeData = new MissionRuntimeData
		{
			Medals = new List<GoalMedal>()
		};
		for (int i = 0; i < _goals.Count; i++)
		{
			_runtimeData.Medals.Add(new GoalMedal
			{
				IsAchieved = false,
				GoalName = _goals[i].Goal.name
			});
		}
	}
}
