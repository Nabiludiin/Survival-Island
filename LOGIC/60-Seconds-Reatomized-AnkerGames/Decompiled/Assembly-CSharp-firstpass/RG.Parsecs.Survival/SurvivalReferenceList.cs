using System;
using System.Collections.Generic;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/New object list", fileName = "New Object list")]
public class SurvivalReferenceList : ObjectsReferenceList
{
	[SerializeField]
	private List<SurvivalEvent> _survivalEvents;

	[SerializeField]
	private List<ReportEvent> _reportEvents;

	[SerializeField]
	private List<SystemEvent> _systemEvents;

	[SerializeField]
	private List<ExpeditionEvent> _expeditionEvents;

	[SerializeField]
	private List<ExpeditionDestination> _expeditionDestinations;

	[SerializeField]
	private List<VisualId> _visualIds;

	[SerializeField]
	private List<ExpeditionThreat> _expeditionThreats;

	[SerializeField]
	private List<CharacterStatus> _characterStatuses;

	[SerializeField]
	private List<Deck> _decks;

	[SerializeField]
	private List<Character> _characters;

	[SerializeField]
	private List<IItem> _items;

	[SerializeField]
	private List<EventTag> _tags;

	[SerializeField]
	private List<Goal> _goals;

	[SerializeField]
	private List<Resource> _resources;

	[SerializeField]
	private List<ShipSystem> _shipSystems;

	[SerializeField]
	private List<SystemStatusEvent> _systemStatusEvents;

	[SerializeField]
	private List<Mission> _missions;

	private Dictionary<string, UnityEngine.Object> _objects;

	public List<SystemStatusEvent> SystemStatusEvents
	{
		get
		{
			return _systemStatusEvents;
		}
		set
		{
			_systemStatusEvents = value;
		}
	}

	public List<SurvivalEvent> SurvivalEvents
	{
		get
		{
			return _survivalEvents;
		}
		set
		{
			_survivalEvents = value;
		}
	}

	public List<ReportEvent> ReportEvents
	{
		get
		{
			return _reportEvents;
		}
		set
		{
			_reportEvents = value;
		}
	}

	public List<SystemEvent> SystemEvents
	{
		get
		{
			return _systemEvents;
		}
		set
		{
			_systemEvents = value;
		}
	}

	public List<ExpeditionEvent> ExpeditionEvents
	{
		get
		{
			return _expeditionEvents;
		}
		set
		{
			_expeditionEvents = value;
		}
	}

	public List<ExpeditionDestination> ExpeditionDestinations
	{
		get
		{
			return _expeditionDestinations;
		}
		set
		{
			_expeditionDestinations = value;
		}
	}

	public List<VisualId> VisualIds
	{
		get
		{
			return _visualIds;
		}
		set
		{
			_visualIds = value;
		}
	}

	public List<ExpeditionThreat> ExpeditionThreats
	{
		get
		{
			return _expeditionThreats;
		}
		set
		{
			_expeditionThreats = value;
		}
	}

	public List<CharacterStatus> CharacterStatuses
	{
		get
		{
			return _characterStatuses;
		}
		set
		{
			_characterStatuses = value;
		}
	}

	public List<Deck> Decks
	{
		get
		{
			return _decks;
		}
		set
		{
			_decks = value;
		}
	}

	public List<Character> Characters
	{
		get
		{
			return _characters;
		}
		set
		{
			_characters = value;
		}
	}

	public List<IItem> Items
	{
		get
		{
			return _items;
		}
		set
		{
			_items = value;
		}
	}

	public List<EventTag> Tags
	{
		get
		{
			return _tags;
		}
		set
		{
			_tags = value;
		}
	}

	public List<Goal> Goals
	{
		get
		{
			return _goals;
		}
		set
		{
			_goals = value;
		}
	}

	public List<Resource> Resources
	{
		get
		{
			return _resources;
		}
		set
		{
			_resources = value;
		}
	}

	public List<ShipSystem> ShipSystems
	{
		get
		{
			return _shipSystems;
		}
		set
		{
			_shipSystems = value;
		}
	}

	public List<Mission> Missions
	{
		get
		{
			return _missions;
		}
		set
		{
			_missions = value;
		}
	}

