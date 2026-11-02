using System.Collections;
using System.Collections.Generic;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class SchedulerManager : MonoBehaviour
{
	[SerializeField]
	private Scheduler _scheduler;

	[SerializeField]
	private DeckList _devToolDeckList;

	[SerializeField]
	private SurvivalEventReferenceList _devToolReferenceList;

	[SerializeField]
	private EndOfDayListenerList _endOfDay;

	[SerializeField]
	private GlobalBoolVariable _survivalOnly;

	[SerializeField]
	private SaveManager _saveManager;

	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private EventTagList _forbiddenTags;

	private static SchedulerManager _instance;

	public bool FromSave;

	protected bool _saveFirstDay = true;

	public static SchedulerManager Instance => _instance;

	protected void Start()
	{
		_instance = this;
		StartCoroutine(StartGame());
	}

	public IEnumerator StartGame()
	{
		if (!FromSave)
		{
			yield return _scheduler.CleanState();
			if (_survivalOnly.Value && _scheduler.SurvivalConfigureEvent != null)
			{
				_scheduler.SurvivalConfigureEvent.Run();
			}
			_characterList.SetStartingData();
			_scheduler.InitialEvent.Run();
			_scheduler.PrepareFirstDay();
			RunEnterEvents();
			GoalManager.Instance.CheckPersonalGoal();
			if (_saveFirstDay)
			{
				_saveManager.ChangeSaveGameState();
			}
			else
			{
				_saveManager.SaveGlobalGameData();
			}
		}
		InitializeDecksOnSaveFinish();
		_scheduler.StartDay();
		_endOfDay.UpdateVisualLayer();
		yield return new WaitForFixedUpdate();
	}

	private void InitializeDecksOnSaveFinish()
	{
		if (EventManager.Instance != null)
		{
			for (int i = 0; i < EventManager.ActiveDecks.Count; i++)
			{
				EventManager.ActiveDecks[i].InitializeDeck();
				EventManager.ActiveDecks[i].Recalculate(_forbiddenTags.EventTags);
			}
		}
	}

	private void RunEnterEvents()
	{
		for (int i = 0; i < _characterList.GetCharacterCount(); i++)
		{
			Character character = _characterList.CharactersInGame[i];
			List<CharacterStatus> currentStatuses = character.RuntimeData.CurrentStatuses;
			for (int j = 0; j < currentStatuses.Count; j++)
			{
				CharacterStatusBehaviour behaviourForStatus = character.StaticData.GetBehaviourForStatus(currentStatuses[j]);
				if (behaviourForStatus != null && behaviourForStatus.OnStatusEnterEvent != null)
				{
					behaviourForStatus.OnStatusEnterEvent.SetCharacterParameterByName("StatusCharacter", character);
					behaviourForStatus.OnStatusEnterEvent.Run();
				}
			}
		}
	}

	public bool IsSurvivalEventScheduledOnNextDay()
	{
		return _scheduler.GetNextScheduledDay()?.HasSurvivalEvent() ?? false;
	}

	public void SkipDays(int daysToSkip)
	{
		_scheduler.SkipDays(daysToSkip);
		if (ExpeditionManager.Instance.IsExpeditionOngoing() || ExpeditionManager.Instance.IsExpeditionPlanned())
		{
			ExpeditionManager.Instance.SkipExpeditionDays(daysToSkip);
		}
	}

	public void Schedule(ParsecsEvent parsecsEvent, int minDelay, int maxDelay, List<ItemParameter> items, List<CharacterParameter> characters, bool forceSchedule = false, bool ignoreCondition = false)
	{
		_scheduler.ScheduleEvent(parsecsEvent, minDelay, maxDelay, items, characters, forceSchedule, ignoreCondition);
	}

	public void Schedule(ParsecsEvent parsecsEvent, int minDelay, int maxDelay, bool forceSchedule = false)
	{
		_scheduler.ScheduleEvent(parsecsEvent, minDelay, maxDelay, forceSchedule);
	}

	public void Schedule(ParsecsEvent parsecsEvent, int days, bool forceSchedule = false)
	{
		_scheduler.ScheduleEvent(parsecsEvent, days, days, forceSchedule);
	}

	public void Schedule(ParsecsEvent parsecsEvent, List<ItemParameter> items, List<CharacterParameter> characters, bool forceSchedule = false)
	{
		_scheduler.ScheduleEvent(parsecsEvent, items, characters, forceSchedule);
	}

	public void Schedule(ParsecsEvent parsecsEvent, bool forceSchedule = false)
	{
		_scheduler.ScheduleEvent(parsecsEvent, forceSchedule);
	}

	public void Dechsedule(SurvivalEvent survivalEvent)
	{
		_scheduler.Deschedule(survivalEvent);
	}

	public bool IsEventScheduled(SurvivalEvent survivalEvent, bool includeActualDay)
	{
		return _scheduler.IsEventScheduled(survivalEvent, includeActualDay);
	}

	public void RescheduleEvent(int minDelay, int maxDelay, bool forceSchedule)
	{
		_scheduler.SetRescheduleEventData(minDelay, maxDelay, forceSchedule);
	}
}
