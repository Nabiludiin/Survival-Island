using UnityEngine;

public class ChunkPoint
{
	public Vector3 worldPosition = Vector3.zero;

	public ObjectSpawner_Chunk spawner;

	public SO_ChunkSpawnRuleAsset rule;

	public SpawnableFloatingObject spawnableFloatingObject;

	public int poolDuplicateIndex = -1;

	public PickupItem_Networked spawnedObject;

	private bool hasStartedSpawning;

	public bool HasSpawnedObject => spawnedObject != null;

	public bool IsCorrupt { get; private set; }

	public bool IsAddedByForce { get; set; }

	public float RemoveDistanceFromRaft { get; set; }

	public void OnDestroyed()
	{
		IsCorrupt = true;
		RemoveObject();
	}

	public void RemoveObject()
	{
		if (rule != null && rule.BiomeZoneID != BiomeZone.BiomeZoneIdentifier.BIOME_NONE)
		{
			BiomeZone closestBiomeTo = BiomeZoneManager.GetClosestBiomeTo(worldPosition);
			if (closestBiomeTo != null && closestBiomeTo.zoneID == rule.BiomeZoneID)
			{
				BiomeZoneManager.RemoveBiomeZone(closestBiomeTo);
			}
		}
		hasStartedSpawning = false;
		if (spawnedObject != null)
		{
			PickupObjectManager.RemovePickupItemNetwork(spawnedObject);
		}
	}

	public bool SpawnObject(int poolDuplicateIndex)
	{
		if (IsCorrupt)
		{
			return false;
		}
		if (HasSpawnedObject)
		{
			return false;
		}
		if (hasStartedSpawning)
		{
			return false;
		}
		this.poolDuplicateIndex = poolDuplicateIndex;
		hasStartedSpawning = true;
		spawnedObject = spawner.SpawnItemNetwork(worldPosition, spawnableFloatingObject, poolDuplicateIndex);
		return true;
	}

	public bool ShowObject()
	{
		if (IsCorrupt)
		{
			return false;
		}
		if (HasSpawnedObject)
		{
			return spawnedObject.gameObject.SetActiveSafe(value: true);
		}
		return false;
	}

	public bool HideObject()
	{
		if (IsCorrupt)
		{
			return false;
		}
		if (HasSpawnedObject)
		{
			return spawnedObject.gameObject.SetActiveSafe(value: false);
		}
		return false;
	}

	public bool Corrupt()
	{
		if (IsCorrupt)
		{
			return false;
		}
		IsCorrupt = true;
		poolDuplicateIndex = -1;
		return true;
	}
}
