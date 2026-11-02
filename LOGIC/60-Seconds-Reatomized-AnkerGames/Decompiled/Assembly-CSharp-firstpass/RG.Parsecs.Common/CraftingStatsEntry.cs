using System;
using RG.Parsecs.Survival;

namespace RG.Parsecs.Common;

[Serializable]
public class CraftingStatsEntry : StatsEntry
{
	public string CraftedItemId;

	public EPlannedCraftingAction Action;

	public int CraftedItemLevel;

	public override string ToString()
	{
		return $"Crafting entry. Day: {Day}, Item: {CraftedItemId}, Action: {Action}, ItemLevel: {CraftedItemLevel}\n";
	}
}
