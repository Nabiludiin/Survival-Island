using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

public class Room : Module
{
	private List<ItemSpawnPoint> _spawnPoints = new List<ItemSpawnPoint>();

	public List<ItemSpawnPoint> SpawnPoints => _spawnPoints;

	public override void Load()
	{
	}

	public override void Unload()
	{
	}

	public void RegisterSpawnPoint(ItemSpawnPoint spawnPoint)
	{
		_spawnPoints.Add(spawnPoint);
	}
}
