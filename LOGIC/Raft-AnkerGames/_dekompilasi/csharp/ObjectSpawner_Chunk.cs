using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner_Chunk : ObjectSpawner
{
	[SerializeField]
	private ChunkPointType chunkPointType;

	[SerializeField]
	private BiomeZone.BiomeZoneIdentifier biomeZoneID;

	[SerializeField]
	private int priority = 1;

	public ChunkPointType ChunkPointType => chunkPointType;

	public BiomeZone.BiomeZoneIdentifier BiomeZoneID => biomeZoneID;

	public int Priority
	{
		get
		{
			return priority;
		}
		private set
		{
			priority = value;
		}
	}

	public int GetRandomUnusedPoolDuplicateIndexFromSpawnable(SpawnableFloatingObject spawnableObject, List<ChunkPoint> excludeChunkPoints)
	{
		if (spawnableObject == null)
		{
			return -1;
		}
		if (spawnableObject.typeToSpawn == ObjectSpawnType.AssetBundleScene && !string.IsNullOrEmpty(spawnableObject.assetBundleKey))
		{
			List<AsyncedLandmark> landmarksFromAssetBundleKey = assetBundleHooker.GetLandmarksFromAssetBundleKey(spawnableObject.assetBundleKey);
			if (landmarksFromAssetBundleKey == null)
			{
				return -1;
			}
			List<int> list = new List<int>();
			foreach (ChunkPoint excludeChunkPoint in excludeChunkPoints)
			{
				if (excludeChunkPoint != null && excludeChunkPoint.poolDuplicateIndex >= 0)
				{
					list.Add(excludeChunkPoint.poolDuplicateIndex);
				}
			}
			List<int> list2 = new List<int>();
			for (int i = 0; i < landmarksFromAssetBundleKey.Count; i++)
			{
				AsyncedLandmark asyncedLandmark = landmarksFromAssetBundleKey[i];
				if (asyncedLandmark != null && !(asyncedLandmark.landmark == null) && !list.Contains((int)asyncedLandmark.landmark.uniqueLandmarkIndex))
				{
					list2.Add((int)asyncedLandmark.landmark.uniqueLandmarkIndex);
				}
			}
			if (list2.ContainsItems())
			{
				return list2[Random.Range(0, list2.Count)];
			}
		}
		else if (spawnableObject.typeToSpawn == ObjectSpawnType.Addressable)
		{
			List<int> list3 = new List<int>();
			foreach (ChunkPoint excludeChunkPoint2 in excludeChunkPoints)
			{
				if (excludeChunkPoint2 != null && excludeChunkPoint2.poolDuplicateIndex >= 0)
				{
					list3.Add(excludeChunkPoint2.poolDuplicateIndex);
				}
			}
			List<int> list4 = new List<int>();
			List<SceneLoader.LandmarkData> landmarksFromAssetBundleKey2 = sceneLoader.GetLandmarksFromAssetBundleKey(spawnableObject.assetBundleKey);
			if (landmarksFromAssetBundleKey2.ContainsItems())
			{
				foreach (SceneLoader.LandmarkData item in landmarksFromAssetBundleKey2)
				{
					if (!list3.Contains((int)item.index))
					{
						list4.Add((int)item.index);
					}
				}
			}
			if (list4.ContainsItems())
			{
				return list4[Random.Range(0, list4.Count)];
			}
		}
		return -1;
	}
}
