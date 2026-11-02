public class Equipment_Backpack : Equipment_Model
{
	public int extraBackpackSlots;

	public override void Initialize(Network_Player playerNetwork)
	{
		base.Initialize(playerNetwork);
	}

	public override void UpdateEquipment()
	{
		base.UpdateEquipment();
		if (!playerNetwork.IsLocalPlayer || !Equipped || !(equippedSlot != null))
		{
			return;
		}
		equippedSlot.locked = false;
		foreach (Slot backpackSlot in playerNetwork.Inventory.backpackSlots)
		{
			if (!backpackSlot.IsEmpty)
			{
				equippedSlot.locked = true;
				break;
			}
		}
	}

	public override void Equip(Slot_Equip equippedSlot)
	{
		base.Equip(equippedSlot);
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Inventory.SetBackpackActiveSlots(extraBackpackSlots);
		}
	}

	public override void UnEquip()
	{
		if (equippedSlot != null && equippedSlot.locked)
		{
			equippedSlot.locked = false;
		}
		base.UnEquip();
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Inventory.SetBackpackActiveSlots(0);
		}
	}
}
