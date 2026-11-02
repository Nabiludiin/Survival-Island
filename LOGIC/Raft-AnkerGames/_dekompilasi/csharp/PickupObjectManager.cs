using Steamworks;
using UnityEngine;

public class PickupObjectManager : MonoBehaviour_Network, SceneEventInterface
{
	private static PoolManager poolManager;

	private static Raft_Network network;

	private static PickupObjectManager self;

	private static SoundManager soundManager;

	private static GameObject dropboxPrefabStatic;

	private static QuestItemManager questItemManager;

	[SerializeField]
	private GameObject dropboxPrefab;

	private void Awake()
	{
		dropboxPrefabStatic = dropboxPrefab;
		ComponentManager<PickupObjectManager>.Value = this;
	}

	private void Start()
	{
		self = this;
		poolManager = ComponentManager<PoolManager>.Value;
		network = ComponentManager<Raft_Network>.Value;
		soundManager = ComponentManager<SoundManager>.Value;
		questItemManager = ComponentManager<QuestItemManager>.Value;
		if (Raft_Network.IsHost && GameManager.IsInNewGame)
		{
			NetworkUpdateManager.SetIndexForBehaviour(this);
			NetworkUpdateManager.AddBehaviour(this);
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			poolManager = null;
			network = null;
			self = null;
			soundManager = null;
			questItemManager = null;
		}
	}

