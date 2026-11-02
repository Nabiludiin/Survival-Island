using System;
using System.Collections.Generic;

namespace RG.Parsecs.Survival;

[Serializable]
public struct ScheduledDayWrapper
{
	public int Day;

	public bool ForceSchedule;

	public bool IgnoreCondition;

	public ScheduleEventWrapper SurvivalEvent;

	public List<ScheduleEventWrapper> ExpeditionEvent;

	public List<ScheduleEventWrapper> ReportEvent;

	public List<ScheduleEventWrapper> SystemEvent;
}
