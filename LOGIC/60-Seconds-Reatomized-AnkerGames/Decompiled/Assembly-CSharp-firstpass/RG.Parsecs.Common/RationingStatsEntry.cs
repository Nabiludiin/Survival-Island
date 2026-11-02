using System;

namespace RG.Parsecs.Common;

[Serializable]
public class RationingStatsEntry : StatsEntry
{
	public string ItemId;

	public string CharacterId;

	public override string ToString()
	{
		return $"Rationing entry. Day: {Day}, Character: {CharacterId}, Item rationed: {ItemId}\n";
	}
}
