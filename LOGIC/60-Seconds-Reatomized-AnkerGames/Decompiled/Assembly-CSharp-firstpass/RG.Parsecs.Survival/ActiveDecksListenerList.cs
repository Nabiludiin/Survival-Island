using System.Collections.Generic;
using RG.Core.Base;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Listener Lists/Active Decks Listener List", fileName = "New Active Decks Listener List")]
public class ActiveDecksListenerList : RGScriptableObject, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	[SerializeField]
	private List<Deck> _activeDecks;

	private const string SAVE_ID = "special_deck_listeners";

	[SerializeField]
	private EventTagList _eventTagList;

	public List<Deck> ActiveDecks => _activeDecks;

	public bool HasBeenLoadedFromSave { get; set; }

	public string ID => Guid;

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		_activeDecks = new List<Deck>();
		Unregister();
	}

	public void RegisterDeckInListenerList(Deck deck)
	{
		if (!_activeDecks.Contains(deck))
		{
			if (_eventTagList.EventTags.Count > 0)
			{
				deck.Recalculate(_eventTagList.EventTags);
			}
			_activeDecks.Add(deck);
		}
	}

	public void UnregisterDeckListenerList(Deck deck)
	{
		_activeDecks.Remove(deck);
	}

	public void UpdateEventInDecks(SurvivalEvent survivalEvent)
	{
		for (int i = 0; i < _activeDecks.Count; i++)
		{
			_activeDecks[i].UpdateEventInArraysBasingOnItsState(survivalEvent);
		}
	}

	public string Serialize()
	{
		StringListWrapper stringListWrapper = new StringListWrapper
		{
			List = new List<string>()
		};
		for (int i = 0; i < _activeDecks.Count; i++)
		{
			stringListWrapper.List.Add(_activeDecks[i].ID);
		}
		return JsonUtility.ToJson(stringListWrapper);
	}

	public void Deserialize(string jsonData)
	{
		StringListWrapper stringListWrapper = JsonUtility.FromJson<StringListWrapper>(jsonData);
		for (int i = 0; i < stringListWrapper.List.Count; i++)
		{
			_activeDecks.Add((Deck)_saveEvent.GetReferenceObjectByID(stringListWrapper.List[i]));
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
		_activeDecks = new List<Deck>();
	}
}
