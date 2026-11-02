using System;

namespace RG.Parsecs.Common;

[Serializable]
public class YesNoDecisionStatsEntry : StatsEntry
{
	public bool YesDecision;

	public override string ToString()
	{
		return $"YesNoDecision collected entry. Day: {Day}, yes decision: {YesDecision}\n";
	}
}
