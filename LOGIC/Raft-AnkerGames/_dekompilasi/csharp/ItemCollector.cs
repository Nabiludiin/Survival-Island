using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class ItemCollector : MonoBehaviour_ID_Network, IBlockPlaced
{
	public Action OnItemCollect;

	public bool playerControlled = true;

	public LayerMask collectMask;

	[SerializeField]
	private Collider collectorCollider;

	[SerializeField]
	private int maxNumberOfItems;

	public EventReference er_collect;

	public bool hapticEvent;

	private Vector3 origSize;

	public List<PickupItem_Networked> collectedItems = new List<PickupItem_Networked>();

	private Raft_Network network;

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		if (playerControlled)
		{
			origSize = ((BoxCollider)collectorCollider).size;
		}
	}

	private void Update()
	{
		if (playerControlled)
		{
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				((BoxCollider)collectorCollider).size = origSize * 1.25f;
			}
			else
			{
				((BoxCollider)collectorCollider).size = origSize;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!collectorCollider.enabled || (maxNumberOfItems != 0 && collectedItems.Count >= maxNumberOfItems) || !Helper.ObjectIsOnLayer(other.transform.gameObject, collectMask))
		{
			return;
		}
		PickupItem_Networked componentInParent = other.transform.GetComponentInParent<PickupItem_Networked>();
		if (!(componentInParent != null) || componentInParent.PickupItem.requiresHands)
		{
			return;
		}
		Message_ItemCollectorCollect message = new Message_ItemCollectorCollect(Messages.ItemCollectorCollect, network.NetworkIDManager, componentInParent, ObjectIndex);
		if (playerControlled)
		{
			if (Raft_Network.IsHost)
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				CollectItem(componentInParent);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else if (Raft_Network.IsHost)
		{
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			CollectItem(componentInParent);
		}
	}

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		if (!playerControlled)
		{
			NetworkIDManager.AddNetworkID(this);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (collectedItems.Count > 0)
		{
			ClearCollectedItems(null);
		}
		if (!playerControlled)
		{
			NetworkIDManager.RemoveNetworkID(this);
		}
	}

	public void AddCollectedItemsToPlayer(Network_Player collectingPlayer)
	{
		if (collectedItems.Count != 0)
		{
			if (collectingPlayer.IsLocalPlayer && playerControlled)
			{
				AchievementHandler.AddPlayerHookCount(collectedItems.Count);
			}
			Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.ItemCollectorClear, network.NetworkIDManager, ObjectIndex, collectingPlayer.steamID);
			if (Raft_Network.IsHost)
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				ClearCollectedItems(collectingPlayer);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public bool CollectItem(PickupItem_Networked item, bool playSound = true)
	{
		if (item == null)
		{
			return false;
		}
		WaterFloatSemih2 component = item.GetComponent<WaterFloatSemih2>();
		if (component == null || !component.enabled)
		{
			return false;
		}
		component.enabled = false;
		if (item.spawner != null && item.spawner.spawnedObjects.Contains(item))
		{
			if (item.spawner.OnRemoveSpawn != null)
			{
				item.spawner.OnRemoveSpawn(item.gameObject);
			}
			item.spawner.spawnedObjects.Remove(item);
		}
		Collider[] componentsInChildren = item.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		item.transform.SetParent(transform);
		item.transform.localPosition = Vector3.zero;
		collectedItems.Add(item);
		if (playSound && !er_collect.IsNull)
		{
			if (hapticEvent)
			{
				SimpleMonoBehaviourSingleton<HapticManager>.Instance?.PlayOneShot(HapticEvent.HookHit);
			}
			RuntimeManager.PlayOneShot(er_collect, transform.position);
		}
		if (OnItemCollect != null)
		{
			OnItemCollect();
		}
		return true;
	}

	private bool ClearCollectedItems(Network_Player collectingPlayer)
	{
		if (!collectedItems.ContainsItems())
		{
			return false;
		}
		foreach (PickupItem_Networked collectedItem in collectedItems)
		{
			if (collectedItem == null)
			{
				continue;
			}
			Collider[] componentsInChildren = collectedItem.GetComponentsInChildren<Collider>();
			if (componentsInChildren != null)
			{
				Collider[] array = componentsInChildren;
				foreach (Collider collider in array)
				{
					if (!(collider == null))
					{
						collider.enabled = true;
					}
				}
			}
			WaterFloatSemih2 component = collectedItem.GetComponent<WaterFloatSemih2>();
			if (component != null)
			{
				component.enabled = true;
			}
			if (collectingPlayer == null)
			{
				PickupObjectManager.RemovePickupItem(collectedItem);
			}
			else
			{
				PickupObjectManager.RemovePickupItem(collectedItem, collectingPlayer.steamID);
			}
		}
		if (collectedItems != null)
		{
			collectedItems.Clear();
		}
		return true;
	}

	public void RestoreFloatingItemsInCollector(RGD_FloatingItem[] collectedItems)
	{
		if (playerControlled)
		{
			return;
		}
		ObjectSpawnerManager value = ComponentManager<ObjectSpawnerManager>.Value;
		foreach (RGD_FloatingItem rgdItem in collectedItems)
		{
			ObjectSpawner objectSpawner = value.spawners.Find((ObjectSpawner s) => s.UniqueIndex == rgdItem.spawnerIndex);
			if (!(objectSpawner == null))
			{
				SpawnableFloatingObject spawnableFromIndex = objectSpawner.GetSpawnableFromIndex(rgdItem.uniqueSpawnableIndex);
				PickupItem_Networked pickupItem_Networked = objectSpawner.SpawnItem(rgdItem.Position.Value, rgdItem.Rotation.Value, spawnableFromIndex, rgdItem.floatingObjectIndex, rgdItem.behaviourIndex, rgdItem.poolDuplicateIndex);
				if (pickupItem_Networked != null)
				{
					CollectItem(pickupItem_Networked, playSound: false);
				}
			}
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		switch (msg.Type)
		{
		case Messages.ItemCollectorCollect:
		{
			PickupItem_Networked networkIDFromObjectIndex = NetworkIDManager.GetNetworkIDFromObjectIndex<PickupItem_Networked>((msg as Message_ItemCollectorCollect).pickupNetworkObjectIndex);
			if (networkIDFromObjectIndex != null)
			{
				return CollectItem(networkIDFromObjectIndex);
			}
			return false;
		}
		case Messages.ItemCollectorClear:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			if (network != null)
			{
				Network_Player playerFromID = network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID);
				return ClearCollectedItems(playerFromID);
			}
			return false;
		}
		default:
			return false;
		}
	}
}
