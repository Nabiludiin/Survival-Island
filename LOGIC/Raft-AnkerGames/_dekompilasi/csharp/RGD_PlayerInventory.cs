using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_PlayerInventory : SerializableBehaviour
{
	public RGD_Slot[] slots;

	public RGD_Slot[] equipSlots;

	public int hotslotIndex;

	public RGD_PlayerInventory(PlayerInventory inventory)
	{
		hotslotIndex = inventory.hotbar.GetSelectedSlotIndex();
		slots = inventory.GetRGDSlots();
		equipSlots = inventory.GetRGDSlotsEquip();
	}

	public void RestoreInventory(PlayerInventory inventory)
	{
		if (!(inventory == null))
		{
			inventory.SetSlotsFromRGD(slots);
			inventory.SetEquipSlotsFromRGD(equipSlots);
			if (inventory.hotbar != null)
			{
				inventory.hotbar.SetSelectedSlotIndex(hotslotIndex);
				inventory.hotbar.SelectHotslot(inventory.GetSlot(hotslotIndex));
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Slots", slots);
		info.AddValue("EquipSlots", equipSlots);
		info.AddValue("HotslotIndex", hotslotIndex);
	}

	public RGD_PlayerInventory(SerializationInfo info, StreamingContext sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Slots":
				slots = item.Value as RGD_Slot[];
				break;
			case "HotslotIndex":
				hotslotIndex = Convert.ToInt32(item.Value);
				break;
			case "EquipSlots":
				equipSlots = item.Value as RGD_Slot[];
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		slots = null;
		equipSlots = null;
		hotslotIndex = 0;
	}
}
