using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class Network_Host_Entities : MonoBehaviour_ID_Network
{
	[SerializeField]
	private AI_NetworkBehaviour[] AINetworkBehaviourPrefabs;

	[SerializeField]
	private Seagull seagullPrefab;

	[SerializeField]
	private bool spawnShark = true;

	[SerializeField]
	private int seagullMaxCount = 2;

	public bool spawnSeagulls = true;

	[HideInInspector]
	public List<RGD_AI_NetworkBehaviour> rgdAINetworkBehaviour = new List<RGD_AI_NetworkBehaviour>();

	private Raft_Network network;

	private int sharkCount;

	private int seagullCount;

	public int SharkCount
	{
		get
		{
			return sharkCount;
		}
		set
		{
			sharkCount = value;
		}
	}

	public int SeagullCount
	{
		get
		{
			return seagullCount;
		}
		set
		{
			seagullCount = value;
		}
	}

	private void Awake()
	{
		ComponentManager<Network_Host_Entities>.Value = this;
		if (Raft_Network.IsHost && !GameManager.IsInNewGame)
		{
			SaveAndLoad.LoadComplete = (Action)Delegate.Combine(SaveAndLoad.LoadComplete, new Action(CheckSharkRespawnLateStarter));
		}
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		if (Raft_Network.IsHost)
		{
			SaveAndLoad.LoadRGDAINetworkBehaviours = (Action)Delegate.Combine(SaveAndLoad.LoadRGDAINetworkBehaviours, new Action(OnLoadRGDAINetworkBehaviors));
			if (GameManager.IsInNewGame)
			{
				ObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
				NetworkIDManager.AddNetworkID(this);
			}
			if ((GameManager.IsInNewGame || GameManager.QuickStartGame) && spawnShark)
			{
				CreateAINetworkBehaviour(AI_NetworkBehaviourType.Shark, GetSharkSpawnPosition());
			}
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		SaveAndLoad.LoadComplete = (Action)Delegate.Remove(SaveAndLoad.LoadComplete, new Action(CheckSharkRespawnLateStarter));
	}

	private void OnLoadRGDAINetworkBehaviors()
	{
		StartCoroutine(LoadRGDAINetworkBehaviorsLate());
	}

	private IEnumerator LoadRGDAINetworkBehaviorsLate()
	{
		yield return new WaitForSeconds(0.1f);
		for (int i = 0; i < rgdAINetworkBehaviour.Count; i++)
		{
			RGD_AI_NetworkBehaviour rGD_AI_NetworkBehaviour = rgdAINetworkBehaviour[i];
			if (!(rGD_AI_NetworkBehaviour == null))
			{
				AI_NetworkBehaviour aI_NetworkBehaviour = CreateAINetworkBehaviour(rGD_AI_NetworkBehaviour.networkBehaviourType, rGD_AI_NetworkBehaviour.position.Value, rGD_AI_NetworkBehaviour.scaleSize, rGD_AI_NetworkBehaviour.aiObjectIndex, rGD_AI_NetworkBehaviour.aiEntityObjectIndex);
				if (aI_NetworkBehaviour != null)
				{
					rGD_AI_NetworkBehaviour.RestoreAINetworkBehaviour(aI_NetworkBehaviour);
				}
			}
		}
		rgdAINetworkBehaviour.Clear();
	}

	public Seagull CreateSeagull(Vector3 position, uint entityObjectIndex, uint objectIndex, uint behaviourIndex)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (!GameModeValueManager.GetCurrentGameModeValue().seagullVariables.shouldSpawn)
		{
			return null;
		}
		if (seagullCount >= seagullMaxCount)
		{
			return null;
		}
		Seagull seagull = UnityEngine.Object.Instantiate(seagullPrefab, position, Quaternion.identity);
		seagull.ObjectIndex = objectIndex;
		seagull.BehaviourIndex = behaviourIndex;
		seagull.entityComponent.ObjectIndex = entityObjectIndex;
		seagull.pickupItem.networkID.ObjectIndex = seagull.ObjectIndex;
		NetworkIDManager.AddNetworkID(seagull.pickupItem.networkID);
		NetworkIDManager.AddNetworkIDTick(seagull);
		NetworkIDManager.AddNetworkID(seagull);
		NetworkIDManager.AddNetworkID(seagull.entityComponent);
		if (Raft_Network.IsHost)
		{
			network.RPC(new Message_Seagull_Create(Messages.Seagull_Create, ComponentManager<NetworkIDManager>.Value, ObjectIndex, seagull), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		return seagull;
	}

	public IEnumerator CreateShark(float timeDelay, Vector3 position, AI_NetworkBehaviourType type)
	{
		yield return new WaitForSeconds(timeDelay);
		if (Raft_Network.IsHost)
		{
			CreateAINetworkBehaviour(type, GetSharkSpawnPosition());
		}
	}

	public AI_NetworkBehaviour CreateAINetworkBehaviour(AI_NetworkBehaviourType behaviourType, Vector3 position, LandmarkEntitySpawner spawner = null)
	{
		if (Raft_Network.IsHost)
		{
			AI_NetworkBehaviour aI_NetworkBehaviour = CreateAINetworkBehaviour(behaviourType, position, -1f, SaveAndLoad.GetUniqueObjectIndex(), SaveAndLoad.GetUniqueObjectIndex(), spawner);
			if (aI_NetworkBehaviour == null)
			{
				return null;
			}
			Message_CreateAINetworkBehaviour message = new Message_CreateAINetworkBehaviour(Messages.CreateAINetworkBehaviour, network.NetworkIDManager, ObjectIndex, position, aI_NetworkBehaviour, spawner);
			if (network == null)
			{
				network = ComponentManager<Raft_Network>.Value;
			}
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			return aI_NetworkBehaviour;
		}
		return null;
	}

	public AI_NetworkBehaviour CreateAINetworkBehaviour(AI_NetworkBehaviourType behaviourType, Vector3 position, float scaleSize, uint objectIndex, uint entityObjectIndex, LandmarkEntitySpawner spawner = null)
	{
		AI_NetworkBehaviour networkBehaviourPrefabFromType = GetNetworkBehaviourPrefabFromType(behaviourType);
		if (networkBehaviourPrefabFromType == null)
		{
			Debug.LogError("Could not find a networkbehaviour with type " + behaviourType.ToString() + " in network entity list", gameObject);
			return null;
		}
		AI_NetworkBehaviour aI_NetworkBehaviour = UnityEngine.Object.Instantiate(networkBehaviourPrefabFromType, position, Quaternion.identity);
		if (spawner != null && spawner.setStartRotation)
		{
			aI_NetworkBehaviour.transform.rotation = spawner.transform.rotation;
		}
		aI_NetworkBehaviour.SetNetworkIndexes(objectIndex, entityObjectIndex);
		aI_NetworkBehaviour.Initialize(position, (scaleSize < 0f) ? aI_NetworkBehaviour.localScaleInterval.GetRandomValue() : scaleSize);
		aI_NetworkBehaviour.connectedSpawner = spawner;
		if (!Raft_Network.IsHost && spawner != null)
		{
			spawner.spawnedEntityBehaviour = aI_NetworkBehaviour;
		}
		if ((bool)aI_NetworkBehaviour.connectedSpawner)
		{
			aI_NetworkBehaviour.connectedSpawner.OnEntityConnectedToSpawner();
		}
		NetworkIDManager.AddNetworkIDTick(aI_NetworkBehaviour);
		NetworkIDManager.AddNetworkID(aI_NetworkBehaviour, typeof(AI_NetworkBehaviour));
		NetworkIDManager.AddNetworkID(aI_NetworkBehaviour.networkEntity);
		return aI_NetworkBehaviour;
	}

	private void CheckSharkRespawnLateStarter()
	{
		StartCoroutine(CheckSharkRespawnLate(0.2f));
	}

	private IEnumerator CheckSharkRespawnLate(float delay)
	{
		yield return new WaitForSeconds(delay);
		CheckSharkRespawn();
	}

	private void CheckSharkRespawn()
	{
		if (spawnShark)
		{
			AI_NetworkBehavior_Shark[] array = UnityEngine.Object.FindObjectsOfType<AI_NetworkBehavior_Shark>();
			if (array == null || array.Length == 0)
			{
				CreateAINetworkBehaviour(AI_NetworkBehaviourType.Shark, GetSharkSpawnPosition());
			}
		}
	}

	private AI_NetworkBehaviour GetNetworkBehaviourPrefabFromType(AI_NetworkBehaviourType behaviourType)
	{
		if (AINetworkBehaviourPrefabs == null)
		{
			return null;
		}
		for (int i = 0; i < AINetworkBehaviourPrefabs.Length; i++)
		{
			AI_NetworkBehaviour aI_NetworkBehaviour = AINetworkBehaviourPrefabs[i];
			if (aI_NetworkBehaviour == null)
			{
				Debug.LogError("AI_NetworkBehaviour is null in Network_Host_Entities prefab list");
			}
			else if (aI_NetworkBehaviour.behaviourType == behaviourType)
			{
				return aI_NetworkBehaviour;
			}
		}
		return null;
	}

	private LandmarkEntitySpawner GetSpawnerFromMessage(Message_CreateAINetworkBehaviour msg)
	{
		if (msg.uniqueLandmarkIndex < 0 || msg.uniqueSpawnerIndex < 0)
		{
			return null;
		}
		for (int i = 0; i < WorldManager.AllLandmarks.Count; i++)
		{
			Landmark landmark = WorldManager.AllLandmarks[i];
			if (landmark.uniqueLandmarkIndex != msg.uniqueLandmarkIndex)
			{
				continue;
			}
			for (int j = 0; j < landmark.spawners.Length; j++)
			{
				if (landmark.spawners[j].spawnerIndex == msg.uniqueSpawnerIndex)
				{
					return landmark.spawners[j];
				}
			}
		}
		return null;
	}

	public Vector3 GetSharkSpawnPosition()
	{
		return ((SingletonGeneric<GameManager>.Singleton != null) ? SingletonGeneric<GameManager>.Singleton.lockedPivot.position : Vector3.zero) + Vector3.down * 10f + new Vector3(UnityEngine.Random.insideUnitCircle.x, 0f, UnityEngine.Random.insideUnitCircle.y) * 10f;
	}

	public bool GetSpawnPositionDontCollideWithChunkPoint(ref Vector3 spawnPosition, float radiusFromRaft, int maxCheckIterations = 10)
	{
		ChunkManager value = ComponentManager<ChunkManager>.Value;
		spawnPosition = Vector3.zero;
		if (value != null)
		{
			Vector3 vector = ((SingletonGeneric<GameManager>.Singleton != null) ? SingletonGeneric<GameManager>.Singleton.lockedPivot.position : Vector3.zero);
			int num = 0;
			do
			{
				spawnPosition = vector + UnityEngine.Random.insideUnitSphere.XZOnly().normalized * radiusFromRaft;
				spawnPosition.y = -30f;
				num++;
				if (num >= maxCheckIterations)
				{
					return false;
				}
			}
			while (value.DoesLineIntersectWithChunkPoints(spawnPosition, vector));
			return true;
		}
		return false;
	}

	public bool GetBirdpackSpawnPosition(ref Vector3 spawnPosition)
	{
		ChunkManager value = ComponentManager<ChunkManager>.Value;
		spawnPosition = Vector3.zero;
		if (value != null)
		{
			Vector3 vector = ((SingletonGeneric<GameManager>.Singleton != null) ? SingletonGeneric<GameManager>.Singleton.lockedPivot.position : Vector3.zero);
			spawnPosition = vector + UnityEngine.Random.insideUnitSphere.XZOnly().normalized * 300f;
			spawnPosition.y = 50f;
			return true;
		}
		return false;
	}

	public override RGD Serialize_Save()
	{
		return new RGD_NetworkBehaviour(RGDType.NetworkBehaviour_NetworkEntities, this);
	}

	public override void Serialize_CreateFromIDManager(List<Message_NetworkBehaviour> msgs)
	{
		msgs.Add(new Message_NetworkBehaviour_ID(Messages.CreateHostNetworkEntities, network.NetworkIDManager, ObjectIndex));
		Seagull[] array = UnityEngine.Object.FindObjectsOfType<Seagull>();
		foreach (Seagull seagull in array)
		{
			msgs.Add(new Message_Seagull_Create(Messages.Seagull_Create, network.NetworkIDManager, ObjectIndex, seagull));
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.CreateAINetworkBehaviour:
		{
			Message_CreateAINetworkBehaviour message_CreateAINetworkBehaviour = msg as Message_CreateAINetworkBehaviour;
			LandmarkEntitySpawner spawnerFromMessage = GetSpawnerFromMessage(message_CreateAINetworkBehaviour);
			AI_NetworkBehaviour aI_NetworkBehaviour = CreateAINetworkBehaviour(message_CreateAINetworkBehaviour.behaviourType, message_CreateAINetworkBehaviour.Position.Value, message_CreateAINetworkBehaviour.scaledSize, message_CreateAINetworkBehaviour.behaviourObjectIndex, message_CreateAINetworkBehaviour.behaviourEntityObjectIndex, spawnerFromMessage);
			if (aI_NetworkBehaviour != null)
			{
				aI_NetworkBehaviour.networkEntity.stat_health.Value = message_CreateAINetworkBehaviour.entityHealth;
			}
			return false;
		}
		case Messages.Seagull_Create:
		{
			Message_Seagull_Create message_Seagull_Create = msg as Message_Seagull_Create;
			CreateSeagull(message_Seagull_Create.Position, message_Seagull_Create.seagullEntityObjectIndex, message_Seagull_Create.seagullObjectIndex, message_Seagull_Create.seagullBehaviourIndex);
			return false;
		}
		case Messages.DamageEntity:
		{
			Message_NetworkEntity_Damage message_NetworkEntity_Damage = msg as Message_NetworkEntity_Damage;
			Network_Entity networkIDFromObjectIndex = NetworkIDManager.GetNetworkIDFromObjectIndex<Network_Entity>(message_NetworkEntity_Damage.entityObjectIndex);
			if (networkIDFromObjectIndex != null)
			{
				SO_Buff buffFromIndex = BuffManager.GetBuffFromIndex(message_NetworkEntity_Damage.buffAssetIndex);
				networkIDFromObjectIndex.Damage(message_NetworkEntity_Damage.damage, message_NetworkEntity_Damage.HitPosition, message_NetworkEntity_Damage.HitNormal, message_NetworkEntity_Damage.damageInflictorEntityType, isLocal: false, buffFromIndex);
				return true;
			}
			return false;
		}
		default:
			return false;
		}
	}
}
