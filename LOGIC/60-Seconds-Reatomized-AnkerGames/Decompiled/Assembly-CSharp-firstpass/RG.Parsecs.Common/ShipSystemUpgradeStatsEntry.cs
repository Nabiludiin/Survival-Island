using System;
using RG.Parsecs.Survival;

namespace RG.Parsecs.Common;

[Serializable]
public class ShipSystemUpgradeStatsEntry : StatsEntry
{
	public string ShipSystemId;

	public EPlannedCraftingAction Action;

	public int SystemLevel;

	public override string ToString()
	{
		return $"System upgrade entry. Day: {Day}, System: {ShipSystemId}, Action: {Action}, SystemLevel: {SystemLevel}\n";
	}
}
