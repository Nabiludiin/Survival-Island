using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class ObjectSpawner_RaftDirection : ObjectSpawner
{
	public static List<ObjectSpawnerNoSpawnZone> NoSpawnZones = new List<ObjectSpawnerNoSpawnZone>();

	public SO_ObjectSpawner spawnSettingsAsset;

	private ObjectSpawnerAssetSettings currentSettings;

	[Tooltip("How many objects can the spawner have in total. (-1 means infinite)")]
	public int maxSpawnedObjects = -1;

	public float spawnDistanceFromRaft = 15f;

	public float removeDistanceFromRaft = 15f;

	public bool removeItemsAutomaticly;

	public SpawnPosition[] spawnPositions;

	private Vector3 spawnDirectionFromRaft;

	private float spawnTimer;

	private float spawnDelay;

	protected override void Start()
	{
		base.Start();
		currentSettings = spawnSettingsAsset.GetSettings(1);
		if (currentSettings != null)
		{
			spawnDelay = currentSettings.spawnRateInterval.GetRandomValue();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		spawnDirectionFromRaft = Raft.direction.normalized;
		if (spawnDirectionFromRaft.magnitude < 1f)
		{
			return;
		}
		if (!currentSettings.PlayerCountWithinRange(network.PlayerCount))
		{
			currentSettings = spawnSettingsAsset.GetSettings(network.PlayerCount);
		}
		if ((maxSpawnedObjects == -1 || spawnedObjects.Count < maxSpawnedObjects) && spawnDelay > 0f)
		{
			spawnTimer += Time.deltaTime;
			if (spawnTimer >= spawnDelay)
			{
				spawnTimer = 0f;
				spawnDelay = currentSettings.spawnRateInterval.GetRandomValue();
				SpawnNewItems();
			}
		}
		if (removeItemsAutomaticly)
		{
			RemoveItemsOutsideRange();
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (spawnPositions == null || spawnPositions.Length == 0)
		{
			return;
		}
		Vector3 vector = transform.position + Raft.direction.normalized * spawnDistanceFromRaft;
		Vector3 vector2 = Vector3.Cross(Raft.direction, Vector3.up);
		for (int i = 0; i < spawnPositions.Length; i++)
		{
			SpawnPosition spawnPosition = spawnPositions[i];
			if (spawnPosition != null)
			{
				Vector3 center = vector + vector2 * spawnPosition.horizontalOffset;
				Gizmos.color = Color.green;
				Gizmos.DrawWireCube(center, new Vector3(spawnPosition.horizontalSize, 1f, spawnPosition.verticalSize) * 2f);
			}
		}
	}

	public void SpawnNewItems()
	{
		if (!gameObject.activeInHierarchy)
		{
			return;
		}
		if (objectSpawnerManager == null)
		{
			objectSpawnerManager = ComponentManager<ObjectSpawnerManager>.Value;
		}
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (!Raft_Network.IsHost)
		{
			return;
		}
		Vector3 normalized = Raft.direction.normalized;
		Vector3 vector = transform.position + normalized * spawnDistanceFromRaft;
		int randomValue = currentSettings.spawnAmount.GetRandomValue();
		for (int i = 0; i < randomValue; i++)
		{
			SpawnableFloatingObject objectToSpawn = GetObjectToSpawn();
			if (objectToSpawn == null)
			{
				i--;
				continue;
			}
			Vector3 vector2 = new Vector3(Random.Range(objectToSpawn.minRotation.x, objectToSpawn.maxRotation.x), Random.Range(objectToSpawn.minRotation.y, objectToSpawn.maxRotation.y), Random.Range(objectToSpawn.minRotation.z, objectToSpawn.maxRotation.z));
			Vector3 vector3 = vector;
			SpawnPosition randomSpawnPosition = GetRandomSpawnPosition();
			if (randomSpawnPosition != null)
			{
				Vector3 vector4 = Vector3.Cross(Raft.direction, Vector3.up);
				vector3 += vector4.normalized * randomSpawnPosition.horizontalOffset + vector4.normalized * randomSpawnPosition.RandomHorizontalSize;
				vector3 += normalized * randomSpawnPosition.RandomVerticalSize;
			}
			if (!IsSpawnPositionValid(vector3))
			{
				continue;
			}
			uint num = (setIndexOnSpawn ? SaveAndLoad.GetUniqueObjectIndex() : 0u);
			uint num2 = ((objectToSpawn.hasNetworkBehaviour && setIndexOnSpawn) ? NetworkUpdateManager.GetUniqueBehaviourIndex() : 0u);
			int poolDuplicateIndex = ((objectToSpawn.poolDuplicateCount > 1) ? Random.Range(0, objectToSpawn.poolDuplicateCount) : 0);
			if (objectToSpawn.typeToSpawn == ObjectSpawnType.AssetBundleScene)
			{
				AsyncedLandmark randomLandmarkFromAssetBundleKey = assetBundleHooker.GetRandomLandmarkFromAssetBundleKey(objectToSpawn.assetBundleKey);
				if (randomLandmarkFromAssetBundleKey == null)
				{
					Debug.Log("Asynced Landmark is null :(");
					break;
				}
				poolDuplicateIndex = (int)randomLandmarkFromAssetBundleKey.landmark.uniqueLandmarkIndex;
			}
			PickupItem_Networked pickupItem_Networked = SpawnItem(vector3, vector2, objectToSpawn, num, num2, poolDuplicateIndex);
			if (!setIndexOnSpawn)
			{
				num = pickupItem_Networked.ObjectIndex;
				if (objectToSpawn.hasNetworkBehaviour)
				{
					num2 = pickupItem_Networked.networkBehaviour.BehaviourIndex;
				}
			}
			network.RPC(new Message_ObjectSpawner_SpawnItem(Messages.ObjectSpawner_SpawnItem, objectSpawnerManager, vector3, vector2, UniqueIndex, num, num2, objectToSpawn.uniqueIndex, poolDuplicateIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private bool IsSpawnPositionValid(Vector3 position)
	{
		if (NoSpawnZones == null || NoSpawnZones.Count == 0)
		{
			return true;
		}
		for (int i = 0; i < NoSpawnZones.Count; i++)
		{
			ObjectSpawnerNoSpawnZone objectSpawnerNoSpawnZone = NoSpawnZones[i];
			if (objectSpawnerNoSpawnZone == null)
			{
				NoSpawnZones.RemoveAt(i);
				i--;
			}
			else if (objectSpawnerNoSpawnZone.transform.position.DistanceXZ(position) <= objectSpawnerNoSpawnZone.radius)
			{
				return false;
			}
		}
		return true;
	}

	private void RemoveItemsOutsideRange()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		for (int i = 0; i < spawnedObjects.Count; i++)
		{
			PickupItem_Networked pickupItem_Networked = spawnedObjects[i];
			Vector3 position = pickupItem_Networked.transform.position;
			Vector3 position2 = transform.position;
			position.y = (position2.y = 0f);
			if (Vector3.Distance(position, position2) >= removeDistanceFromRaft)
			{
				PickupObjectManager.RemovePickupItemNetwork(pickupItem_Networked);
				i--;
			}
		}
	}

	private SpawnPosition GetRandomSpawnPosition()
	{
		if (spawnPositions == null || spawnPositions.Length == 0)
		{
			return null;
		}
		SpawnPosition spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];
		if (spawnPosition != null)
		{
			return spawnPosition;
		}
		return null;
	}
}
