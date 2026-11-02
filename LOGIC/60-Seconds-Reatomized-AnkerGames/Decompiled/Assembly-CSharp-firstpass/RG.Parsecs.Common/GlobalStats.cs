using System.Collections.Generic;
using RG.Core.Base;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Common;

[CreateAssetMenu(menuName = "60 Parsecs!/Goal Editor/New Tracks Global Data")]
public class GlobalStats : RGScriptableObject, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	private Dictionary<string, int> _globalsData;

	private Dictionary<string, List<string>> _globalDataLists;

	[SerializeField]
	private GlobalStatsList _globalStats;

	private const string STAT_GLOBALY_PREFIX = "globaltStats";

	public string ID => Guid;

	public override string GuidPrefix => "globaltStats";

	public int GetData(string id)
	{
		if (_globalsData.ContainsKey(id))
		{
			return _globalsData[id];
		}
		if (_globalDataLists.ContainsKey(id))
		{
			return _globalDataLists[id].Count;
		}
		Debug.LogWarning($"There is no key {id} in global statistics");
		return 0;
	}

	public void AddData(string id, int data)
	{
		if (_globalsData.ContainsKey(id))
		{
			_globalsData[id] += data;
		}
	}

	public void AddDataToList(string id, string data)
	{
		if (_globalDataLists.ContainsKey(id) && !_globalDataLists[id].Contains(data))
		{
			_globalDataLists[id].Add(data);
		}
	}

	private void InitDictionary()
	{
		_globalsData = new Dictionary<string, int>();
		_globalDataLists = new Dictionary<string, List<string>>();
		if (_globalStats != null)
		{
			for (int i = 0; i < _globalStats.Variables.Count; i++)
			{
				_globalsData.Add(_globalStats.Variables[i], 0);
			}
			for (int j = 0; j < _globalStats.List.Count; j++)
			{
				_globalDataLists.Add(_globalStats.List[j], new List<string>());
			}
		}
	}

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		_globalsData.Clear();
		_globalDataLists.Clear();
		Unregister();
	}

	public string Serialize()
	{
		GlobalStatsWrapper globalStatsWrapper = new GlobalStatsWrapper
		{
			Data = new List<StatsWrapper>()
		};
		foreach (KeyValuePair<string, int> globalsDatum in _globalsData)
		{
			StatsWrapper item = new StatsWrapper
			{
				Key = globalsDatum.Key,
				Value = globalsDatum.Value,
				IsList = false
			};
			globalStatsWrapper.Data.Add(item);
		}
		foreach (KeyValuePair<string, List<string>> globalDataList in _globalDataLists)
		{
			StatsWrapper item2 = new StatsWrapper
			{
				Key = globalDataList.Key,
				List = globalDataList.Value,
				IsList = true
			};
			globalStatsWrapper.Data.Add(item2);
		}
		return JsonUtility.ToJson(globalStatsWrapper);
	}

	public void Deserialize(string jsonData)
	{
		_globalsData.Clear();
		_globalDataLists.Clear();
		InitDictionary();
		foreach (StatsWrapper datum in JsonUtility.FromJson<GlobalStatsWrapper>(jsonData).Data)
		{
			if (datum.IsList)
			{
				_globalDataLists[datum.Key] = datum.List;
			}
			else
			{
				_globalsData[datum.Key] = datum.Value;
			}
		}
	}

	public void Register()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.RegisterListener(this);
		}
		else
		{
			Debug.Log($"Globaly Stats {name} has not set SaveEvent");
		}
	}

	public void Unregister()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.UnregisterListener(this);
		}
	}

	public void ResetData()
	{
		InitDictionary();
	}
}
