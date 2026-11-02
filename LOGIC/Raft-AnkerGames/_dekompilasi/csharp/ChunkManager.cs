using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;

public class ChunkManager : MonoBehaviour_ID_Network, SceneEventInterface
{
	public static bool RaftIsInsideChunkPoint;

	public static float RaftDistanceToClosestChunkPoint;

	private Transform _raftTransform;

	public bool drawGizmos = true;

	[SerializeField]
	private Vector2Int numberOfChunks = new Vector2Int(3, 3);

	public static uint ChunkSize = 1000u;

	[SerializeField]
	private Interval_Int emptyAtStartXInterval;

	[SerializeField]
	private Interval_Int emptyAtStartYInterval;

	public List<ObjectSpawner_Chunk> currentSpawners = new List<ObjectSpawner_Chunk>();

	private SO_ChunkSpawnRuleAsset[] allChunkPointRules;

	public List<ObjectSpawner_Chunk> allChunkSpawners = new List<ObjectSpawner_Chunk>();

	[SerializeField]
	private float chunkCheckUpdateRate = 1f;

	public TimerEventer checkExtraSpawners;

	[SerializeField]
	private float addbigIslandTimer = 30f;

	private float chunkCheckUpdateProgress;

	private Raft_Network network;

	private Chunk[,] chunks;

	private Dictionary<ChunkPointType, List<ChunkPoint>> chunkPoints = new Dictionary<ChunkPointType, List<ChunkPoint>>();

	private const int ViewDistance = 400;

	private bool isDirty = true;

	public Transform RaftTransform
	{
		get
		{
			if (_raftTransform == null)
			{
				_raftTransform = SingletonGeneric<GameManager>.Singleton.raftTransform;
			}
			return _raftTransform;
		}
	}

	public Rect TotalBounds => new Rect(BottomLeftCorner.x, BottomLeftCorner.y, TotalChunkWidth, TotalChunkHeight);

	public int TotalChunkWidth => (int)ChunkSize * numberOfChunks.x;

	public int TotalChunkHeight => (int)ChunkSize * numberOfChunks.y;

	public Vector3 BottomLeftCorner => Vector3.zero + new Vector3((float)ChunkSize / 2f, 0f, (float)ChunkSize / 2f) - new Vector3((float)TotalChunkWidth / 2f, 0f, (float)TotalChunkHeight / 2f);

	public float HalfChunkSize => (float)ChunkSize / 2f;

	public bool HasSpawnedType(ChunkPointType type)
	{
		if (chunkPoints.ContainsKey(type))
		{
			return chunkPoints[type].Count > 0;
		}
		return false;
	}

