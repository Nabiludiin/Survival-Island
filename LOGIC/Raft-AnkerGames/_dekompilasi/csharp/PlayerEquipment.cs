using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
	[SerializeField]
	private Network_Player playerNetwork;

	private Equipment[] equipment;

	private void Awake()
	{
		equipment = GetComponentsInChildren<Equipment>();
	}

	private void Start()
	{
		Equipment[] array = equipment;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initialize(playerNetwork);
		}
	}

	private void OnDestroy()
	{
		playerNetwork = null;
	}

	private void Update()
	{
		if (!playerNetwork.IsLocalPlayer)
		{
			return;
		}
		for (int i = 0; i < equipment.Length; i++)
		{
			if (equipment[i].Equipped)
			{
				equipment[i].UpdateEquipment();
			}
		}
	}

	public void Equip(ItemInstance item, Slot_Equip slot)
	{
		int index = -1;
		Equipment equipmentFromBaseItem = GetEquipmentFromBaseItem(item.baseItem, ref index);
		if (!(equipmentFromBaseItem != null))
		{
			return;
		}
		equipmentFromBaseItem.Equip(slot);
		if (equipmentFromBaseItem.sendNetworkInfo)
		{
			Message_Equipment message = new Message_Equipment(Messages.Equipment_Equip, playerNetwork, playerNetwork.steamID, index);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void UnEquip(ItemInstance item)
	{
		int index = -1;
		Equipment equipmentFromBaseItem = GetEquipmentFromBaseItem(item.baseItem, ref index);
		if (!(equipmentFromBaseItem != null))
		{
			return;
		}
		equipmentFromBaseItem.UnEquip();
		if (equipmentFromBaseItem.sendNetworkInfo)
		{
			Message_Equipment message = new Message_Equipment(Messages.Equipment_UnEquip, playerNetwork, playerNetwork.steamID, index);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public bool EquipItemNetwork(int equipmentIndex)
	{
		if (equipmentIndex < 0 || equipmentIndex >= this.equipment.Length)
		{
			return false;
		}
		Equipment equipment = this.equipment[equipmentIndex];
		if (equipment == null || equipment.Equipped)
		{
			return false;
		}
		if (!equipment.isInitialized)
		{
			equipment.Initialize(playerNetwork);
		}
		equipment.Equip(null);
		return true;
	}

	public bool UnEquipItemNetwork(int equipmentIndex)
	{
		if (equipmentIndex < 0 || equipmentIndex >= this.equipment.Length)
		{
			return false;
		}
		Equipment equipment = this.equipment[equipmentIndex];
		if (equipment == null || !equipment.Equipped)
		{
			return false;
		}
		equipment.UnEquip();
		return true;
	}

	public List<int> GetEquipedIndexes()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < equipment.Length; i++)
		{
			if (equipment[i].Equipped)
			{
				list.Add(i);
			}
		}
		return list;
	}

	public void HandleCreateMessage(Message_Equipment_Create msg)
	{
		for (int i = 0; i < msg.equipedIndexes.Length; i++)
		{
			EquipItemNetwork(msg.equipedIndexes[i]);
		}
	}

	public bool HasBackpackEquipped()
	{
		Equipment[] array = this.equipment;
		foreach (Equipment equipment in array)
		{
			if (equipment.Equipped && equipment.equipableItem.settings_equipment.EquipType == EquipSlotType.Backpack)
			{
				return true;
			}
		}
		return false;
	}

	public void SetAllEquipmentModelVisible(bool visible)
	{
		Equipment[] array = equipment;
		for (int i = 0; i < array.Length; i++)
		{
			Equipment_Model equipment_Model = (Equipment_Model)array[i];
			if (equipment_Model != null && equipment_Model.Equipped)
			{
				equipment_Model.SetModelState(visible);
			}
		}
	}

	private Equipment GetEquipmentFromBaseItem(Item_Base item, ref int index)
	{
		for (int i = 0; i < equipment.Length; i++)
		{
			if (equipment[i].equipableItem.UniqueIndex == item.UniqueIndex)
			{
				index = i;
				return equipment[i];
			}
		}
		index = -1;
		return null;
	}
}
