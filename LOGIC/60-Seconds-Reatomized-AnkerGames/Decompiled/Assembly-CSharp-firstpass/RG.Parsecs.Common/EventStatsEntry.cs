using System;

namespace RG.Parsecs.Common;

[Serializable]
public class EventStatsEntry : StatsEntry
{
	public string EventId;

	public override string ToString()
	{
		return string.Format("Event entry. Day: {0},");
	}
}
