using System;
using System.Collections.Generic;

namespace RG.Parsecs.Common;

[Serializable]
internal struct StatsManagerWrapper
{
	public List<CharacterStatusChangeStatsEntry> CharacterStatusChangeStatsEntries;

	public List<ExpeditionStatsEntry> ExpeditionStatsEntries;

	public List<EventStatsEntry> EventStatsEntries;

	public List<ShipSystemUpgradeStatsEntry> SystemUpgradeStatsEntries;

	public List<CraftingStatsEntry> CraftingStatsEntries;

	public List<TestStatsEntry> TestStatsEntries;

	public List<NeedStatsEntry> NeedStatsEntries;

	public List<RationingStatsEntry> RationingStatsEntries;

	public List<ItemCollectedStatsEntry> ItemCollectedStatsEntries;

	public List<AvailableDestinationsChangeStatsEntry> AvailableDestinationsChangeStatsEntries;

	public List<YesNoDecisionStatsEntry> YesNoDecisionStatsEntries;
}
