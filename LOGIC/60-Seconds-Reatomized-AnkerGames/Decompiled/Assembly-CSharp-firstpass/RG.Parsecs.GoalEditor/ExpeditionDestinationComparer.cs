using System.Collections.Generic;
using RG.Parsecs.Common;

namespace RG.Parsecs.GoalEditor;

internal class ExpeditionDestinationComparer : IEqualityComparer<ExpeditionStatsEntry>
{
	public bool Equals(ExpeditionStatsEntry x, ExpeditionStatsEntry y)
	{
		return x.DestinationId == y.DestinationId;
	}

	public int GetHashCode(ExpeditionStatsEntry obj)
	{
		return obj.DestinationId.GetHashCode();
	}
}
