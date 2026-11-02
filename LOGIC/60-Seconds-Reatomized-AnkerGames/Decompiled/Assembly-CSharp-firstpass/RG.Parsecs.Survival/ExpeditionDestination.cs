using System;
using System.Collections.Generic;
using I2.Loc;
using RG.Core.Base;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(fileName = "New Expedition Destination", menuName = "60 Parsecs!/Event Editor/Planets/New Expedition Destination")]
public class ExpeditionDestination : RGScriptableObject, ISaveable
{
	[SerializeField]
	private ExpeditionDestinationStaticData _staticData = new ExpeditionDestinationStaticData();

	[SerializeField]
	private ExpeditionDestinationDynamicData _dynamicData = new ExpeditionDestinationDynamicData();

	[SerializeField]
	private SaveEvent _saveEvent;

	public SaveEvent SaveEvent
	{
		get
		{
			return _saveEvent;
		}
		set
		{
			_saveEvent = value;
		}
	}

	public ExpeditionDestinationStaticData StaticData => _staticData;

	public LocalizedString Name => _staticData.Name;

	public LocalizedString Description => _staticData.Description;

	public bool Enabled => _dynamicData.Enabled;

	public ExpeditionEvent Event => _staticData.Event;

	public int RequiredSystemLevel => _staticData.RequiredSystemLevel;

	public Sprite DestinationSprite => _staticData.DestinationSprite;

	public Resource[] ResourcesToFind => _staticData.ResourcesToFind;

	public List<ExpeditionThreat> Threats
	{
		get
		{
			return _dynamicData.Threats;
		}
		set
		{
			_dynamicData.Threats = value;
		}
	}

	public string ID => Guid;

	public ExpeditionDestinationDynamicData DynamicData => _dynamicData;

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	public void Initialize()
	{
		_dynamicData.Enabled = _staticData.Enabled;
		_dynamicData.Threats = new List<ExpeditionThreat>();
		_dynamicData.Items = new List<IItem>();
		_dynamicData.Visited = _staticData.Visited;
		for (int i = 0; i < _staticData.Threats.Length; i++)
		{
			_dynamicData.Threats.Add(_staticData.Threats[i]);
		}
		for (int j = 0; j < _staticData.ItemsToFind.Length; j++)
		{
			_dynamicData.Items.Add(_staticData.ItemsToFind[j]);
		}
	}

	public void SetEnabled(bool enabled)
	{
		_dynamicData.Enabled = enabled;
	}

	public string Serialize()
	{
		ExpeditionDestinationWrapper expeditionDestinationWrapper = new ExpeditionDestinationWrapper
		{
			Enabled = _dynamicData.Enabled,
			Visited = _dynamicData.Visited,
			Threats = new List<string>(),
			Items = new List<string>()
		};
		for (int i = 0; i < _dynamicData.Threats.Count; i++)
		{
			expeditionDestinationWrapper.Threats.Add(_dynamicData.Threats[i].ID);
		}
		for (int j = 0; j < _dynamicData.Items.Count; j++)
		{
			expeditionDestinationWrapper.Items.Add(_dynamicData.Items[j].BaseStaticData.ItemId);
		}
		return JsonUtility.ToJson(expeditionDestinationWrapper);
	}

	public void Deserialize(string jsonData)
	{
		ExpeditionDestinationWrapper expeditionDestinationWrapper = JsonUtility.FromJson<ExpeditionDestinationWrapper>(jsonData);
		_dynamicData.Enabled = expeditionDestinationWrapper.Enabled;
		_dynamicData.Visited = expeditionDestinationWrapper.Visited;
		_dynamicData.Threats.Clear();
		for (int i = 0; i < expeditionDestinationWrapper.Threats.Count; i++)
		{
			_dynamicData.Threats.Add((ExpeditionThreat)_saveEvent.GetReferenceObjectByID(expeditionDestinationWrapper.Threats[i]));
		}
		if (_saveEvent.SaveVersion > 1)
		{
			_dynamicData.Items.Clear();
			for (int j = 0; j < expeditionDestinationWrapper.Items.Count; j++)
			{
				_dynamicData.Items.Add((IItem)_saveEvent.GetReferenceObjectByID(expeditionDestinationWrapper.Items[j]));
			}
		}
		JsonUtility.FromJsonOverwrite(jsonData, _dynamicData);
	}

	public void Register()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.RegisterListener(this);
		}
		else
		{
			Debug.Log($"Expedition destination {ID} has not set SaveEvent");
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
		Initialize();
	}
}
