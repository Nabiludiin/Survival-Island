using System.Collections.Generic;

namespace RG.Parsecs.Survival;

internal struct SchedulerWrapper
{
	public int CurrentDayNumber;

	public List<ScheduledDayWrapper> Days;
}
