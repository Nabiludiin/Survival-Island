using System;
using RG.Parsecs.Survival;

namespace RG.Parsecs.Common;

[Serializable]
public class AvailableDestinationsChangeStatsEntry : StatsEntry
{
	public PlanetDeck Planet;

	public int AvailableDestinations;

	public override string ToString()
	{
		return $"Available locations entry. Day: {Day}, Planet: {Planet.Id}, Available: {AvailableDestinations}\n";
	}
}
