using System;
using UnityEngine;

public class AI_NetworkBehaviour : MonoBehaviour_NetworkTick
{
	public Action OnTransformScaleModified;

	public AI_NetworkBehaviourType behaviourType;

	public Network_Entity networkEntity;

	public Interval_Float localScaleInterval = new Interval_Float(1f, 1f);

	public float scaledSize = 1f;

	[HideInInspector]
	public Raft_Network network;

	public LandmarkEntitySpawner connectedSpawner;

	public bool hasBeenLoaded;

	public PickupChanneling pickupItemChanneling;

	public bool ConnectedToSpawner => connectedSpawner != null;

	[HideInInspector]
	public Raft_Network Network
	{
		get
		{
			if (network == null)
			{
				network = ComponentManager<Raft_Network>.Value;
				if (network == null)
				{
					network = UnityEngine.Object.FindObjectOfType<Raft_Network>();
				}
			}
			return network;
		}
	}

	protected virtual void Awake()
	{
		if (pickupItemChanneling == null)
		{
			pickupItemChanneling = GetComponent<PickupChanneling>();
			_ = pickupItemChanneling == null;
		}
	}

	protected virtual void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	protected override void OnDestroy()
	{
		NetworkIDManager.RemoveNetworkTickID(this);
		NetworkIDManager.RemoveNetworkID(this, typeof(AI_NetworkBehaviour));
		NetworkIDManager.RemoveNetworkID(networkEntity);
	}

	protected virtual void Update()
	{
		if (Raft_Network.IsHost)
		{
			UpdateHost();
		}
		else
		{
			UpdateRemoteclient();
		}
	}

	public virtual void OnRestoreStart(RGD_AI_NetworkBehaviour rgdNetworkBehaviour)
	{
	}

	public virtual void OnRestoreFinished(RGD_AI_NetworkBehaviour rgdNetworkBehaviour)
	{
		if (rgdNetworkBehaviour.health <= 0 && pickupItemChanneling != null)
		{
			pickupItemChanneling.SetPickupColliderState(state: true);
		}
	}

	public virtual RGD_AI_NetworkBehaviour Create_SaveData()
	{
		return null;
	}

	public virtual void SetNetworkIndexes(uint objectIndex, uint entityObjectIndex)
	{
		ObjectIndex = objectIndex;
		networkEntity.ObjectIndex = entityObjectIndex;
	}

	public virtual void Initialize(Vector3 spawnPosition, float scaleSize)
	{
		scaledSize = scaleSize;
		transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
		OnTransformScaleModified?.Invoke();
	}

	protected virtual void UpdateHost()
	{
		UpdateNetworkTick();
	}

	protected virtual void UpdateRemoteclient()
	{
	}
}
