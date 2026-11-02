using System;
using System.Collections.Generic;
using RG.Core.Base;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(fileName = "New Deck", menuName = "60 Parsecs!/Event Editor/New Deck")]
public class Deck : RGScriptableObject, ISaveable
{
	private const string DECK_GUID_PREFIX = "deck";

	private const float MIN_EVENT_PROBABILITY = 0.001f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _deckProbability;

	[SerializeField]
	private SurvivalEventDeckArrayElement[] _survivalEvents;

	[NonSerialized]
	private List<SurvivalEventDeckArrayElement> _activeEvents = new List<SurvivalEventDeckArrayElement>();

	[NonSerialized]
	private List<SurvivalEventDeckArrayElement> _discardedEvents = new List<SurvivalEventDeckArrayElement>();

	[NonSerialized]
	private List<SurvivalEventDeckArrayElement> _junkedEvents = new List<SurvivalEventDeckArrayElement>();

	[NonSerialized]
	private List<SurvivalEventDeckArrayElement> _notToDrawnEvents = new List<SurvivalEventDeckArrayElement>();

	[SerializeField]
	private SaveEvent _saveEvent;

	public DeckState CurrentState { get; protected set; }

	public bool IsActive => CurrentState == DeckState.ACTIVATED;

	public float DeckProbability => _deckProbability;

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

	public string ID => Guid;

	public override string GuidPrefix => "deck";

	public void Recalculate(List<EventTag> tags)
	{
		AssignEventsToArraysBasingOrTheyState();
		for (int num = _activeEvents.Count - 1; num >= 0; num--)
		{
			for (int i = 0; i < tags.Count; i++)
			{
				if (_activeEvents[num].SurvivalEvent.Tags.Contains(tags[i]))
				{
					_notToDrawnEvents.Add(_activeEvents[num]);
					_activeEvents.RemoveAt(num);
					break;
				}
			}
		}
		for (int num2 = _discardedEvents.Count - 1; num2 >= 0; num2--)
		{
			for (int j = 0; j < tags.Count; j++)
			{
				if (_discardedEvents[num2].SurvivalEvent.Tags.Contains(tags[j]))
				{
					_notToDrawnEvents.Add(_discardedEvents[num2]);
					_discardedEvents.RemoveAt(num2);
					break;
				}
			}
		}
	}

	public bool HasActiveEvents()
	{
		return _activeEvents.Count > 0;
	}

	public bool HasDiscardedEvents()
	{
		return _discardedEvents.Count > 0;
	}

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	public void InitializeDeck()
	{
		AssignEventsToArraysBasingOrTheyState();
	}

