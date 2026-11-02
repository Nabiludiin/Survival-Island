using System;
using UnityEngine;

[Serializable]
public class ItemInstance_Equipment
{
	[SerializeField]
	private EquipSlotType slotType;

	public EquipSlotType EquipType => slotType;

	public ItemInstance_Equipment(EquipSlotType slotType)
	{
		this.slotType = slotType;
	}

	public ItemInstance_Equipment Clone()
	{
		return (ItemInstance_Equipment)MemberwiseClone();
	}
}