	private void Awake()
	{
		ComponentManager<ChunkManager>.Value = this;
		allChunkPointRules = Resources.LoadAll<SO_ChunkSpawnRuleAsset>("ChunkSpawnerRules");
		InitializeChunks();
		SortSpawnersByPriority();
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	private void Start()
	{
		NetworkIDManager.AddNetworkID(this);
		network = ComponentManager<Raft_Network>.Value;
		ObjectSpawnerManager value = ComponentManager<ObjectSpawnerManager>.Value;
		if (!(value != null))
		{
			return;
		}
		for (int i = 0; i < value.spawners.Count; i++)
		{
			ObjectSpawner objectSpawner = value.spawners[i];
			if (objectSpawner != null && objectSpawner is ObjectSpawner_Chunk)
			{
				allChunkSpawners.Add(objectSpawner as ObjectSpawner_Chunk);
			}
		}
	}

	private void Update()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (checkExtraSpawners.DoUpdate(Time.deltaTime, autoResetTimer: true) && GameManager.TimePlayed >= addbigIslandTimer * 60f)
		{
			AddSpawnerToCurrentSpawners(ChunkPointType.Landmark_Big);
		}
		if (RaftTransform == null)
		{
			return;
		}
		chunkCheckUpdateProgress += Time.deltaTime;
		if (chunkCheckUpdateProgress < chunkCheckUpdateRate)
		{
			return;
		}
		chunkCheckUpdateProgress = 0f;
		RaftDistanceToClosestChunkPoint = -1f;
		RaftIsInsideChunkPoint = false;
		foreach (List<ChunkPoint> value in chunkPoints.Values)
		{
			for (int i = 0; i < value.Count; i++)
			{
				ChunkPoint chunkPoint = value[i];
				if (chunkPoint == null)
				{
					value.RemoveAt(i);
					i--;
					continue;
				}
				float num = RaftTransform.position.DistanceXZ(chunkPoint.worldPosition);
				float maxDistanceFromPoint = ((chunkPoint.rule != null) ? chunkPoint.rule.ViewDistance : 400f);
				Network_Player network_Player = Helper.ClosestPlayerToPoint(chunkPoint.worldPosition, maxDistanceFromPoint, includeDeadPlayers: true, ignoreYValue: true);
				if (network_Player != null)
				{
					if (!chunkPoint.IsCorrupt)
					{
						if (!chunkPoint.HasSpawnedObject)
						{
							BiomeZone biomeAtPoint = BiomeZoneManager.GetBiomeAtPoint(chunkPoint.worldPosition);
							ObjectSpawner_Chunk spawner = GetSpawner(chunkPoint.rule.ChunkPointType, biomeAtPoint);
							SpawnableFloatingObject objectToSpawn = spawner.GetObjectToSpawn();
							if (spawner != null && objectToSpawn != null)
							{
								List<ChunkPoint> chunkPointsFromPointType = GetChunkPointsFromPointType(chunkPoint.rule.ChunkPointType);
								chunkPoint.spawner = spawner;
								chunkPoint.spawnableFloatingObject = objectToSpawn;
								chunkPoint.poolDuplicateIndex = chunkPoint.spawner.GetRandomUnusedPoolDuplicateIndexFromSpawnable(chunkPoint.spawnableFloatingObject, chunkPointsFromPointType);
								isDirty = true;
							}
							if (chunkPoint.poolDuplicateIndex >= 0)
							{
								if (chunkPoint.SpawnObject(chunkPoint.poolDuplicateIndex))
								{
									isDirty = true;
								}
							}
							else if (chunkPoint.Corrupt())
							{
								isDirty = true;
							}
						}
						if (!chunkPoint.IsCorrupt && chunkPoint.ShowObject())
						{
							isDirty = true;
						}
					}
				}
				else if (chunkPoint.HideObject())
				{
					isDirty = true;
				}
				if (chunkPoint != null && !chunkPoint.IsCorrupt && chunkPoint.rule != null)
				{
					if (RaftDistanceToClosestChunkPoint < 0f || num < RaftDistanceToClosestChunkPoint)
					{
						RaftDistanceToClosestChunkPoint = num;
					}
					if (num < chunkPoint.rule.CollisionOverlapRadius * 1.25f)
					{
						RaftIsInsideChunkPoint = true;
					}
				}
				if (chunkPoint.RemoveDistanceFromRaft > 0f && num >= chunkPoint.RemoveDistanceFromRaft && network_Player == null && !chunkPoint.IsCorrupt)
				{
					chunkPoint.OnDestroyed();
					value.RemoveAt(i);
					i--;
					isDirty = true;
				}
			}
		}
		if (isDirty)
		{
			isDirty = false;
			SendMessageRefreshPoints();
		}
	}

	private void OnValidate()
	{
		if (numberOfChunks.x <= 0)
		{
			numberOfChunks.x = 1;
		}
		if (numberOfChunks.y <= 0)
		{
			numberOfChunks.y = 1;
		}
		emptyAtStartXInterval.minValue = Mathf.Clamp(emptyAtStartXInterval.minValue, 0, numberOfChunks.x - 1);
		emptyAtStartXInterval.maxValue = Mathf.Clamp(emptyAtStartXInterval.maxValue, 0, numberOfChunks.x - 1);
		emptyAtStartYInterval.minValue = Mathf.Clamp(emptyAtStartYInterval.minValue, 0, numberOfChunks.y - 1);
		emptyAtStartYInterval.maxValue = Mathf.Clamp(emptyAtStartYInterval.maxValue, 0, numberOfChunks.y - 1);
	}

