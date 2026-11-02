using System;
using System.Collections;
using System.Collections.Generic;
using RG.Core.Base;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(fileName = "New Scheduler", menuName = "60 Parsecs!/Create Scheduler")]
public class Scheduler : RGScriptableObject, ISaveable
{
	private enum ESchedulePhase
	{
		INCREASE_DAY,
		DECREASE_DAY,
		OUT_OF_RANGE
	}

	private const int THROW_EXCEPTION_THRESHOLD = 100;

	private const int EXECUTE_SYSTEM_EVENTS_LISTENER_LIST_PRIORITY = 5;

	[SerializeField]
	private ReportEvent _defaultSurvivalConfigureEvent;

	private ReportEvent _survivalConfigureEvent;

	[SerializeField]
	private ReportEvent _defaultInitialEvent;

	private ReportEvent _initialEvent;

	[SerializeField]
	private List<ScheduledDay> _days = new List<ScheduledDay>();

	[SerializeField]
	private EndOfDayListenerList _endOfDay;

	[SerializeField]
	private ActiveDecksListenerList _activeDecksListener;

	[SerializeField]
	private DeckList _defaultStartDecks;

	[SerializeField]
	private SurvivalEvent _blockingEvent;

	private DeckList _startDecks;

	[SerializeField]
	private SurvivalData _survivalData;

	[SerializeField]
	private int _daysToSkip;

	[SerializeField]
	private ScheduledDay _nextDay;

	[SerializeField]
	private ScheduledDay _currentScheduledDay;

	[SerializeField]
	private SaveEvent _saveEvent;

	private List<Deck> _activeDecks;

	[SerializeField]
	private int _currentDay;

	[SerializeField]
	private EventTagList _forbiddenTags;

	[SerializeField]
	private EndGameData _endGameData;

	[SerializeField]
	private GlobalBoolVariable _shouldDisplaySendExpeditionPage;

	private const string SAVE_SYSTEM_ID = "Scheduler";

	private const int NEXT_DAY = 1;

	[SerializeField]
	private int _nextDayPriority;

	private int _minRescheduleDelay;

	private int _maxRescheduleDelay;

	private bool _forceSchedule;

	private bool _rescheduleEvent;

	public ReportEvent SurvivalConfigureEvent
	{
		get
		{
			return _survivalConfigureEvent;
		}
		set
		{
			_survivalConfigureEvent = value;
		}
	}

	public ReportEvent InitialEvent
	{
		get
		{
			return _initialEvent;
		}
		set
		{
			_initialEvent = value;
		}
	}

	public DeckList StartDecks
	{
		get
		{
			return _startDecks;
		}
		set
		{
			_startDecks = value;
		}
	}

	public string ID => Guid;

	public ScheduledDay CurrentScheduledDay => _currentScheduledDay;

	public IEnumerator CleanState()
	{
		Clean();
		yield return null;
	}

	private void Clean()
	{
		_currentDay = 1;
		_days.Clear();
		_currentScheduledDay = null;
		_nextDay = null;
		_nextDay = new ScheduledDay(1);
		_days.Add(_nextDay);
	}

	public void PrepareFirstDay()
	{
		_currentDay--;
		if (_startDecks != null && _startDecks.Decks != null)
		{
			for (int i = 0; i < _startDecks.Decks.Count; i++)
			{
				_startDecks.Decks[i].SetActive(activate: true);
			}
		}
		else
		{
			Debug.LogError($"No start decks set in scheduler: {name}");
		}
		_activeDecks = _activeDecksListener.ActiveDecks;
		if (!_nextDay.HasSurvivalEvent())
		{
			DrawEvent();
		}
		_currentDay++;
		_currentScheduledDay = _nextDay;
		_nextDay = null;
		SkipDayExecute();
	}

	public void SetNextDay()
	{
		SkipDayExecute();
		PrepareNextDay();
		_currentScheduledDay = _nextDay;
		_nextDay = null;
	}

	private void SkipDayExecute()
	{
		if (_daysToSkip <= 0)
		{
			return;
		}
		int currentDay = _currentDay;
		_currentDay += _daysToSkip;
		for (int i = 0; i < _days.Count; i++)
		{
			if (_days[i].Day >= currentDay)
			{
				_days[i].Day += _daysToSkip;
			}
		}
		_survivalData.CurrentDay += _daysToSkip;
		_survivalData.DaysToSkip = 0;
		_daysToSkip = 0;
	}

