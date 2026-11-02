using System;
using System.Collections.Generic;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class ScheduledDay : IComparable
{
	[SerializeField]
	private List<ScheduleEvent> _survivalEvents = new List<ScheduleEvent>();

	[SerializeField]
	private List<ScheduleEvent> _expeditionEvents = new List<ScheduleEvent>();

	[SerializeField]
	private List<ScheduleEvent> _reportEvents = new List<ScheduleEvent>();

	[SerializeField]
	private List<ScheduleEvent> _systemEvents = new List<ScheduleEvent>();

	[SerializeField]
	private int _day;

	[SerializeField]
	private bool _forceSchedule;

	[SerializeField]
	private bool _ignoreCondition;

	public SurvivalEvent SurvivalEvent
	{
		get
		{
			if (_survivalEvents == null)
			{
				return null;
			}
			if (_survivalEvents.Count > 0)
			{
				if (_survivalEvents[0] == null)
				{
					return null;
				}
				return (SurvivalEvent)_survivalEvents[0].Event;
			}
			return null;
		}
	}

	public ExpeditionEvent ExpeditionEvent
	{
		get
		{
			if (_expeditionEvents.Count <= 0)
			{
				return null;
			}
			return (ExpeditionEvent)_expeditionEvents[0].Event;
		}
	}

	public bool ForceSchedule
	{
		get
		{
			return _forceSchedule;
		}
		set
		{
			_forceSchedule = value;
		}
	}

	public bool IgnoreCondition
	{
		get
		{
			return _ignoreCondition;
		}
		set
		{
			_ignoreCondition = value;
		}
	}

	public int Day
	{
		get
		{
			return _day;
		}
		set
		{
			_day = value;
		}
	}

	public ScheduledDay(int day)
	{
		_day = day;
		_survivalEvents = new List<ScheduleEvent>();
		_expeditionEvents = new List<ScheduleEvent>();
		_reportEvents = new List<ScheduleEvent>();
		_systemEvents = new List<ScheduleEvent>();
	}

	public ScheduledDay(ScheduledDayWrapper wrapper, SaveEvent saveEvent)
	{
		_day = wrapper.Day;
		_ignoreCondition = wrapper.IgnoreCondition;
		_forceSchedule = wrapper.ForceSchedule;
		if (!string.IsNullOrEmpty(wrapper.SurvivalEvent.EventName))
		{
			_survivalEvents = new List<ScheduleEvent>
			{
				new ScheduleEvent(wrapper.SurvivalEvent, saveEvent)
			};
		}
		_expeditionEvents = new List<ScheduleEvent>();
		for (int i = 0; i < wrapper.ExpeditionEvent.Count; i++)
		{
			_expeditionEvents.Add(new ScheduleEvent(wrapper.ExpeditionEvent[i], saveEvent));
		}
		_reportEvents = new List<ScheduleEvent>();
		for (int j = 0; j < wrapper.ReportEvent.Count; j++)
		{
			_reportEvents.Add(new ScheduleEvent(wrapper.ReportEvent[j], saveEvent));
		}
		_systemEvents = new List<ScheduleEvent>();
		for (int k = 0; k < wrapper.SystemEvent.Count; k++)
		{
			_systemEvents.Add(new ScheduleEvent(wrapper.SystemEvent[k], saveEvent));
		}
	}

	public ScheduledDayWrapper Save()
	{
		ScheduledDayWrapper result = new ScheduledDayWrapper
		{
			Day = _day,
			SurvivalEvent = default,
			ExpeditionEvent = new List<ScheduleEventWrapper>(),
			ReportEvent = new List<ScheduleEventWrapper>(),
			SystemEvent = new List<ScheduleEventWrapper>(),
			IgnoreCondition = _ignoreCondition,
			ForceSchedule = _forceSchedule
		};
		if (_survivalEvents != null && _survivalEvents.Count > 0)
		{
			result.SurvivalEvent = _survivalEvents[0].Save();
		}
		for (int i = 0; i < _expeditionEvents.Count; i++)
		{
			result.ExpeditionEvent.Add(_expeditionEvents[i].Save());
		}
		for (int j = 0; j < _reportEvents.Count; j++)
		{
			result.ReportEvent.Add(_reportEvents[j].Save());
		}
		for (int k = 0; k < _systemEvents.Count; k++)
		{
			result.SystemEvent.Add(_systemEvents[k].Save());
		}
		return result;
	}

	public bool HasSurvivalEvent()
	{
		return _survivalEvents.Count > 0;
	}

	public void ExecuteEndDay()
	{
		for (int i = 0; i < _survivalEvents.Count; i++)
		{
			_survivalEvents[i].RunEventWithScheduledParameters();
		}
		for (int j = 0; j < _reportEvents.Count; j++)
		{
			_reportEvents[j].RunEventWithScheduledParameters();
		}
		for (int k = 0; k < _expeditionEvents.Count; k++)
		{
			_expeditionEvents[k].RunEventWithScheduledParameters();
		}
	}

	public void ExecuteSystemsEvents()
	{
		int num = 0;
		while (num < _systemEvents.Count)
		{
			num = _systemEvents.Count;
			for (int i = 0; i < num; i++)
			{
				_systemEvents[i].RunEventWithScheduledParameters();
			}
		}
	}

	public void ExecuteNewDay()
	{
		for (int i = 0; i < _survivalEvents.Count; i++)
		{
			_survivalEvents[i].RunEventWithScheduledParameters();
			_survivalEvents[i].RunEventWithScheduledParameters();
		}
	}

	public void AddEvent(ScheduleEvent scheduledEvent)
	{
		if (scheduledEvent.Event is SurvivalEvent)
		{
			_survivalEvents.Add(scheduledEvent);
		}
		else if (scheduledEvent.Event is ExpeditionEvent)
		{
			_expeditionEvents.Add(scheduledEvent);
		}
		else if (scheduledEvent.Event is ReportEvent)
		{
			_reportEvents.Add(scheduledEvent);
		}
		else
		{
			_systemEvents.Add(scheduledEvent);
		}
	}

	public void AddEvent(ExpeditionEvent expeditionEvent)
	{
		_expeditionEvents.Add(new ScheduleEvent(expeditionEvent));
	}

	public void AddEvent(SurvivalEvent survivalEvent)
	{
		_survivalEvents.Add(new ScheduleEvent(survivalEvent));
	}

	public void AddEvent(ReportEvent reportEvent)
	{
		_reportEvents.Add(new ScheduleEvent(reportEvent));
	}

	public void AddEvent(SystemEvent systemEvent)
	{
		_systemEvents.Add(new ScheduleEvent(systemEvent));
	}

	public bool RemoveEvent(SurvivalEvent survivalEvent)
	{
		return _survivalEvents.Remove(_survivalEvents.Find((ScheduleEvent e) => e.Event.Equals(survivalEvent)));
	}

	public ScheduleEvent GetMainEvent()
	{
		return _survivalEvents[0];
	}

	public bool RemoveEvent(ExpeditionEvent expeditionEvent)
	{
		return _expeditionEvents.Remove(_expeditionEvents.Find((ScheduleEvent e) => e.Event.Equals(expeditionEvent)));
	}

	public bool RemoveEvent(ReportEvent reportEvent)
	{
		return _reportEvents.Remove(_reportEvents.Find((ScheduleEvent e) => e.Event.Equals(reportEvent)));
	}

	public bool RemoveEvent(SystemEvent systemEvent)
	{
		return _systemEvents.Remove(_systemEvents.Find((ScheduleEvent e) => e.Event.Equals(systemEvent)));
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		ScheduledDay scheduledDay = (ScheduledDay)obj;
		return Day.CompareTo(scheduledDay.Day);
	}
}
