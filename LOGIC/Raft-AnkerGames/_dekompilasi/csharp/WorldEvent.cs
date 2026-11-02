using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldEvent
{
	public Action OnEventStopped;

	public SO_WorldEvent eventData;

	public List<MonoBehaviour_Network> spawnedEntites = new List<MonoBehaviour_Network>();

	public bool isActive;

	public float inactivityProgress;

	public float progress;

	private WorldEventManager handler;

	public WorldEvent(SO_WorldEvent data, WorldEventManager handler)
	{
		this.handler = handler;
		eventData = data;
		eventData.InitHandle(this);
	}

	public virtual void StartEvent()
	{
		isActive = true;
		progress = 0f;
		inactivityProgress = 0f;
	}

	public virtual bool UpdateEvent()
	{
		if (!isActive)
		{
			inactivityProgress += Time.deltaTime;
			return false;
		}
		progress += Time.deltaTime;
		return true;
	}

	public virtual void StopEvent()
	{
		isActive = false;
		progress = 0f;
		inactivityProgress = 0f;
		OnEventStopped?.Invoke();
		spawnedEntites.Clear();
	}

	public void AddSpawnedEntity(AI_NetworkBehaviour behaviour)
	{
		if (!spawnedEntites.Contains(behaviour))
		{
			spawnedEntites.Add(behaviour);
			TrackBehaviour(behaviour);
		}
	}

	public void RemoveSpawnedEntity(AI_NetworkBehaviour behaviour)
	{
		if (spawnedEntites.Contains(behaviour))
		{
			spawnedEntites.Remove(behaviour);
		}
		if (isActive && spawnedEntites.Count <= 0)
		{
			handler.StopEvent(this);
		}
	}

	public void RestoreWorldEvent(RGD_WorldEvent rgdworldEvent)
	{
		isActive = rgdworldEvent.isActive;
		progress = rgdworldEvent.progress;
		inactivityProgress = rgdworldEvent.inactivityProgress;
		if (rgdworldEvent.spawnedEntitiesObjectIndexes != null)
		{
			uint[] spawnedEntitiesObjectIndexes = rgdworldEvent.spawnedEntitiesObjectIndexes;
			for (int i = 0; i < spawnedEntitiesObjectIndexes.Length; i++)
			{
				MonoBehaviour_ID_Network networkIDFromObjectIndex = NetworkIDManager.GetNetworkIDFromObjectIndex(spawnedEntitiesObjectIndexes[i]);
				spawnedEntites.Add(networkIDFromObjectIndex as MonoBehaviour_Network);
				TrackBehaviour(networkIDFromObjectIndex as AI_NetworkBehaviour);
			}
		}
	}

	public RGD_WorldEvent GetData()
	{
		return new RGD_WorldEvent(this);
	}

	protected AI_NetworkBehaviour SpawnEntity(AI_NetworkBehaviourType type, Vector3 pos)
	{
		AI_NetworkBehaviour aI_NetworkBehaviour = WorldEventManager.networkHostEntities.CreateAINetworkBehaviour(type, pos);
		AddSpawnedEntity(aI_NetworkBehaviour);
		return aI_NetworkBehaviour;
	}

	protected void TrackBehaviour(AI_NetworkBehaviour behaviour)
	{
		WorldEvent_Tracker_AI worldEvent_Tracker_AI = behaviour.gameObject.GetComponent<WorldEvent_Tracker_AI>();
		if (worldEvent_Tracker_AI == null)
		{
			worldEvent_Tracker_AI = behaviour.gameObject.AddComponent<WorldEvent_Tracker_AI>();
		}
		worldEvent_Tracker_AI.InitTracker(this);
	}
}