	public void SkipDays(int daysToSkip)
	{
		if (daysToSkip < 0)
		{
			Debug.LogError("Error from Scheduler: the number of days to skip cannot be lower than 0!!");
			return;
		}
		_days.Sort();
		for (int i = 0; i < _days.Count; i++)
		{
			if (_days[i].Day >= _currentDay && _days[i].ExpeditionEvent != null && _days[i].Day <= _currentDay + daysToSkip)
			{
				_days[i].ExpeditionEvent.Run();
				_days[i].RemoveEvent(_days[i].ExpeditionEvent);
				break;
			}
		}
		_daysToSkip = daysToSkip;
		_survivalData.DaysToSkip = _daysToSkip;
	}

	private void OnDestroy()
	{
		_endOfDay.UnregisterOnDayStarted(ChangeDayMethod, "PreEventSystems");
		_endOfDay.UnregisterOnDayStarted(FinishDay, "ReportEvents");
		_endOfDay.UnregisterOnDayStarted(ExecuteSystemsEvents, "SystemEvents");
		_endOfDay.UnregisterOnDayStarted(SetNextDay, "PrepareSystem");
		_endOfDay.UnregisterOnDayStarted(StartDay, "ActionEvents");
		Unregister();
	}

	private void OnEnable()
	{
		_endOfDay.RegisterOnEndOfDay(ChangeDayMethod, "PreEventSystems", 20, this);
		_endOfDay.RegisterOnEndOfDay(FinishDay, "ReportEvents", 0, this);
		_endOfDay.RegisterOnEndOfDay(ExecuteSystemsEvents, "SystemEvents", 5, this);
		_endOfDay.RegisterOnEndOfDay(SetNextDay, "PrepareSystem", _nextDayPriority, this);
		_endOfDay.RegisterOnEndOfDay(StartDay, "ActionEvents", 0, this);
		Register();
	}

	public void StartDay()
	{
		if (!_endGameData.RuntimeData.ShouldEndGame)
		{
			_currentScheduledDay.ExecuteNewDay();
		}
	}

	public void ScheduleBlockingEventOnCurrentDay()
	{
		if (_blockingEvent != null)
		{
			ScheduleEvent(_blockingEvent, 0, 0, forceSchedule: true);
		}
	}

	public void ChangeDayMethod()
	{
		ChangeDay();
	}

	public void FinishDay()
	{
		_currentScheduledDay.ExecuteEndDay();
		ClearOldEvents();
	}

	public void ExecuteSystemsEvents()
	{
		_currentScheduledDay.ExecuteSystemsEvents();
	}

	private void ClearOldEvents()
	{
		_days.RemoveAll((ScheduledDay eventDay) => eventDay.Day <= _currentDay - 2);
	}

	private void PrepareNextDay()
	{
		ScheduledDay scheduledDay = _days.Find((ScheduledDay d) => d.Day == _currentDay);
		if (scheduledDay == null)
		{
			_nextDay = new ScheduledDay(_currentDay);
			_days.Add(_nextDay);
		}
		else
		{
			_nextDay = scheduledDay;
		}
		if (_nextDay.HasSurvivalEvent() && ((!_nextDay.SurvivalEvent.CanBeDrawn(isScheduled: true) && !_nextDay.IgnoreCondition) || (_shouldDisplaySendExpeditionPage != null && _shouldDisplaySendExpeditionPage.Value)))
		{
			ScheduleEvent mainEvent = _nextDay.GetMainEvent();
			_nextDay.RemoveEvent(_nextDay.SurvivalEvent);
			if (_rescheduleEvent)
			{
				ScheduleEvent(mainEvent.Event, _minRescheduleDelay, _maxRescheduleDelay, mainEvent.Items, mainEvent.Characters, _forceSchedule);
				_rescheduleEvent = false;
			}
			else if (_shouldDisplaySendExpeditionPage != null && _shouldDisplaySendExpeditionPage.Value)
			{
				int minDelay = ((_minRescheduleDelay == 0) ? 1 : _minRescheduleDelay);
				int maxDelay = ((_maxRescheduleDelay == 0) ? 1 : _maxRescheduleDelay);
				ScheduleEvent(mainEvent.Event, minDelay, maxDelay, mainEvent.Items, mainEvent.Characters, _forceSchedule);
			}
		}
		if (_shouldDisplaySendExpeditionPage != null && _shouldDisplaySendExpeditionPage.Value)
		{
			ScheduleBlockingEventOnCurrentDay();
		}
		if (!_nextDay.HasSurvivalEvent())
		{
			DrawEvent();
		}
	}

