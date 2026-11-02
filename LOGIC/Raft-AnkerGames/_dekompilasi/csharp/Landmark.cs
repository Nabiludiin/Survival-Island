using System;
using System.Collections.Generic;
using UnityEngine;

public class Landmark : MonoBehaviour
{
	public Action OnLandmarkReset;

	public Action OnLandmarkRemoved;

	public ValidTerrainPointGenerator terrainPointGenerator;

	public SO_TreasureLootSettings treasureLootSetting;

	private TreasurePointManager treasurePointManager;

	public uint uniqueLandmarkIndex;

	[HideInInspector]
	public LandmarkItem[] landmarkItems;

	[HideInInspector]
	public LandmarkEntitySpawner[] spawners;

	[SerializeField]
	public bool isSpawned;

	public bool containsNonResetableLandmarkItem;

	private bool initialized;

	public void Initialize()
	{
		if (initialized)
		{
			return;
		}
		initialized = true;
		treasurePointManager = ComponentManager<TreasurePointManager>.Value;
		List<LandmarkItem> result = null;
		transform.GetComponentsInChildrenRecursively(ref result);
		if (result.ContainsItems())
		{
			landmarkItems = result.ToArray();
			LandmarkItem[] array = landmarkItems;
			foreach (LandmarkItem landmarkItem in array)
			{
				if (landmarkItem != null)
				{
					landmarkItem.localLandmarkPosition = Helper.GetLocalPositionComparedToRoot(transform, landmarkItem.transform);
				}
			}
		}
		spawners = transform.GetComponentsInChildren<LandmarkEntitySpawner>(includeInactive: true);
		if (Raft_Network.IsHost && GameManager.IsInNewGame)
		{
			AssignItemIndexes();
		}
	}

	public void OnSpawn()
	{
		isSpawned = true;
		ResetPickups();
	}

	public void OnRemove()
	{
		isSpawned = false;
		if (OnLandmarkRemoved != null)
		{
			OnLandmarkRemoved();
		}
		if (treasurePointManager != null && transform != null)
		{
			treasurePointManager.RemoveTreasurePointsViaTransformReference(transform);
		}
	}

	public void ResetPickups()
	{
		if (!initialized)
		{
			Initialize();
		}
		SendOnLandmarkReset();
		if (Raft_Network.IsHost)
		{
			GenerateTreasures();
		}
		SendMessage("OnLandmarkReset", SendMessageOptions.DontRequireReceiver);
		if (landmarkItems != null)
		{
			LandmarkItem[] array = landmarkItems;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnLandmarkReset();
			}
		}
	}

	public void SendOnLandmarkReset()
	{
		OnLandmarkReset?.Invoke();
	}

	public LandmarkItem GetLandmarkItemByLocalPosition(Vector3 localPosition)
	{
		if (!initialized)
		{
			Initialize();
		}
		if (landmarkItems != null)
		{
			float num = 0.01f;
			LandmarkItem[] array = landmarkItems;
			foreach (LandmarkItem landmarkItem in array)
			{
				Vector3 localPositionComparedToRoot = Helper.GetLocalPositionComparedToRoot(transform, landmarkItem.transform);
				if (Vector3.Distance(localPosition, localPositionComparedToRoot) <= num)
				{
					return landmarkItem;
				}
			}
		}
		return null;
	}

	public void AssignItemIndexes()
	{
		for (int i = 0; i < landmarkItems.Length; i++)
		{
			LandmarkItem landmarkItem = landmarkItems[i];
			if (!(landmarkItem == null) && landmarkItem.connectedBehaviourID != null)
			{
				landmarkItem.AssignIndexAndTrack(SaveAndLoad.GetUniqueObjectIndex());
			}
		}
	}

	private void GenerateTreasures()
	{
		if (treasureLootSetting == null)
		{
			return;
		}
		if (treasurePointManager == null)
		{
			treasurePointManager = ComponentManager<TreasurePointManager>.Value;
		}
		if (SaveAndLoad.IsGameLoading)
		{
			return;
		}
		if (treasurePointManager != null && terrainPointGenerator != null)
		{
			foreach (Vector3 validPoint in terrainPointGenerator.GetValidPoints(treasureLootSetting.numberOfTreasures.GetRandomValue()))
			{
				TreasurePoint randomItem = treasureLootSetting.lootTable.GetRandomItem<TreasurePoint>();
				if (randomItem != null)
				{
					treasurePointManager.AddTreasurePointNetworked(transform, validPoint, randomItem.uniqueTreasureIndex);
				}
			}
			return;
		}
		Debug.Log("TreasurePointmanager or terrainPointGenerator data is null");
	}
}