	public override void Build()
	{
		_objects = new Dictionary<string, UnityEngine.Object>();
		for (int i = 0; i < _visualIds.Count; i++)
		{
			if (_objects.ContainsKey(_visualIds[i].Id))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_visualIds[i].Id].name}, Object which was tried to added: {_visualIds[i].name}");
			}
			else
			{
				_objects.Add(_visualIds[i].Id, _visualIds[i]);
			}
		}
		for (int j = 0; j < _expeditionThreats.Count; j++)
		{
			if (_objects.ContainsKey(_expeditionThreats[j].ID))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_expeditionThreats[j].ID].name}, Object which was tried to added: {_expeditionThreats[j].name}");
			}
			else
			{
				_objects.Add(_expeditionThreats[j].ID, _expeditionThreats[j]);
			}
		}
		for (int k = 0; k < _characterStatuses.Count; k++)
		{
			if (_objects.ContainsKey(_characterStatuses[k].Id))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_characterStatuses[k].Id].name}, Object which was tried to added: {_characterStatuses[k].name}");
			}
			else
			{
				_objects.Add(_characterStatuses[k].Id, _characterStatuses[k]);
			}
		}
		for (int l = 0; l < _decks.Count; l++)
		{
			if (_objects.ContainsKey(_decks[l].ID))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_decks[l].ID].name}, Object which was tried to added: {_decks[l].name}");
			}
			else
			{
				_objects.Add(_decks[l].ID, _decks[l]);
			}
		}
		for (int m = 0; m < _characters.Count; m++)
		{
			if (_objects.ContainsKey(_characters[m].ID))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_characters[m].ID].name}, Object which was tried to added: {_characters[m].name}");
			}
			else
			{
				_objects.Add(_characters[m].ID, _characters[m]);
			}
		}
		for (int n = 0; n < _items.Count; n++)
		{
			if (_objects.ContainsKey(_items[n].BaseStaticData.ItemId))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_items[n].BaseStaticData.ItemId].name}, Object which was tried to added: {_items[n].name}");
			}
			else
			{
				_objects.Add(_items[n].BaseStaticData.ItemId, _items[n]);
			}
		}
		for (int num = 0; num < _expeditionDestinations.Count; num++)
		{
			if (_objects.ContainsKey(_expeditionDestinations[num].ID))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_expeditionDestinations[num].ID].name}, Object which was tried to added: {_expeditionDestinations[num].name}");
			}
			else
			{
				_objects.Add(_expeditionDestinations[num].ID, _expeditionDestinations[num]);
			}
		}
		for (int num2 = 0; num2 < _tags.Count; num2++)
		{
			if (_objects.ContainsKey("etag_" + _tags[num2].name))
			{
				Debug.LogError(string.Format("Duplicate ID of tag when adding to SurvivalReferenceList dictionary. Key: {0}", "etag_" + _tags[num2].name));
			}
			else
			{
				_objects.Add("etag_" + _tags[num2].name, _tags[num2]);
			}
		}
		for (int num3 = 0; num3 < _survivalEvents.Count; num3++)
		{
			if (_objects.ContainsKey(_survivalEvents[num3].ID))
			{
				Debug.LogError(_survivalEvents[num3].name);
			}
			else
			{
				_objects.Add(_survivalEvents[num3].ID, _survivalEvents[num3]);
			}
		}
		for (int num4 = 0; num4 < _expeditionEvents.Count; num4++)
		{
			if (_objects.ContainsKey(_expeditionEvents[num4].ID))
			{
				Debug.LogError(_expeditionEvents[num4].name);
			}
			else
			{
				_objects.Add(_expeditionEvents[num4].ID, _expeditionEvents[num4]);
			}
		}
		for (int num5 = 0; num5 < _reportEvents.Count; num5++)
		{
			if (_objects.ContainsKey(_reportEvents[num5].ID))
			{
				Debug.LogError(_reportEvents[num5].name);
			}
			else
			{
				_objects.Add(_reportEvents[num5].ID, _reportEvents[num5]);
			}
		}
		for (int num6 = 0; num6 < _systemEvents.Count; num6++)
		{
			if (_objects.ContainsKey(_systemEvents[num6].ID))
			{
				Debug.LogError(_systemEvents[num6].name);
			}
			else
			{
				_objects.Add(_systemEvents[num6].ID, _systemEvents[num6]);
			}
		}
		for (int num7 = 0; num7 < _goals.Count; num7++)
		{
			if (_objects.ContainsKey(_goals[num7].ID))
			{
				Debug.LogError(_goals[num7].name);
			}
			else
			{
				_objects.Add(_goals[num7].ID, _goals[num7]);
			}
		}
		for (int num8 = 0; num8 < _resources.Count; num8++)
		{
			if (_objects.ContainsKey(_resources[num8].ID))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_resources[num8].ID].name}, Object which was tried to added: {_resources[num8].name}");
			}
			else
			{
				_objects.Add(_resources[num8].ID, _resources[num8]);
			}
		}
		for (int num9 = 0; num9 < _shipSystems.Count; num9++)
		{
			if (_objects.ContainsKey(_shipSystems[num9].Id))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_shipSystems[num9].Id].name}, Object which was tried to added: {_shipSystems[num9].name}");
			}
			else
			{
				_objects.Add(_shipSystems[num9].Id, _shipSystems[num9]);
			}
		}
		for (int num10 = 0; num10 < _systemStatusEvents.Count; num10++)
		{
			if (_objects.ContainsKey(_systemStatusEvents[num10].ID))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_systemStatusEvents[num10].ID].name}, Object which was tried to added: {_systemStatusEvents[num10].name}");
			}
			else
			{
				_objects.Add(_systemStatusEvents[num10].ID, _systemStatusEvents[num10]);
			}
		}
		for (int num11 = 0; num11 < _missions.Count; num11++)
		{
			if (_objects.ContainsKey(_missions[num11].ID))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_missions[num11].ID].name}, Object which was tried to added: {_missions[num11].name}");
			}
			else
			{
				_objects.Add(_missions[num11].ID, _missions[num11]);
			}
		}
	}

	public override void Destroy()
	{
		if (_objects != null)
		{
			_objects.Clear();
		}
	}

	public override UnityEngine.Object GetReferenceObjectByGUID(string id)
	{
		if (_objects.ContainsKey(id))
		{
			return _objects[id];
		}
		throw new Exception($"There is no {id} in dictionary");
	}
}
