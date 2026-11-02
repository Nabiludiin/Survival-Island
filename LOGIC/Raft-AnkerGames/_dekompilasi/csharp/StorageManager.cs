using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class StorageManager : MonoBehaviour_Network, SceneEventInterface
{
	public static List<Storage_Small> allStorages = new List<Storage_Small>();

	private Network_Player playerNetwork;

	public Storage_Small currentStorage;

	public float maxDistanceToStorage = 5f;

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (currentStorage != null && !GameManager.IsQuittingGame)
		{
			CloseStorage(currentStorage);
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			allStorages.Clear();
		}
	}

	private void Update()
	{
		if (!playerNetwork.IsLocalPlayer || !(currentStorage != null) || !currentStorage.IsOpen)
		{
			return;
		}
		InputAction inputAction = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		_ = PlayerInput.GetPlayerByIndex(0).actions["Inventory"];
		InputAction inputAction2 = PlayerInput.GetPlayerByIndex(0).actions["Cancel"];
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["SortPlayerIntoChest"];
		InputAction inputAction3 = PlayerInput.GetPlayerByIndex(0).actions["SortChest"];
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "SortStorage"))
		{
			Inventory inventoryReference = currentStorage.GetInventoryReference();
			if (inventoryReference != null)
			{
				if (inventoryReference is Inventory_Trashcan inventory_Trashcan)
				{
					inventory_Trashcan.Button_EmptyTrash();
				}
				else
				{
					inventoryReference.SortPlayerInventoryIntoStorage();
				}
			}
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad && inputAction3.WasPressedThisFrame())
		{
			Inventory inventoryReference2 = currentStorage.GetInventoryReference();
			if (inventoryReference2 != null)
			{
				inventoryReference2.Sort();
			}
		}
		bool flag = ((!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad) ? ((MyInput.GetButtonDown("Interact") && currentStorage.CanCloseWithUseButton) || MyInput.GetButtonDown("Inventory") || MyInput.GetButtonDown("Cancel")) : ((inputAction.WasPressedThisFrame() && currentStorage.CanCloseWithUseButton) || inputAction2.WasPressedThisFrame()));
		if (Vector3.Distance(playerNetwork.transform.position, currentStorage.transform.position) > maxDistanceToStorage || playerNetwork.Stats.IsDead)
		{
			Message_Storage_Close message = new Message_Storage_Close(Messages.StorageManager_Close, this, currentStorage);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				CloseStorage(currentStorage);
			}
			else
			{
				playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else if (flag)
		{
			Message_Storage_Close message2 = new Message_Storage_Close(Messages.StorageManager_Close, this, currentStorage);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message2, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				CloseStorage(currentStorage);
			}
			else
			{
				playerNetwork.SendP2P(message2, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public bool OpenStorage(Storage_Small storage)
	{
		if (storage == null || storage.IsOpen)
		{
			return false;
		}
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (playerNetwork.IsLocalPlayer)
		{
			currentStorage = storage;
			Inventory inventoryReference = storage.GetInventoryReference();
			playerNetwork.Inventory.secondInventory = inventoryReference;
			inventoryReference.secondInventory = playerNetwork.Inventory;
		}
		storage.Open(playerNetwork);
		return true;
	}

	public bool CloseStorage(Storage_Small storage)
	{
		if (!storage.IsOpen)
		{
			return false;
		}
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (playerNetwork.IsLocalPlayer)
		{
			storage.GetInventoryReference().secondInventory = (playerNetwork.Inventory.secondInventory = null);
		}
		storage.Close(playerNetwork);
		if (playerNetwork.IsLocalPlayer)
		{
			currentStorage = null;
		}
		return true;
	}

	public bool CloseCurrentStorage()
	{
		if (currentStorage != null && playerNetwork != null)
		{
			currentStorage.Close(playerNetwork);
			return true;
		}
		return false;
	}

	public Storage_Small GetStorageByObjectIndex(uint index)
	{
		return allStorages.Find((Storage_Small s) => s.ObjectIndex == index);
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_Storage_Create(Messages.StorageManager_Create, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.StorageManager_Create:
		{
			Message_Storage_Create obj = msg as Message_Storage_Create;
			obj.RestoreStorages(obj, this);
			return false;
		}
		case Messages.StorageManager_Open:
		{
			Message_Storage message_Storage = msg as Message_Storage;
			if (GetStorageByObjectIndex(message_Storage.storageObjectIndex) != null)
			{
				return OpenStorage(GetStorageByObjectIndex(message_Storage.storageObjectIndex));
			}
			Debug.LogError("Could not open storage, storage with object index could not be found");
			return false;
		}
		case Messages.StorageManager_Close:
		{
			Message_Storage_Close message_Storage_Close = msg as Message_Storage_Close;
			Storage_Small storageByObjectIndex = GetStorageByObjectIndex(message_Storage_Close.storageObjectIndex);
			if (storageByObjectIndex != null)
			{
				storageByObjectIndex.GetInventoryReference().SetSlotsFromRGD(message_Storage_Close.slots);
				return CloseStorage(storageByObjectIndex);
			}
			Debug.LogError("Could not close storage, storage with object index could not be found");
			return false;
		}
		default:
			return false;
		}
	}
}
