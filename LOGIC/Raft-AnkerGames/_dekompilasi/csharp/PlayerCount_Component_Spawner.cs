using System.Collections.Generic;

public class PlayerCount_Component_Spawner : PlayerCount_Component
{
	public LandmarkEntityCollection spawnerCollection;

	public List<LandmarkEntitySpawner> spawner_lowRange;

	public List<LandmarkEntitySpawner> spawner_mediumRange;

	public List<LandmarkEntitySpawner> spawner_highRange;

	protected override void RangeUpdated(Interval_Int newRange)
	{
		base.RangeUpdated(newRange);
		if (Raft_Network.IsHost)
		{
			SetSpawnersCanSpawn(spawner_lowRange, value: false);
			SetSpawnersCanSpawn(spawner_mediumRange, value: false);
			SetSpawnersCanSpawn(spawner_highRange, value: false);
			if (newRange == playerRange.lowRange)
			{
				SetSpawnersCanSpawn(spawner_lowRange, value: true);
			}
			else if (newRange == playerRange.mediumRange)
			{
				SetSpawnersCanSpawn(spawner_mediumRange, value: true);
			}
			else if (newRange == playerRange.highRange)
			{
				SetSpawnersCanSpawn(spawner_highRange, value: true);
			}
		}
	}

	private void SetSpawnersCanSpawn(List<LandmarkEntitySpawner> spawners, bool value)
	{
		foreach (LandmarkEntitySpawner spawner in spawners)
		{
			if (spawner != null)
			{
				spawner.ShouldSpawn = value;
			}
		}
	}
}