	public ScheduledDay GetNextScheduledDay()
	{
		return _days.Find((ScheduledDay d) => d.Day == _currentDay + 1);
	}

	private void ChangeDay()
	{
		_currentDay++;
	}

	private void ShuffleDeckList(ref List<DeckData> data)
	{
		if (data.Count > 2)
		{
			for (int num = data.Count - 1; num > 1; num--)
			{
				int index = UnityEngine.Random.Range(0, num + 1);
				DeckData value = data[num];
				data[num] = data[index];
				data[index] = value;
			}
		}
	}

	private Deck DrawDeck()
	{
		_activeDecks = _activeDecksListener.ActiveDecks;
		List<DeckData> data = new List<DeckData>();
		float num = 0f;
		for (int i = 0; i < _activeDecks.Count; i++)
		{
			if (_activeDecks[i].DeckProbability > 0f)
			{
				_activeDecks[i].AssignEventsToArraysBasingOrTheyState();
				if (_forbiddenTags.IsDirty)
				{
					_activeDecks[i].Recalculate(_forbiddenTags.EventTags);
				}
			}
			if (_activeDecks[i].DeckProbability > 0f && _activeDecks[i].HasActiveEvents())
			{
				num += _activeDecks[i].DeckProbability;
				data.Add(new DeckData
				{
					Deck = _activeDecks[i],
					Probability = _activeDecks[i].DeckProbability
				});
			}
		}
		_forbiddenTags.SetAsClean();
		if (data.Count == 0)
		{
			for (int j = 0; j < _activeDecks.Count; j++)
			{
				if (_activeDecks[j].DeckProbability > 0f && (_activeDecks[j].HasActiveEvents() || _activeDecks[j].HasDiscardedEvents()))
				{
					num += _activeDecks[j].DeckProbability;
					data.Add(new DeckData
					{
						Deck = _activeDecks[j],
						Probability = _activeDecks[j].DeckProbability
					});
				}
			}
		}
		ShuffleDeckList(ref data);
		for (int k = 0; k < data.Count; k++)
		{
			data[k].Probability /= num;
			if (k > 0)
			{
				data[k].Probability += data[k - 1].Probability;
			}
		}
		float num2 = UnityEngine.Random.Range(0f, 1f);
		for (int l = 0; l < data.Count; l++)
		{
			if (num2 < data[l].Probability)
			{
				return data[l].Deck;
			}
		}
		throw new UnityException("No deck was drawn in DrawDeck in Scheduler! Critical error");
	}

	private void DrawEvent()
	{
		Deck deck = DrawDeck();
		SurvivalEvent randomEvent = deck.GetRandomEvent();
		CharacterDeck characterDeck = deck as CharacterDeck;
		ScheduleEvent scheduleEvent = new ScheduleEvent(randomEvent);
		if ((bool)characterDeck)
		{
			CharacterParameter characterParameter = characterDeck.GetCharacterParameter();
			if (scheduleEvent.Event.ContainsParameter(characterParameter.Name))
			{
				scheduleEvent.AddCharacterParameter(characterDeck.GetCharacterParameter());
			}
		}
		_nextDay.AddEvent(scheduleEvent);
	}

	private void AddEventToSpecificDay(ScheduleEvent scheduleEvent, int dayOffset, bool forceSchedule = false, bool ignoreCondition = false)
	{
		ScheduledDay scheduledDay = _days.Find((ScheduledDay d) => d.Day == _currentDay + dayOffset);
		if (scheduledDay == null)
		{
			scheduledDay = new ScheduledDay(_currentDay + dayOffset);
			_days.Add(scheduledDay);
		}
		if (scheduleEvent.Event is SurvivalEvent)
		{
			scheduledDay.IgnoreCondition = ignoreCondition;
		}
		scheduledDay.ForceSchedule = forceSchedule;
		scheduledDay.AddEvent(scheduleEvent);
	}

