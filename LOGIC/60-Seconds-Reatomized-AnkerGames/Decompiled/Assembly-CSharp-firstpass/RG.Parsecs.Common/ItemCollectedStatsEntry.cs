using System;

namespace RG.Parsecs.Common;

[Serializable]
public class ItemCollectedStatsEntry : StatsEntry
{
	public string ItemId;

	public bool FromExpedition;

	public override string ToString()
	{
		return $"Item collected entry. Day: {Day}, Item: {ItemId}, From Expedition: {FromExpedition}\n";
	}
}
