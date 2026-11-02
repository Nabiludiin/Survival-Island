using UnityEngine;

public class CookingStand : MonoBehaviour_ID_Network, IBlockPlaced
{
	public Block_CookingStand blockCookingStand;

	private Raft_Network network;

	protected virtual void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this);
	}

	public void OnValidate()
	{
		if (blockCookingStand == null)
		{
			blockCookingStand = GetComponent<Block_CookingStand>();
		}
	}

	public void OnBlockPlacedInterface()
	{
		NetworkIDManager.AddNetworkID(this, typeof(CookingStand));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		switch (msg.Type)
		{
		case Messages.CookingStand_InsertItem:
		{
			Message_CookingStand_Insert message_CookingStand_Insert = msg as Message_CookingStand_Insert;
			Item_Base itemByIndex = ItemManager.GetItemByIndex(message_CookingStand_Insert.itemToInsertIndex);
			Network_Player playerFromID = network.GetPlayerFromID(message_CookingStand_Insert.SteamID);
			if (!(blockCookingStand != null))
			{
				break;
			}
			CookingSlot[] array = null;
			if (message_CookingStand_Insert.emptySlotIndexes == null)
			{
				array = blockCookingStand.GetCookingSlotsForItem(itemByIndex);
			}
			else if (message_CookingStand_Insert.emptySlotIndexes.Length != 0)
			{
				array = new CookingSlot[message_CookingStand_Insert.emptySlotIndexes.Length];
				for (int i = 0; i < message_CookingStand_Insert.emptySlotIndexes.Length; i++)
				{
					int num = message_CookingStand_Insert.emptySlotIndexes[i];
					if (num >= 0 && num < blockCookingStand.cookingSlots.Length)
					{
						array[i] = blockCookingStand.cookingSlots[message_CookingStand_Insert.emptySlotIndexes[i]];
					}
				}
			}
			else
			{
				Debug.LogError("Could not insert item into cookingslot, cookingslotindex was out of range of cookingslot");
			}
			return blockCookingStand.InsertItem(itemByIndex, array, playerFromID);
		}
		case Messages.CookingStand_SlotComplete:
		{
			Message_CookingStand_SlotComplete message_CookingStand_SlotComplete = msg as Message_CookingStand_SlotComplete;
			blockCookingStand.cookingSlots[message_CookingStand_SlotComplete.cookingSlotIndex].OnComplete();
			return false;
		}
		case Messages.CookingStand_IncrementFuel:
		{
			Message_CookingStand_IncrementFuel message_CookingStand_IncrementFuel = msg as Message_CookingStand_IncrementFuel;
			if (network == null)
			{
				Debug.Log("Network was null when trying to increment/decrement fuel, Cookingstand.cs");
			}
			return blockCookingStand.IncrementFuel(message_CookingStand_IncrementFuel.incrementAmount, network.GetPlayerFromID(message_CookingStand_IncrementFuel.SteamID));
		}
		case Messages.CookingStand_CollectItem:
		{
			Message_CookingStand_Collect message_CookingStand_Collect = msg as Message_CookingStand_Collect;
			if (blockCookingStand != null)
			{
				CookingSlot cookingSlot = blockCookingStand.cookingSlots[message_CookingStand_Collect.slotToCollectIndex];
				if (cookingSlot != null)
				{
					return blockCookingStand.CollectItem(cookingSlot, network.GetPlayerFromID(message_CookingStand_Collect.SteamID));
				}
			}
			break;
		}
		}
		return false;
	}
}