	private void AddSurvivalEventToSpecificDay(ScheduleEvent scheduleEvent, bool forceSchedule = false, int min = 1, int max = 1, bool ignoreCondition = false)
	{
		int dayOffset;
		int num = (dayOffset = UnityEngine.Random.Range(min, max));
		ESchedulePhase eSchedulePhase = ESchedulePhase.INCREASE_DAY;
		int num2 = 0;
		do
		{
			ScheduledDay scheduledDay = _days.Find((ScheduledDay d) => d.Day == _currentDay + dayOffset);
			if (scheduledDay == null)
			{
				AddEventToSpecificDay(scheduleEvent, dayOffset, forceSchedule, ignoreCondition);
				return;
			}
			if (!scheduledDay.HasSurvivalEvent())
			{
				scheduledDay.AddEvent(scheduleEvent);
				scheduledDay.ForceSchedule = forceSchedule;
				scheduledDay.IgnoreCondition = ignoreCondition;
				return;
			}
			if (forceSchedule && scheduledDay.ForceSchedule)
			{
				Debug.LogWarning("DESIGN WARNING: you are force scheduling an event at day where there is a force scheduled event!");
			}
			if ((!forceSchedule || !scheduledDay.HasSurvivalEvent() || !(scheduledDay.SurvivalEvent == _blockingEvent)) & forceSchedule)
			{
				MoveSurvivalEvents(scheduledDay);
				scheduledDay.AddEvent(scheduleEvent);
				scheduledDay.ForceSchedule = true;
				scheduledDay.IgnoreCondition = ignoreCondition;
				return;
			}
			switch (eSchedulePhase)
			{
			case ESchedulePhase.INCREASE_DAY:
			case ESchedulePhase.OUT_OF_RANGE:
				dayOffset++;
				break;
			case ESchedulePhase.DECREASE_DAY:
				dayOffset--;
				break;
			}
			if (dayOffset > max && eSchedulePhase == ESchedulePhase.INCREASE_DAY)
			{
				eSchedulePhase = ESchedulePhase.DECREASE_DAY;
				dayOffset = num - 1;
			}
			else if (dayOffset < min && eSchedulePhase == ESchedulePhase.DECREASE_DAY)
			{
				eSchedulePhase = ESchedulePhase.OUT_OF_RANGE;
				dayOffset = max + 1;
			}
			num2++;
		}
		while (num2 <= 100);
		throw new UnityException("Critical failure: Cannot schedule day - max limit number of loop achieved");
	}

	private void MoveSurvivalEvents(ScheduledDay day)
	{
		_days.Sort();
		int num = -1;
		for (int i = 0; i < _days.Count; i++)
		{
			if (_days[i].Day == day.Day)
			{
				num = i;
				break;
			}
		}
		ScheduledDay scheduledDay = _days[num];
		int num2 = -1;
		int num3 = 0;
		bool flag = true;
		for (int j = num + 1; j < _days.Count; j++)
		{
			num3++;
			if (_days[j].SurvivalEvent == null || _days[j].Day - 1 != scheduledDay.Day)
			{
				if (_days[j].SurvivalEvent == null)
				{
					flag = false;
				}
				num2 = j - 1;
				break;
			}
			scheduledDay = _days[j];
		}
		ScheduledDay scheduledDay2 = null;
		int num4 = -1;
		if (!flag)
		{
			num4 = num2;
			scheduledDay2 = _days[num2 + 1];
		}
		else if (num2 == -1)
		{
			num4 = num + num3;
			scheduledDay2 = new ScheduledDay(day.Day + num3 + 1);
			_days.Add(scheduledDay2);
		}
		else
		{
			num4 = num2;
			scheduledDay2 = new ScheduledDay(_days[num2].Day + 1);
			_days.Add(scheduledDay2);
			_days.Sort();
		}
		for (int num5 = num4; num5 >= num; num5--)
		{
			ScheduledDay scheduledDay3 = _days[num5];
			scheduledDay2.AddEvent(scheduledDay3.SurvivalEvent);
			scheduledDay2.IgnoreCondition = scheduledDay3.IgnoreCondition;
			scheduledDay2.ForceSchedule = scheduledDay3.ForceSchedule;
			scheduledDay3.RemoveEvent(scheduledDay3.SurvivalEvent);
			scheduledDay2 = scheduledDay3;
		}
	}

	public void ScheduleEvent(ParsecsEvent parsecsEvent, bool forceSchedule = false)
	{
		ScheduleEvent scheduleEvent = new ScheduleEvent(parsecsEvent);
		if (parsecsEvent is SurvivalEvent)
		{
			AddSurvivalEventToSpecificDay(scheduleEvent, forceSchedule);
		}
		else
		{
			AddEventToSpecificDay(scheduleEvent, 1);
		}
	}

