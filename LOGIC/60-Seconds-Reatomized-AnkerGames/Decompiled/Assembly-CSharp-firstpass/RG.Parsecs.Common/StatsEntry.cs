using System;

namespace RG.Parsecs.Common;

[Serializable]
public class StatsEntry
{
	public int Day;

	public DateTime Timestamp;

	public EGameState GameState;

	public EGameMode GameMode;

	public bool OnPlanet;
}