	public void AssignEventsToArraysBasingOrTheyState()
	{
		_activeEvents.Clear();
		_discardedEvents.Clear();
		_junkedEvents.Clear();
		_notToDrawnEvents.Clear();
		for (int i = 0; i < _survivalEvents.Length; i++)
		{
			if (_survivalEvents[i].SurvivalEvent == null)
			{
				Debug.LogError($"There is \"None\" event in {name} deck that was being activated.");
			}
			else if (_survivalEvents[i].SurvivalEvent.CanBeDrawn())
			{
				switch (_survivalEvents[i].SurvivalEvent.CurrentState)
				{
				case EEventState.ACTIVE:
					_activeEvents.Add(_survivalEvents[i]);
					break;
				case EEventState.DISCARDED:
					_discardedEvents.Add(_survivalEvents[i]);
					break;
				case EEventState.JUNKED:
					_junkedEvents.Add(_survivalEvents[i]);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				_notToDrawnEvents.Add(_survivalEvents[i]);
			}
		}
	}

	public void UpdateEventInArraysBasingOnItsState(SurvivalEvent survivalEvent)
	{
		SurvivalEventDeckArrayElement survivalEventDeckArrayElement = FindDeckEvent(survivalEvent);
		if (survivalEventDeckArrayElement == null)
		{
			return;
		}
		if (_activeEvents.Contains(survivalEventDeckArrayElement) && survivalEventDeckArrayElement.SurvivalEvent.CurrentState != EEventState.ACTIVE)
		{
			_activeEvents.Remove(survivalEventDeckArrayElement);
		}
		if (_discardedEvents.Contains(survivalEventDeckArrayElement) && survivalEventDeckArrayElement.SurvivalEvent.CurrentState != EEventState.DISCARDED)
		{
			_discardedEvents.Remove(survivalEventDeckArrayElement);
		}
		if (_junkedEvents.Contains(survivalEventDeckArrayElement) && survivalEventDeckArrayElement.SurvivalEvent.CurrentState != EEventState.JUNKED)
		{
			_junkedEvents.Remove(survivalEventDeckArrayElement);
		}
		switch (survivalEvent.CurrentState)
		{
		case EEventState.ACTIVE:
			if (!_activeEvents.Contains(survivalEventDeckArrayElement))
			{
				_activeEvents.Add(survivalEventDeckArrayElement);
			}
			break;
		case EEventState.DISCARDED:
			if (!_discardedEvents.Contains(survivalEventDeckArrayElement))
			{
				_discardedEvents.Add(survivalEventDeckArrayElement);
			}
			break;
		case EEventState.JUNKED:
			if (!_junkedEvents.Contains(survivalEventDeckArrayElement))
			{
				_junkedEvents.Add(survivalEventDeckArrayElement);
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public SurvivalEventDeckArrayElement FindDeckEvent(SurvivalEvent survivalEvent)
	{
		for (int i = 0; i < _survivalEvents.Length; i++)
		{
			if (_survivalEvents[i].SurvivalEvent == survivalEvent)
			{
				return _survivalEvents[i];
			}
		}
		return null;
	}

	public SurvivalEvent GetRandomEvent()
	{
		List<SurvivalEventDeckArrayElement> list = ((CountAvailableEvents(_activeEvents) > 0) ? _activeEvents : _discardedEvents);
		if (list.Count == 0)
		{
			return null;
		}
		float num = SumEventsProbability(list);
		if (num <= 0f)
		{
			return GetRandomEventBasedOnRandomIndex(list);
		}
		return GetRandomEventBasedOnProbability(list, num);
	}

	private int CountAvailableEvents(List<SurvivalEventDeckArrayElement> events)
	{
		int num = 0;
		for (int i = 0; i < events.Count; i++)
		{
			if (events[i].Probability >= 0.001f)
			{
				num++;
			}
		}
		return num;
	}

	private SurvivalEvent GetRandomEventBasedOnRandomIndex(List<SurvivalEventDeckArrayElement> events)
	{
		int index = UnityEngine.Random.Range(0, events.Count);
		return events[index].SurvivalEvent;
	}

	private SurvivalEvent GetRandomEventBasedOnProbability(List<SurvivalEventDeckArrayElement> events, float probabilitySum)
	{
		float num = UnityEngine.Random.Range(0f, probabilitySum);
		float num2 = 0f;
		for (int i = 0; i < events.Count; i++)
		{
			if (!(events[i].Probability < 0.001f))
			{
				if (num >= num2 && num < num2 + events[i].Probability)
				{
					return events[i].SurvivalEvent;
				}
				num2 += events[i].Probability;
			}
		}
		return null;
	}

	private float SumEventsProbability(List<SurvivalEventDeckArrayElement> events)
	{
		float num = 0f;
		for (int i = 0; i < events.Count; i++)
		{
			if (!(events[i].Probability < 0.001f))
			{
				num += events[i].Probability;
			}
		}
		return num;
	}

	public virtual void SetActive(bool activate)
	{
		if (activate && CurrentState != DeckState.ACTIVATED)
		{
			CurrentState = DeckState.ACTIVATED;
			EventManager.RegisterDeckInListenerList(this);
			InitializeDeck();
		}
		else if (!activate && CurrentState != DeckState.DEACTIVATED)
		{
			CurrentState = DeckState.DEACTIVATED;
			EventManager.UnregisterDeckFromListenerList(this);
		}
	}

	public virtual string Serialize()
	{
		return JsonUtility.ToJson(new DeckCurrentStateWrapper
		{
			CurrentState = CurrentState
		});
	}

	public virtual void Deserialize(string jsonData)
	{
		CurrentState = JsonUtility.FromJson<DeckCurrentStateWrapper>(jsonData).CurrentState;
	}

	public void Register()
	{
		if (_saveEvent != null)
		{
			_saveEvent.RegisterListener(this);
		}
		else
		{
			Debug.Log($"Event {name} has not set SaveEvent");
		}
	}

	public void Unregister()
	{
		_saveEvent.UnregisterListener(this);
	}

	public void ResetData()
	{
		CurrentState = DeckState.DEACTIVATED;
		_activeEvents.Clear();
		_discardedEvents.Clear();
		_junkedEvents.Clear();
	}
}