	public void ScheduleEvent(ParsecsEvent parsecsEvent, List<ItemParameter> items, List<CharacterParameter> characters, bool forceSchedule = false)
	{
		ScheduleEvent scheduleEvent = new ScheduleEvent(parsecsEvent)
		{
			Characters = characters,
			Items = items
		};
		if (parsecsEvent is SurvivalEvent)
		{
			AddSurvivalEventToSpecificDay(scheduleEvent, forceSchedule);
		}
		else
		{
			AddEventToSpecificDay(scheduleEvent, 1);
		}
	}

	public void ScheduleEvent(ParsecsEvent parsecsEvent, int minDelay, int maxDelay, bool forceSchedule = false)
	{
		int dayOffset = UnityEngine.Random.Range(minDelay, maxDelay);
		ScheduleEvent scheduleEvent = new ScheduleEvent(parsecsEvent);
		if (parsecsEvent is SurvivalEvent)
		{
			AddSurvivalEventToSpecificDay(scheduleEvent, forceSchedule, minDelay, maxDelay);
		}
		else
		{
			AddEventToSpecificDay(scheduleEvent, dayOffset);
		}
	}

	public void ScheduleEvent(ParsecsEvent parsecsEvent, int minDelay, int maxDelay, List<ItemParameter> items, List<CharacterParameter> characters, bool forceSchedule = false, bool ignoreCondition = false)
	{
		int dayOffset = UnityEngine.Random.Range(minDelay, maxDelay);
		ScheduleEvent scheduleEvent = new ScheduleEvent(parsecsEvent)
		{
			Characters = characters,
			Items = items
		};
		if (parsecsEvent is SurvivalEvent)
		{
			AddSurvivalEventToSpecificDay(scheduleEvent, forceSchedule, minDelay, maxDelay, ignoreCondition);
		}
		else
		{
			AddEventToSpecificDay(scheduleEvent, dayOffset);
		}
	}

	public void Deschedule(SurvivalEvent survivalEvent)
	{
		for (int i = 0; i < _days.Count; i++)
		{
			ScheduledDay scheduledDay = _days[i];
			if (scheduledDay.SurvivalEvent != null && scheduledDay.SurvivalEvent.Equals(survivalEvent))
			{
				scheduledDay.RemoveEvent(survivalEvent);
				break;
			}
		}
	}

	public bool IsEventScheduled(SurvivalEvent survivalEvent, bool includeActualDay)
	{
		for (int i = 0; i < _days.Count; i++)
		{
			ScheduledDay scheduledDay = _days[i];
			if (scheduledDay.Day >= _currentDay && (includeActualDay || scheduledDay.Day != _currentDay) && scheduledDay.SurvivalEvent != null && scheduledDay.SurvivalEvent.Equals(survivalEvent))
			{
				return true;
			}
		}
		return false;
	}

	public void SetRescheduleEventData(int minDelay, int maxDelay, bool forceSchedule)
	{
		_minRescheduleDelay = minDelay;
		_maxRescheduleDelay = maxDelay;
		_forceSchedule = forceSchedule;
		_rescheduleEvent = true;
	}

	public string Serialize()
	{
		SchedulerWrapper schedulerWrapper = new SchedulerWrapper
		{
			CurrentDayNumber = _currentDay,
			Days = new List<ScheduledDayWrapper>()
		};
		for (int i = 0; i < _days.Count; i++)
		{
			schedulerWrapper.Days.Add(_days[i].Save());
		}
		return JsonUtility.ToJson(schedulerWrapper);
	}

	public void Deserialize(string jsonData)
	{
		SchedulerWrapper schedulerWrapper = JsonUtility.FromJson<SchedulerWrapper>(jsonData);
		_currentDay = schedulerWrapper.CurrentDayNumber;
		_days.Clear();
		for (int i = 0; i < schedulerWrapper.Days.Count; i++)
		{
			_days.Add(new ScheduledDay(schedulerWrapper.Days[i], _saveEvent));
		}
		_currentScheduledDay = _days.Find((ScheduledDay day) => day.Day == _currentDay);
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
		_survivalConfigureEvent = _defaultSurvivalConfigureEvent;
		_initialEvent = _defaultInitialEvent;
		_startDecks = _defaultStartDecks;
		Clean();
	}
}