	public static void RemovePickupItemNetwork(PickupItem_Networked pickupNetwork)
	{
		if (self == null)
		{
			self = ComponentManager<PickupObjectManager>.Value;
		}
		if (self == null)
		{
			Debug.LogError("Could not find self in PickupObjectManager");
			return;
		}
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		Message_PickupObjectManager_RemoveItem message = new Message_PickupObjectManager_RemoveItem(Messages.RemoveItem, self, pickupNetwork.ObjectIndex);
		if (Raft_Network.IsHost)
		{
			if (RemovePickupItem(pickupNetwork))
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public static void RemovePickupItemNetwork(PickupItem_Networked pickupNetwork, Network_UserId pickupPlayerID)
	{
		if (self == null)
		{
			self = ComponentManager<PickupObjectManager>.Value;
		}
		Message_PickupObjectManager_RemoveItem message = new Message_PickupObjectManager_RemoveItem(Messages.RemoveItem, self, pickupPlayerID, pickupNetwork.ObjectIndex);
		if (Raft_Network.IsHost)
		{
			if (RemovePickupItem(pickupNetwork, pickupPlayerID))
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public static bool RemovePickupItem(PickupItem_Networked pickupNetwork, Network_UserId pickupPlayerID)
	{
		if (pickupNetwork == null)
		{
			return false;
		}
		if (Raft_Network.IsHost && !pickupNetwork.CanBePickedUp())
		{
			return false;
		}
		if (pickupPlayerID.IsValid())
		{
			Network_Player playerFromID = network.GetPlayerFromID(pickupPlayerID);
			if (playerFromID != null && playerFromID.IsLocalPlayer)
			{
				playerFromID.PickupScript.AddItemToInventory(pickupNetwork.PickupItem);
			}
		}
		return RemovePickupItem(pickupNetwork);
	}

	public static bool RemovePickupItem(PickupItem_Networked pickupNetwork)
	{
		if (pickupNetwork == null)
		{
			return false;
		}
		if (Raft_Network.IsHost && !pickupNetwork.CanBePickedUp())
		{
			return false;
		}
		if (pickupNetwork.stopTrackingOnPickup)
		{
			NetworkIDManager.RemoveNetworkID(pickupNetwork, typeof(PickupItem_Networked));
		}
		if (pickupNetwork.spawner != null)
		{
			if (pickupNetwork.spawner.OnRemoveSpawn != null)
			{
				pickupNetwork.spawner.OnRemoveSpawn(pickupNetwork.gameObject);
			}
			if (pickupNetwork.spawner.spawnedObjects.Contains(pickupNetwork))
			{
				pickupNetwork.spawner.spawnedObjects.Remove(pickupNetwork);
			}
		}
		if (pickupNetwork.networkBehaviour != null)
		{
			NetworkUpdateManager.SetBehaviourDead(pickupNetwork.networkBehaviour);
		}
		if (pickupNetwork.PickupItem != null && pickupNetwork.PickupItem.yieldHandler != null)
		{
			SO_ItemYield yieldAsset = pickupNetwork.PickupItem.yieldHandler.yieldAsset;
			if (yieldAsset != null && yieldAsset.yieldAssets.Count > 1)
			{
				pickupNetwork.PickupItem.yieldHandler.Yield.Clear();
			}
		}
		if (Raft_Network.IsHost && pickupNetwork != null && pickupNetwork.PickupItem != null && pickupNetwork.PickupItem.pickupItemType == PickupItemType.QuestItem)
		{
			QuestItemPickup questItemPickup = pickupNetwork.PickupItem as QuestItemPickup;
			if (questItemPickup != null)
			{
				if (questItemManager == null)
				{
					questItemManager = ComponentManager<QuestItemManager>.Value;
				}
				if (questItemManager != null)
				{
					questItemManager.AddQuestItemsNetworked(sendNotification: true, questItemPickup.questYield);
				}
			}
		}
		pickupNetwork.HandleOnPickedUp();
		if (pickupNetwork.OnPickedUp != null)
		{
			pickupNetwork.OnPickedUp();
		}
		return true;
	}

	public static void DropItem(ItemInstance item, Vector3 position, Vector3 dropDirection, uint dropObjectIndex, bool parentedToRaft)
	{
		if (item == null)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate(dropboxPrefabStatic, position, Quaternion.identity, parentedToRaft ? SingletonGeneric<GameManager>.Singleton.lockedPivot : null);
		soundManager.PlayUI_Drop();
		if (!(gameObject != null))
		{
			return;
		}
		PickupItem component = gameObject.GetComponent<PickupItem>();
		if (component != null)
		{
			component.transform.SetParentSafe(parentedToRaft ? SingletonGeneric<GameManager>.Singleton.lockedPivot : null);
			component.transform.position = position;
			component.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
			component.itemInstance = item;
			component.isDropped = true;
			component.networkID.ObjectIndex = dropObjectIndex;
			component.gameObject.SetActive(value: true);
			NetworkIDManager.AddNetworkID(component.networkID);
			Rigidbody component2 = component.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				component2.velocity = Vector3.zero;
				component2.AddForce(dropDirection * 100f + Vector3.up * 200f);
			}
		}
	}

	private static void SpawnDropItem(ItemInstance itemInstance, Vector3 position, Vector3 rotation, uint itemObjectIndex)
	{
		PickupItem component = Object.Instantiate(dropboxPrefabStatic, position, Quaternion.identity, SingletonGeneric<GameManager>.Singleton.lockedPivot).GetComponent<PickupItem>();
		component.transform.SetParentSafe(SingletonGeneric<GameManager>.Singleton.lockedPivot);
		component.transform.position = position;
		component.transform.eulerAngles = rotation;
		component.itemInstance = itemInstance;
		component.requiresHands = true;
		component.isDropped = true;
		component.networkID.ObjectIndex = itemObjectIndex;
		component.gameObject.SetActive(value: true);
		NetworkIDManager.AddNetworkID(component.networkID);
	}

	public override RGD Serialize_Save()
	{
		return new RGD_NetworkBehaviour(RGDType.NetworkBehaviour_PickupObjectManager, this);
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_PickupManager_Create(Messages.CreateNetworkPickupManager, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.CreateNetworkPickupManager:
			foreach (RGD_PickupItem pickupItem in (msg as Message_PickupManager_Create).pickupItems)
			{
				SpawnDropItem(pickupItem.rgdInstance.GetRealInstance(), pickupItem.Position, pickupItem.Rotation, pickupItem.itemObjectIndex);
			}
			return false;
		case Messages.RemoveItem:
		{
			Message_PickupObjectManager_RemoveItem message_PickupObjectManager_RemoveItem = msg as Message_PickupObjectManager_RemoveItem;
			return RemovePickupItem(NetworkIDManager.GetNetworkIDFromObjectIndex<PickupItem_Networked>(message_PickupObjectManager_RemoveItem.itemObjectIndex), message_PickupObjectManager_RemoveItem.SteamID);
		}
		case Messages.DropItem:
		{
			Message_DropItem message_DropItem = msg as Message_DropItem;
			if (Raft_Network.IsHost)
			{
				message_DropItem.dropObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
			}
			DropItem(message_DropItem.rgdInstance.GetRealInstance(), message_DropItem.DropPosition, message_DropItem.DropDirection, message_DropItem.dropObjectIndex, message_DropItem.parentedToRaft);
			return true;
		}
		default:
			return false;
		}
	}
}