	private void OnDrawGizmos()
	{
		_ = drawGizmos;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this);
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			RaftIsInsideChunkPoint = false;
			RaftDistanceToClosestChunkPoint = -1f;
		}
	}

	private void OnWorldShift(Vector3 shift)
	{
		CoroutineManager.Singleton.AddCoroutine(DelayedShift(shift, 0.1f));
	}

	private IEnumerator DelayedShift(Vector3 shift, float delay)
	{
		yield return new WaitForSeconds(delay);
		shift *= -1f;
		foreach (List<ChunkPoint> value in chunkPoints.Values)
		{
			for (int i = 0; i < value.Count; i++)
			{
				ChunkPoint chunkPoint = value[i];
				if (chunkPoint == null)
				{
					continue;
				}
				chunkPoint.worldPosition += shift;
				if (Raft_Network.IsHost && chunkPoint.RemoveDistanceFromRaft == 0f && GetChunkFromWorldPosition(chunkPoint.worldPosition) == null)
				{
					float maxDistanceFromPoint = ((chunkPoint.rule != null) ? chunkPoint.rule.ViewDistance : 400f);
					if (Helper.ClosestPlayerToPoint(chunkPoint.worldPosition, maxDistanceFromPoint, includeDeadPlayers: true, ignoreYValue: true) == null)
					{
						chunkPoint.OnDestroyed();
						value.RemoveAt(i);
						i--;
					}
				}
			}
		}
		if (Raft_Network.IsHost)
		{
			int num = 0;
			int num2 = numberOfChunks.x;
			int num3 = 0;
			int num4 = numberOfChunks.y;
			if (shift.z != 0f)
			{
				if (shift.z < 0f)
				{
					num3 = num4 - 1;
				}
				else
				{
					num4 = num3 + 1;
				}
			}
			else if (shift.x != 0f)
			{
				if (shift.x < 0f)
				{
					num = num2 - 1;
				}
				else
				{
					num2 = num + 1;
				}
			}
			for (int j = num3; j < num4; j++)
			{
				for (int k = num; k < num2; k++)
				{
					GenerateChunkData(k, j);
				}
			}
		}
		isDirty = true;
	}

	public void RemoveSpawnerFromCurrentSpawners(ChunkPointType pointType)
	{
		if (Raft_Network.IsHost)
		{
			ObjectSpawner_Chunk spawner = GetSpawner(pointType);
			if (!(spawner == null) && currentSpawners.Contains(spawner))
			{
				currentSpawners.Remove(spawner);
			}
		}
	}

	public void AddSpawnerToCurrentSpawners(ChunkPointType pointType)
	{
		if (Raft_Network.IsHost)
		{
			ObjectSpawner_Chunk spawner = GetSpawner(pointType);
			AddSpawnerToCurrentSpawners(spawner);
		}
	}

	public void AddSpawnerToCurrentSpawners(ObjectSpawner_Chunk newSpawner)
	{
		if (Raft_Network.IsHost && !(newSpawner == null) && !currentSpawners.Contains(newSpawner))
		{
			currentSpawners.Add(newSpawner);
			SortSpawnersByPriority();
		}
	}

	public ChunkPoint AddChunkPointForcibly(ChunkPointType pointType)
	{
		if (!Raft_Network.IsHost)
		{
			return null;
		}
		SO_ChunkSpawnRuleAsset ruleFromPointType = GetRuleFromPointType(pointType);
		if (ruleFromPointType == null)
		{
			Debug.Log("Can't force spawn chunkpoint with type " + pointType.ToString() + " because no rule could be found for that type.");
			return null;
		}
		Vector3 position = RaftTransform.position;
		Vector3 vector = Helper.XZDirectionFromAngle(ruleFromPointType.SpawnAngle.GetRandomValue());
		position += vector * ruleFromPointType.SpawnDistanceFromRaft.GetRandomValue();
		ChunkPoint chunkPoint = new ChunkPoint
		{
			worldPosition = position,
			rule = ruleFromPointType,
			IsAddedByForce = true
		};
		bool flag;
		do
		{
			flag = true;
			foreach (List<ChunkPoint> value in chunkPoints.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					ChunkPoint chunkPoint2 = value[i];
					if (chunkPoint2 != null && chunkPoint2 != chunkPoint && !chunkPoint.rule.DoesPointPassSpawnRules(chunkPoint, chunkPoint2))
					{
						flag = false;
						chunkPoint.worldPosition += vector * 200f;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
		}
		while (!flag);
		chunkPoint.RemoveDistanceFromRaft = chunkPoint.worldPosition.DistanceXZ(RaftTransform.position) + 250f;
		AddChunkPoint(chunkPoint);
		if (chunkPoint.rule.BiomeZoneID != BiomeZone.BiomeZoneIdentifier.BIOME_NONE)
		{
			BiomeZoneManager.AddBiomeZone(new BiomeZone(chunkPoint.rule.BiomeZoneID, chunkPoint.worldPosition, chunkPoint.rule.BiomeZoneRadius));
		}
		foreach (List<ChunkPoint> value2 in chunkPoints.Values)
		{
			for (int j = 0; j < value2.Count; j++)
			{
				ChunkPoint chunkPoint3 = value2[j];
				if (chunkPoint3 != null && chunkPoint3 != chunkPoint && !DoesPointFit(chunkPoint3) && (!chunkPoint3.HasSpawnedObject || !chunkPoint3.spawnedObject.gameObject.activeInHierarchy))
				{
					chunkPoint3.OnDestroyed();
					value2.RemoveAt(j);
					j--;
				}
			}
		}
		isDirty = true;
		return chunkPoint;
	}

	public ChunkPoint AddChunkPointCheat(ChunkPointType pointType, Vector3 spawnerOffset)
	{
		if (!Raft_Network.IsHost)
		{
			return null;
		}
		SO_ChunkSpawnRuleAsset ruleFromPointType = GetRuleFromPointType(pointType);
		if (ruleFromPointType == null)
		{
			Debug.Log("Tried to add chunkpoint with type " + pointType.ToString() + " but could not find any rule");
			return null;
		}
		ChunkPoint chunkPoint = new ChunkPoint
		{
			worldPosition = RaftTransform.position + spawnerOffset,
			IsAddedByForce = false,
			rule = ruleFromPointType
		};
		AddChunkPoint(chunkPoint);
		if (chunkPoint.rule.BiomeZoneID != BiomeZone.BiomeZoneIdentifier.BIOME_NONE)
		{
			BiomeZoneManager.AddBiomeZone(new BiomeZone(chunkPoint.rule.BiomeZoneID, chunkPoint.worldPosition, chunkPoint.rule.BiomeZoneRadius));
		}
		foreach (List<ChunkPoint> value in chunkPoints.Values)
		{
			for (int i = 0; i < value.Count; i++)
			{
				ChunkPoint chunkPoint2 = value[i];
				if (chunkPoint2 != null && chunkPoint2 != chunkPoint && !DoesPointFit(chunkPoint2))
				{
					chunkPoint2.OnDestroyed();
					value.RemoveAt(i);
					i--;
				}
			}
		}
		isDirty = true;
		return chunkPoint;
	}

	public void AddChunkPoint(ChunkPoint point)
	{
		if (point == null)
		{
			return;
		}
		point.worldPosition.y = 0f;
		if (chunkPoints == null)
		{
			chunkPoints = new Dictionary<ChunkPointType, List<ChunkPoint>>();
		}
		if (point.rule == null)
		{
			if (point.spawner == null)
			{
				return;
			}
			point.rule = GetRuleFromPointType(point.spawner.ChunkPointType);
			if (point.rule == null)
			{
				return;
			}
		}
		ChunkPointType chunkPointType = point.rule.ChunkPointType;
		if (chunkPoints.ContainsKey(chunkPointType))
		{
			chunkPoints[chunkPointType].Add(point);
			return;
		}
		List<ChunkPoint> list = new List<ChunkPoint>();
		list.Add(point);
		chunkPoints.Add(chunkPointType, list);
	}

	public ChunkPoint AddRGDChunkPoint(RGD_ChunkPoint rgdPoint)
	{
		if (rgdPoint == null)
		{
			return null;
		}
		SO_ChunkSpawnRuleAsset ruleFromPointType = GetRuleFromPointType(rgdPoint.chunkPointType);
		if (ruleFromPointType == null)
		{
			return null;
		}
		ChunkPoint chunkPoint = new ChunkPoint();
		chunkPoint.rule = ruleFromPointType;
		chunkPoint.worldPosition = rgdPoint.worldPosition.Value;
		chunkPoint.IsAddedByForce = rgdPoint.isAddedByForce;
		chunkPoint.spawner = GetSpawner((uint)rgdPoint.spawnerIndex);
		if (rgdPoint.spawnableUniqueIndex != -1 && chunkPoint.spawner != null)
		{
			chunkPoint.spawnableFloatingObject = chunkPoint.spawner.GetSpawnableFromIndex((uint)rgdPoint.spawnableUniqueIndex);
		}
		chunkPoint.poolDuplicateIndex = -1;
		if (rgdPoint.hasSpawnedObject && chunkPoint.spawner != null)
		{
			chunkPoint.spawnedObject = chunkPoint.spawner.GetSpawnedObjectFromObjectIndex(rgdPoint.spawnedObjectIndex);
			if (chunkPoint.spawnedObject != null)
			{
				chunkPoint.poolDuplicateIndex = chunkPoint.spawnedObject.poolDuplicateIndex;
				chunkPoint.spawnedObject.gameObject.SetActiveSafe(rgdPoint.visible);
				if (Raft_Network.IsHost && SaveAndLoad.IsGameLoading)
				{
					float num = chunkPoint.spawnedObject.transform.position.DistanceXZ(chunkPoint.worldPosition);
					if (num > 1f)
					{
						Debug.Log("Fixed position of landmark(" + chunkPoint.spawnedObject.transform.name + ") because distance was to great compared to it's chunkpoint: " + num);
						chunkPoint.spawnedObject.transform.position = chunkPoint.worldPosition;
					}
				}
			}
		}
		if (rgdPoint.hasSpawnedObject && (chunkPoint.spawner == null || chunkPoint.spawnedObject == null))
		{
			return null;
		}
		AddChunkPoint(chunkPoint);
		return chunkPoint;
	}

	public void ClearAllChunkPoints()
	{
		if (chunkPoints == null)
		{
			return;
		}
		foreach (List<ChunkPoint> value in chunkPoints.Values)
		{
			for (int i = 0; i < value.Count; i++)
			{
				if (value[i] != null)
				{
					value[i].OnDestroyed();
				}
			}
		}
		chunkPoints.Clear();
		isDirty = true;
	}

	public void ClearChunkPoints(ChunkPointType pointType, uint safeZoneRadiusFromRaft = 0u)
	{
		if (chunkPoints == null)
		{
			return;
		}
		if (chunkPoints.ContainsKey(pointType))
		{
			foreach (ChunkPoint item in chunkPoints[pointType])
			{
				if (safeZoneRadiusFromRaft == 0 || !(RaftTransform.position.DistanceXZ(item.worldPosition) < (float)safeZoneRadiusFromRaft))
				{
					item.OnDestroyed();
				}
			}
			if (!chunkPoints[pointType].ContainsItems())
			{
				chunkPoints.Remove(pointType);
			}
		}
		isDirty = true;
	}

	public ObjectSpawner_Chunk GetSpawner(uint spawnerUniqueIndex)
	{
		if (!allChunkSpawners.ContainsItems())
		{
			return null;
		}
		return allChunkSpawners.Find((ObjectSpawner_Chunk s) => s.UniqueIndex == spawnerUniqueIndex);
	}

	public ObjectSpawner_Chunk GetSpawner(ChunkPointType pointType, BiomeZone biomeZone)
	{
		if (!allChunkSpawners.ContainsItems())
		{
			return null;
		}
		BiomeZone.BiomeZoneIdentifier biomeZoneID = biomeZone?.zoneID ?? BiomeZone.BiomeZoneIdentifier.BIOME_NONE;
		ObjectSpawner_Chunk objectSpawner_Chunk = allChunkSpawners.Find((ObjectSpawner_Chunk s) => s.ChunkPointType == pointType && s.BiomeZoneID == biomeZoneID);
		if (objectSpawner_Chunk == null)
		{
			return GetSpawner(pointType);
		}
		return objectSpawner_Chunk;
	}

	public ObjectSpawner_Chunk GetSpawner(ChunkPointType pointType)
	{
		if (!allChunkSpawners.ContainsItems())
		{
			return null;
		}
		return allChunkSpawners.Find((ObjectSpawner_Chunk s) => s.ChunkPointType == pointType);
	}

	public List<ChunkPoint> GetChunkPointsFromPointType(ChunkPointType pointType)
	{
		if (chunkPoints == null)
		{
			return null;
		}
		if (chunkPoints.ContainsKey(pointType))
		{
			return chunkPoints[pointType];
		}
		return null;
	}

	public Dictionary<ChunkPointType, List<ChunkPoint>> GetAllChunkPointsDictionary()
	{
		return chunkPoints;
	}

	public List<ChunkPoint> GetAllChunkPointsList()
	{
		List<ChunkPoint> list = new List<ChunkPoint>();
		foreach (List<ChunkPoint> value in chunkPoints.Values)
		{
			if (value.ContainsItems())
			{
				list.AddRange(value);
			}
		}
		return list;
	}

	public SO_ChunkSpawnRuleAsset GetRuleFromPointType(ChunkPointType pointType)
	{
		for (int i = 0; i < allChunkPointRules.Length; i++)
		{
			if (allChunkPointRules[i].ChunkPointType == pointType)
			{
				return allChunkPointRules[i];
			}
		}
		return null;
	}

	public bool DoesLineIntersectWithChunkPoints(Vector3 lineStart, Vector3 lineEnd)
	{
		foreach (List<ChunkPoint> value in chunkPoints.Values)
		{
			foreach (ChunkPoint item in value)
			{
				if (item != null && !(item.rule == null) && Helper.IsLineIntersectingCircle(lineStart.XZOnly(), lineEnd.XZOnly(), item.worldPosition.XZOnly(), item.rule.CollisionOverlapRadius * 1.3f))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void SortSpawnersByPriority()
	{
		if (currentSpawners.ContainsItems())
		{
			currentSpawners = currentSpawners.OrderBy((ObjectSpawner_Chunk s) => s.Priority).ToList();
			currentSpawners.Reverse();
		}
	}

	private void InitializeChunks()
	{
		chunks = new Chunk[numberOfChunks.x, numberOfChunks.y];
		for (int i = 0; i < numberOfChunks.y; i++)
		{
			for (int j = 0; j < numberOfChunks.x; j++)
			{
				Chunk chunk = new Chunk();
				chunk.size = ChunkSize;
				chunk.worldPosition = BottomLeftCorner + Vector3.right * (j * ChunkSize) + Vector3.forward * (i * ChunkSize);
				chunks[j, i] = chunk;
			}
		}
	}

	public void GenerateChunkDataStartOfGame()
	{
		for (int i = 0; i < numberOfChunks.y; i++)
		{
			for (int j = 0; j < numberOfChunks.x; j++)
			{
				if (!emptyAtStartXInterval.IsInsideOfIntervalInclusive(j) || !emptyAtStartYInterval.IsInsideOfIntervalInclusive(i))
				{
					Chunk chunk = chunks[j, i];
					GenerateChunkData(chunk);
				}
			}
		}
		AddChunkPointCheat(ChunkPointType.Landmark_Small, new Vector3(0f, 0f, 500f));
	}

	public void GenerateChunkDataAllChunks()
	{
		Chunk[,] array = chunks;
		foreach (Chunk chunk in array)
		{
			if (chunk != null)
			{
				GenerateChunkData(chunk);
			}
		}
		isDirty = true;
	}

	private List<ChunkPoint> GenerateChunkData(int chunkX, int chunkY)
	{
		if (!ChunkIndexWithinRange(chunkX, chunkY))
		{
			return null;
		}
		Chunk chunk = chunks[chunkX, chunkY];
		if (chunk == null)
		{
			return null;
		}
		return GenerateChunkData(chunk);
	}

	private List<ChunkPoint> GenerateChunkData(Chunk chunk)
	{
		if (chunk == null)
		{
			return null;
		}
		if (!currentSpawners.ContainsItems())
		{
			return null;
		}
		List<ChunkPoint> list = new List<ChunkPoint>();
		for (int i = 0; i < currentSpawners.Count; i++)
		{
			ObjectSpawner_Chunk objectSpawner_Chunk = currentSpawners[i];
			if (!(objectSpawner_Chunk == null))
			{
				List<ChunkPoint> list2 = GenerateChunkData(chunk, objectSpawner_Chunk.ChunkPointType);
				if (list2.ContainsItems())
				{
					list.AddRange(list2);
				}
			}
		}
		return list;
	}

	private List<ChunkPoint> GenerateChunkData(Chunk chunk, ChunkPointType pointType)
	{
		if (chunk == null)
		{
			return null;
		}
		SO_ChunkSpawnRuleAsset ruleFromPointType = GetRuleFromPointType(pointType);
		List<ChunkPoint> list = new List<ChunkPoint>();
		int num = ((ruleFromPointType == null) ? 1 : ruleFromPointType.SpawnsPerChunk.GetRandomValue());
		for (int i = 0; i < num; i++)
		{
			if (ruleFromPointType.TestSpawnChance())
			{
				ChunkPoint chunkPoint = new ChunkPoint();
				chunkPoint.worldPosition = chunk.GetRandomPoint();
				chunkPoint.rule = ruleFromPointType;
				if (DoesPointFit(chunkPoint))
				{
					list.Add(chunkPoint);
					AddChunkPoint(chunkPoint);
				}
			}
		}
		return list;
	}

	private bool DoesPointFit(ChunkPoint pointToCheck)
	{
		if (pointToCheck == null)
		{
			return false;
		}
		if (chunkPoints == null)
		{
			return false;
		}
		Vector3 worldPosition = pointToCheck.worldPosition;
		float collisionOverlapRadius = pointToCheck.rule.CollisionOverlapRadius;
		foreach (List<ChunkPoint> value in chunkPoints.Values)
		{
			for (int i = 0; i < value.Count; i++)
			{
				ChunkPoint chunkPoint = value[i];
				if (chunkPoint != null && chunkPoint != pointToCheck)
				{
					float num = worldPosition.DistanceXZ(chunkPoint.worldPosition);
					float collisionOverlapRadius2 = chunkPoint.rule.CollisionOverlapRadius;
					float num2 = collisionOverlapRadius + collisionOverlapRadius2;
					if (num <= num2)
					{
						return false;
					}
					if (!pointToCheck.rule.DoesPointPassSpawnRules(pointToCheck, chunkPoint))
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	private bool ChunkIndexWithinRange(int xIndex, int yIndex)
	{
		if (chunks == null)
		{
			return false;
		}
		if (xIndex >= 0 && xIndex < chunks.GetLength(0) && yIndex >= 0)
		{
			return yIndex < chunks.GetLength(1);
		}
		return false;
	}

	private Chunk GetChunkFromWorldPosition(Vector3 worldPosition)
	{
		if (numberOfChunks == Vector2.zero)
		{
			return null;
		}
		Vector3 vector = worldPosition + new Vector3((float)TotalChunkWidth / 2f, 0f, (float)TotalChunkHeight / 2f);
		int num = Mathf.FloorToInt(vector.x / (float)ChunkSize);
		int num2 = Mathf.FloorToInt(vector.z / (float)ChunkSize);
		if (ChunkIndexWithinRange(num, num2))
		{
			return chunks[num, num2];
		}
		return null;
	}

	private void SendMessageRefreshPoints()
	{
		if (Raft_Network.IsHost)
		{
			Message_ChunkManager_ChunkPoints message = new Message_ChunkManager_ChunkPoints(Messages.ChunkManager_RefreshChunkPoints, network.NetworkIDManager, ObjectIndex, GetAllChunkPointsList());
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.ChunkManager_Create:
		{
			Message_ChunkManager_ChunkPoints message_ChunkManager_ChunkPoints2 = msg as Message_ChunkManager_ChunkPoints;
			ObjectIndex = message_ChunkManager_ChunkPoints2.NetworkID_ObjectIndex;
			if (message_ChunkManager_ChunkPoints2.rgdChunkPoints.ContainsItems())
			{
				for (int j = 0; j < message_ChunkManager_ChunkPoints2.rgdChunkPoints.Length; j++)
				{
					AddRGDChunkPoint(message_ChunkManager_ChunkPoints2.rgdChunkPoints[j]);
				}
			}
			return false;
		}
		case Messages.ChunkManager_ClearChunkPoints:
			ClearAllChunkPoints();
			return false;
		case Messages.ChunkManager_RefreshChunkPoints:
		{
			chunkPoints.Clear();
			Message_ChunkManager_ChunkPoints message_ChunkManager_ChunkPoints = msg as Message_ChunkManager_ChunkPoints;
			if (message_ChunkManager_ChunkPoints.rgdChunkPoints.ContainsItems())
			{
				for (int i = 0; i < message_ChunkManager_ChunkPoints.rgdChunkPoints.Length; i++)
				{
					AddRGDChunkPoint(message_ChunkManager_ChunkPoints.rgdChunkPoints[i]);
				}
			}
			return false;
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
