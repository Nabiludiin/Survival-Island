using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class TreasurePointManager : MonoBehaviour_ID_Network, SceneEventInterface
{
	public bool drawGizmoTreasurePoints = true;

	public SO_TreasureLootSettings[] treasureLootSettings;

	public Dictionary<Transform, List<TreasurePoint>> treasurePoints = new Dictionary<Transform, List<TreasurePoint>>();

	private Raft_Network network;

	private void Awake()
	{
		ComponentManager<TreasurePointManager>.Value = this;
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		treasureLootSettings = Resources.LoadAll<SO_TreasureLootSettings>("SO_TreasureLootSettings");
	}

	public void RemoveTreasurePointsViaTransformReference(Transform trans)
	{
		treasurePoints.TryGetValue(trans, out var value);
		if (value != null)
		{
			foreach (TreasurePoint item in value)
			{
				if (item != null)
				{
					item.RemoveSelf();
				}
			}
		}
		if (treasurePoints != null)
		{
			treasurePoints.Remove(trans);
		}
	}

	public Transform GetParentForTreasure(int parentObjectIndex)
	{
		if (parentObjectIndex < 0)
		{
			return null;
		}
		Landmark landmark = WorldManager.AllLandmarks.Find((Landmark l) => l.uniqueLandmarkIndex == parentObjectIndex);
		if (landmark != null)
		{
			return landmark.transform;
		}
		MonoBehaviour_ID_Network networkIDFromObjectIndex = NetworkIDManager.GetNetworkIDFromObjectIndex((uint)parentObjectIndex);
		if (networkIDFromObjectIndex != null)
		{
			return networkIDFromObjectIndex.transform;
		}
		return null;
	}

	public List<TreasurePoint> GetTreasurePointsInAreaTo(Vector3 point, float maxDistanceFromPoint, bool includeUnBuried, bool includeUnrevealed)
	{
		List<TreasurePoint> list = new List<TreasurePoint>();
		foreach (Transform key in treasurePoints.Keys)
		{
			if (!key.gameObject.activeInHierarchy)
			{
				continue;
			}
			treasurePoints.TryGetValue(key, out var value);
			if (value == null || value.Count <= 0)
			{
				continue;
			}
			foreach (TreasurePoint item in value)
			{
				if (!(item == null) && item.gameObject.activeInHierarchy && (includeUnBuried || item.IsBuried) && (includeUnrevealed || item.isRevealedViaDetector) && Vector3.Distance(point, item.transform.position) <= maxDistanceFromPoint)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	public TreasurePoint GetClosestTreasurePointTo(Vector3 point, float maxDistanceFromPoint, bool includeUnBuried, bool includeUnrevealed, out float distanceToPoint)
	{
		TreasurePoint result = null;
		float num = float.MaxValue;
		foreach (Transform key in treasurePoints.Keys)
		{
			if (!key.gameObject.activeInHierarchy)
			{
				continue;
			}
			treasurePoints.TryGetValue(key, out var value);
			if (value == null || value.Count <= 0)
			{
				continue;
			}
			foreach (TreasurePoint item in value)
			{
				if (!(item == null) && item.gameObject.activeInHierarchy && (includeUnBuried || item.IsBuried) && (includeUnrevealed || item.isRevealedViaDetector))
				{
					float num2 = Vector3.Distance(point, item.transform.position);
					if (num2 <= num && num2 <= maxDistanceFromPoint)
					{
						result = item;
						num = num2;
					}
				}
			}
		}
		distanceToPoint = num;
		return result;
	}

	public TreasurePoint AddTreasurePointNetworked(Transform treasureParent, Vector3 pos, int uniqueTreasureIndex)
	{
		TreasurePoint treasurePoint = null;
		if (Raft_Network.IsHost)
		{
			treasurePoint = AddTreasurePoint(SaveAndLoad.GetUniqueObjectIndex(), SaveAndLoad.GetUniqueObjectIndex(), treasureParent, pos, uniqueTreasureIndex);
			if (treasurePoint != null)
			{
				Message_TreasurePointManager_Spawn message = new Message_TreasurePointManager_Spawn(Messages.TreasurePointManager_Spawn, network.NetworkIDManager, ObjectIndex, treasurePoint);
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else if (!Raft_Network.IsHost)
		{
			Debug.Log("Client should not reach here.");
		}
		return treasurePoint;
	}

	public TreasurePoint AddTreasurePoint(uint objectIndex, uint objectIndexPickup, Transform treasureParent, Vector3 pos, int uniqueTreasureIndex)
	{
		if (treasureParent == null)
		{
			treasureParent = transform;
		}
		TreasurePoint treasurePoint = null;
		if (Raft_Network.IsHost && uniqueTreasureIndex < 0)
		{
			treasurePoint = GetRandomTresurePrefab();
			if (treasurePoint != null)
			{
				uniqueTreasureIndex = treasurePoint.uniqueTreasureIndex;
			}
		}
		else
		{
			treasurePoint = GetTreasurePrefabFromUniqueIndex(uniqueTreasureIndex);
		}
		if (treasurePoint == null)
		{
			Debug.Log("Randomized Treasure Point is null :(, not adding point");
			return null;
		}
		TreasurePoint treasurePoint2 = Object.Instantiate(treasurePoint, pos, Quaternion.identity, treasureParent);
		treasurePoint2.transform.localPosition = pos;
		treasurePoint2.Initialize(objectIndex, objectIndexPickup, treasureParent);
		if (treasurePoints.ContainsKey(treasureParent))
		{
			treasurePoints.TryGetValue(treasureParent, out var value);
			if (value != null)
			{
				value.Add(treasurePoint2);
			}
			else
			{
				Debug.LogError("List is null in dictionary for transform " + treasureParent.name + " in treasurepointdictionairy");
			}
		}
		else
		{
			treasurePoints.Add(treasureParent, new List<TreasurePoint> { treasurePoint2 });
		}
		return treasurePoint2;
	}

	private TreasurePoint GetRandomTresurePrefab()
	{
		if (!treasureLootSettings.ContainsItems())
		{
			return null;
		}
		SO_TreasureLootSettings sO_TreasureLootSettings = treasureLootSettings[Random.Range(0, treasureLootSettings.Length)];
		if (sO_TreasureLootSettings == null)
		{
			return null;
		}
		return sO_TreasureLootSettings.lootTable.GetRandomItem<TreasurePoint>();
	}

	private TreasurePoint GetTreasurePrefabFromUniqueIndex(int uniqueTreasureIndex)
	{
		if (!treasureLootSettings.ContainsItems())
		{
			return null;
		}
		SO_TreasureLootSettings[] array = treasureLootSettings;
		foreach (SO_TreasureLootSettings sO_TreasureLootSettings in array)
		{
			if (sO_TreasureLootSettings == null)
			{
				continue;
			}
			TreasurePoint[] allItems = sO_TreasureLootSettings.lootTable.GetAllItems<TreasurePoint>();
			if (!treasureLootSettings.ContainsItems())
			{
				continue;
			}
			TreasurePoint[] array2 = allItems;
			foreach (TreasurePoint treasurePoint in array2)
			{
				if (treasurePoint.uniqueTreasureIndex == uniqueTreasureIndex)
				{
					return treasurePoint;
				}
			}
		}
		return null;
	}

	private void ClearTreasurePoints()
	{
		treasurePoints.Clear();
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			treasurePoints.Clear();
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.TreasurePointManager_Spawn:
		{
			Message_TreasurePointManager_Spawn message_TreasurePointManager_Spawn = msg as Message_TreasurePointManager_Spawn;
			if (message_TreasurePointManager_Spawn != null)
			{
				if (!Raft_Network.IsHost)
				{
					AddTreasurePoint(message_TreasurePointManager_Spawn.tpObjectIndex, message_TreasurePointManager_Spawn.tpPickupObjectIndex, GetParentForTreasure(message_TreasurePointManager_Spawn.tpParentObjectIndex), message_TreasurePointManager_Spawn.position, message_TreasurePointManager_Spawn.uniqueTreasureIndex);
					return false;
				}
				return false;
			}
			break;
		}
		case Messages.TreasurePointManager_Create:
		{
			Message_TreasurePoint_Create message_TreasurePoint_Create = msg as Message_TreasurePoint_Create;
			if (message_TreasurePoint_Create != null)
			{
				if (message_TreasurePoint_Create.rgd_treasurePoints.ContainsItems())
				{
					RGD_TreasurePoint_Networked[] rgd_treasurePoints = message_TreasurePoint_Create.rgd_treasurePoints;
					for (int i = 0; i < rgd_treasurePoints.Length; i++)
					{
						rgd_treasurePoints[i].RestoreTreasurePoint(this);
					}
				}
			}
			else
			{
				Debug.Log("msg create for treasurepoints resulted in null");
			}
			return false;
		}
		}
		return base.Deserialize(msg, remoteID);
	}
}
