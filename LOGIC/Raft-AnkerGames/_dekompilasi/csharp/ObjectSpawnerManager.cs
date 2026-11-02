using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectSpawnerManager : MonoBehaviour_Network, SceneEventInterface
{
	[HideInInspector]
	public List<ObjectSpawner> spawners = new List<ObjectSpawner>();

	[HideInInspector]
	public Transform floatingObjectParent;

	public ObjectSpawner_RaftDirection plankSpawner;

	public ObjectSpawner_RaftDirection itemSpawner;

	private Raft_Network network;

	private Raft raft;

	private void Awake()
	{
		ComponentManager<ObjectSpawnerManager>.Value = this;
		spawners = GetComponentsInChildren<ObjectSpawner>().ToList();
	}

	private void Start()
	{
		floatingObjectParent = new GameObject("FloatingObjectParent").transform;
		raft = ComponentManager<Raft>.Value;
		network = ComponentManager<Raft_Network>.Value;
		if (Raft_Network.IsHost && GameManager.IsInNewGame)
		{
			NetworkUpdateManager.SetIndexForBehaviour(this);
			NetworkUpdateManager.AddBehaviour(this);
		}
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	private void Update()
	{
		bool flag = raft.Moving && !raft.IsAnchored && GameModeValueManager.GetCurrentGameModeValue().enviromentalVariables.spawnObjects;
		for (int i = 0; i < spawners.Count; i++)
		{
			if (spawners[i] == null)
			{
				spawners.RemoveAt(i);
				i--;
			}
			else
			{
				spawners[i].enabled = flag;
			}
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame && ObjectSpawner_RaftDirection.NoSpawnZones != null)
		{
			ObjectSpawner_RaftDirection.NoSpawnZones.Clear();
		}
	}

	public void ClearAllSpawners(bool clearUsingNetwork)
	{
		foreach (ObjectSpawner spawner in spawners)
		{
			spawner.ClearItems(clearUsingNetwork);
		}
	}

	public void PrewarmStart()
	{
		if (plankSpawner == null || itemSpawner == null || floatingObjectParent == null)
		{
			Debug.Log("Aborting PrewarmStart()");
			return;
		}
		float spawnDistanceFromRaft = plankSpawner.spawnDistanceFromRaft;
		plankSpawner.SpawnNewItems();
		itemSpawner.SpawnNewItems();
		floatingObjectParent.position -= Vector3.forward * (spawnDistanceFromRaft / 2f);
		plankSpawner.SpawnNewItems();
		itemSpawner.SpawnNewItems();
		floatingObjectParent.position -= Vector3.forward * (spawnDistanceFromRaft / 2f);
		plankSpawner.SpawnNewItems();
		itemSpawner.SpawnNewItems();
		Helper.LogBuild("PrewarmStart() complete");
	}

	private void OnWorldShift(Vector3 shift)
	{
		foreach (ObjectSpawner spawner in spawners)
		{
			foreach (PickupItem_Networked spawnedObject in spawner.spawnedObjects)
			{
				if (spawnedObject.transform.parent == floatingObjectParent)
				{
					spawnedObject.transform.position -= shift;
				}
			}
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_NetworkBehaviour(RGDType.NetworkBehaviour_ObjectSpawnerManager, this);
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_SpawnerManager_Create(Messages.ObjectSpawner_Create, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.ObjectSpawner_Create:
		{
			Message_SpawnerManager_Create message_SpawnerManager_Create = msg as Message_SpawnerManager_Create;
			if (message_SpawnerManager_Create != null && message_SpawnerManager_Create.floatingItems != null)
			{
				RGD_FloatingItem[] floatingItems = message_SpawnerManager_Create.floatingItems;
				foreach (RGD_FloatingItem rgdItem in floatingItems)
				{
					ObjectSpawner objectSpawner2 = spawners.Find((ObjectSpawner s) => s.UniqueIndex == rgdItem.spawnerIndex);
					if (!(objectSpawner2 == null))
					{
						SpawnableFloatingObject spawnableFromIndex = objectSpawner2.GetSpawnableFromIndex(rgdItem.uniqueSpawnableIndex);
						objectSpawner2.SpawnItem(rgdItem.Position.Value, rgdItem.Rotation.Value, spawnableFromIndex, rgdItem.floatingObjectIndex, rgdItem.behaviourIndex, rgdItem.poolDuplicateIndex);
					}
				}
			}
			return false;
		}
		case Messages.ObjectSpawner_SpawnItem:
		{
			Message_ObjectSpawner_SpawnItem msgSpawn = msg as Message_ObjectSpawner_SpawnItem;
			ObjectSpawner objectSpawner = spawners.Find((ObjectSpawner s) => s.UniqueIndex == msgSpawn.uniqueSpawnerIndex);
			objectSpawner.SpawnItem(msgSpawn.SpawnPosition, msgSpawn.SpawnRotation, objectSpawner.GetSpawnableFromIndex(msgSpawn.uniqueSpawnableIndex), msgSpawn.spawnedObjectIndex, msgSpawn.spawnedBehaviourIndex, msgSpawn.poolDuplicateIndex);
			return false;
		}
		default:
			return false;
		}
	}
}
