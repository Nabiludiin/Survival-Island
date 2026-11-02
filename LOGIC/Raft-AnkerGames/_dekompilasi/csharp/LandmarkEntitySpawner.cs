using System;
using System.Collections;
using UnityEngine;

public class LandmarkEntitySpawner : MonoBehaviour
{
	public Color gizmoColor = Color.magenta;

	public uint spawnerIndex;

	public AI_NetworkBehaviourType[] spawnabeEntityTypes;

	public bool setStartRotation;

	[SerializeField]
	private bool spawnOnLandmarkReset = true;

	public WaypointHandler waypointHandler;

	public Transform referencePoint;

	[HideInInspector]
	public AI_NetworkBehaviourType entityType = AI_NetworkBehaviourType.None;

	private bool shouldSpawn;

	public Landmark landmark;

	protected Network_Host_Entities networkHostEntities;

	public AI_NetworkBehaviour spawnedEntityBehaviour;

	[HideInInspector]
	public bool ShouldSpawn
	{
		get
		{
			return shouldSpawn;
		}
		set
		{
			shouldSpawn = value;
		}
	}

	public virtual void AwakeCall()
	{
		SetRandomEntityType();
		this.landmark = GetComponentInParent<Landmark>();
		if (this.landmark == null)
		{
			Debug.LogError("Can't find landmark, " + gameObject.name);
		}
		else if (Raft_Network.IsHost)
		{
			Landmark landmark = this.landmark;
			landmark.OnLandmarkReset = (Action)Delegate.Combine(landmark.OnLandmarkReset, new Action(OnLandmarkReset));
			Landmark landmark2 = this.landmark;
			landmark2.OnLandmarkRemoved = (Action)Delegate.Combine(landmark2.OnLandmarkRemoved, new Action(OnLandmarkRemoved));
		}
	}

	protected virtual void Start()
	{
		networkHostEntities = ComponentManager<Network_Host_Entities>.Value;
	}

	private void OnDestroy()
	{
		landmark = null;
	}

	protected virtual void OnDisable()
	{
		if (spawnedEntityBehaviour != null)
		{
			spawnedEntityBehaviour.gameObject.SetActiveSafe(value: false);
		}
	}

	protected virtual void OnEnable()
	{
		if (spawnedEntityBehaviour != null)
		{
			spawnedEntityBehaviour.gameObject.SetActiveSafe(value: true);
		}
	}

	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.5f);
		Gizmos.DrawSphere(transform.position, 1f);
	}

	private void OnLandmarkReset()
	{
		if (Raft_Network.IsHost && !SaveAndLoad.IsGameLoading)
		{
			SetRandomEntityType();
			if (spawnOnLandmarkReset)
			{
				TryToSpawnEntity();
			}
		}
	}

	private void OnLandmarkRemoved()
	{
		if (Raft_Network.IsHost && spawnedEntityBehaviour != null)
		{
			NetworkIDManager.SendIDBehaviourDead(spawnedEntityBehaviour.ObjectIndex, typeof(AI_NetworkBehaviour), removeInstantly: true);
		}
	}

	public void Restore(RGD_LandmarkSpawner rgdSpawner)
	{
		if (rgdSpawner != null && rgdSpawner.spawnedEntityData != null)
		{
			SingletonGeneric<GameManager>.Singleton.StartCoroutine(RestoreLate(rgdSpawner));
		}
	}

	protected virtual IEnumerator RestoreLate(RGD_LandmarkSpawner rgdSpawner)
	{
		yield return new WaitForSeconds(0.1f);
		if (networkHostEntities == null)
		{
			networkHostEntities = ComponentManager<Network_Host_Entities>.Value;
		}
		entityType = rgdSpawner.spawnedEntityData.networkBehaviourType;
		RGD_AI_NetworkBehaviour spawnedEntityData = rgdSpawner.spawnedEntityData;
		spawnedEntityBehaviour = networkHostEntities.CreateAINetworkBehaviour(spawnedEntityData.networkBehaviourType, spawnedEntityData.position.Value, 0f - spawnedEntityData.scaleSize, spawnedEntityData.aiObjectIndex, spawnedEntityData.aiEntityObjectIndex, this);
		if (spawnedEntityBehaviour != null)
		{
			spawnedEntityData.RestoreAINetworkBehaviour(spawnedEntityBehaviour);
		}
	}

	public void DisconnectCurrentEntityFromSpawner()
	{
		if (spawnedEntityBehaviour != null)
		{
			spawnedEntityBehaviour.connectedSpawner = null;
			spawnedEntityBehaviour = null;
		}
	}

	public virtual void OnEntityConnectedToSpawner()
	{
	}

	public void ManuallySpawn()
	{
		CreateEntity(forceShouldSpawn: true);
	}

	protected bool HasSpawnedEntity()
	{
		if (spawnedEntityBehaviour != null && spawnedEntityBehaviour.behaviourType == AI_NetworkBehaviourType.ButlerBot)
		{
			return true;
		}
		if (spawnedEntityBehaviour != null)
		{
			return !spawnedEntityBehaviour.networkEntity.IsDead;
		}
		return false;
	}

	private void SpawnFake()
	{
		SetRandomEntityType();
		CreateEntity();
	}

	public void SetRandomEntityType()
	{
		if (spawnabeEntityTypes == null || spawnabeEntityTypes.Length == 0)
		{
			Debug.Log("spawnable entity types are not set in: " + transform.name + transform.root.name);
			return;
		}
		int num = UnityEngine.Random.Range(0, spawnabeEntityTypes.Length);
		entityType = spawnabeEntityTypes[num];
	}

	private bool ShouldEntityBeSpawnedInGamemode(SO_GameModeValue p_gameMode, AI_NetworkBehaviourType p_type)
	{
		bool result = true;
		switch (p_type)
		{
		case AI_NetworkBehaviourType.StoneBird:
			if (!p_gameMode.stonebirdVariables.shouldSpawn)
			{
				result = false;
			}
			break;
		case AI_NetworkBehaviourType.PufferFish:
			if (!p_gameMode.pufferfishVariables.shouldSpawn)
			{
				result = false;
			}
			break;
		}
		return result;
	}

	protected virtual AI_NetworkBehaviour CreateEntity(bool forceShouldSpawn = false)
	{
		if (!forceShouldSpawn && !ShouldSpawn)
		{
			return null;
		}
		if (!ShouldEntityBeSpawnedInGamemode(GameModeValueManager.GetCurrentGameModeValue(), entityType))
		{
			return null;
		}
		if (networkHostEntities == null)
		{
			networkHostEntities = ComponentManager<Network_Host_Entities>.Value;
		}
		if (networkHostEntities != null)
		{
			spawnedEntityBehaviour = networkHostEntities.CreateAINetworkBehaviour(entityType, transform.position, this);
			return spawnedEntityBehaviour;
		}
		return null;
	}

	protected virtual void TryToSpawnEntity()
	{
		CreateEntity();
	}
}
