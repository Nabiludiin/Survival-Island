using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkSpawnerRule", menuName = "Raft/ChunkSpawnerRule")]
public class SO_ChunkSpawnRuleAsset : ScriptableObject
{
	public bool isFrequencyPoint;

	[SerializeField]
	private ChunkPointType chunkPointType;

	[SerializeField]
	private BiomeZone.BiomeZoneIdentifier biomeZoneID;

	[SerializeField]
	private float biomeZoneRadius = 200f;

	[SerializeField]
	private Interval_Int spawnsPerChunk;

	[Range(0f, 100f)]
	[SerializeField]
	private float spawnChance;

	[SerializeField]
	private float collisionOverlapRadius = 100f;

	[SerializeField]
	private float viewDistance = 400f;

	[SerializeField]
	private bool useMinDistance = true;

	[SerializeField]
	private List<ChunkPointType> others = new List<ChunkPointType>();

	[SerializeField]
	private float minDistanceToOthers;

	[SerializeField]
	private Interval_Float spawnDistanceFromRaft;

	[SerializeField]
	private Interval_Float spawnAngle;

	[SerializeField]
	private Color chunkPointGizmoColor = Color.white;

	public ChunkPointType ChunkPointType
	{
		get
		{
			return chunkPointType;
		}
		private set
		{
			chunkPointType = value;
		}
	}

	public BiomeZone.BiomeZoneIdentifier BiomeZoneID
	{
		get
		{
			return biomeZoneID;
		}
		private set
		{
			biomeZoneID = value;
		}
	}

	public float BiomeZoneRadius
	{
		get
		{
			return biomeZoneRadius;
		}
		private set
		{
			biomeZoneRadius = value;
		}
	}

	public Interval_Int SpawnsPerChunk
	{
		get
		{
			return spawnsPerChunk;
		}
		private set
		{
			spawnsPerChunk = value;
		}
	}

	public float CollisionOverlapRadius
	{
		get
		{
			return collisionOverlapRadius;
		}
		private set
		{
			collisionOverlapRadius = value;
		}
	}

	public float ViewDistance => viewDistance;

	public Interval_Float SpawnDistanceFromRaft
	{
		get
		{
			return spawnDistanceFromRaft;
		}
		private set
		{
			spawnDistanceFromRaft = value;
		}
	}

	public Interval_Float SpawnAngle
	{
		get
		{
			return spawnAngle;
		}
		private set
		{
			spawnAngle = value;
		}
	}

	public Color ChunkPointGizmoColor
	{
		get
		{
			return chunkPointGizmoColor;
		}
		private set
		{
			chunkPointGizmoColor = value;
		}
	}

	public bool TestSpawnChance()
	{
		return Random.Range(0f, 100f) < spawnChance;
	}

	public bool DoesPointPassSpawnRules(ChunkPoint pointToCheck, List<List<ChunkPoint>> points)
	{
		return true;
	}

	public bool DoesPointPassSpawnRules(ChunkPoint pointToCheck, ChunkPoint pointToCompare)
	{
		if (!useMinDistance)
		{
			return true;
		}
		if (others.Contains(pointToCompare.rule.ChunkPointType) && pointToCheck.worldPosition.DistanceXZ(pointToCompare.worldPosition) < minDistanceToOthers)
		{
			return false;
		}
		return true;
	}
}
