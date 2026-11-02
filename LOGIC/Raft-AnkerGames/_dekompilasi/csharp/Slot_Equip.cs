using System.Collections.Generic;
using UnityEngine;

public class Slot_Equip : Slot, SceneEventInterface
{
	private static List<Slot_Equip> allEquipSlots = new List<Slot_Equip>();

	private PlayerEquipment playerEquipment;

	protected override void Awake()
	{
		base.Awake();
		allEquipSlots.Add(this);
	}

	protected override void Start()
	{
		base.Start();
		if (playerEquipment == null)
		{
			playerEquipment = ComponentManager<PlayerEquipment>.Value;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		playerEquipment = null;
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			allEquipSlots.Clear();
		}
	}

	public override void SetItem(ItemInstance newInstance)
	{
		if (playerEquipment == null)
		{
			playerEquipment = ComponentManager<PlayerEquipment>.Value;
		}
		ItemInstance itemInstance = (ValidInstance(base.itemInstance) ? base.itemInstance.Clone() : null);
		base.SetItem(newInstance);
		if (playerEquipment == null)
		{
			Debug.LogError("Can't equip item because playerEquipment script could not be found");
		}
		else if (base.itemInstance == null && itemInstance != null)
		{
			playerEquipment.UnEquip(itemInstance);
		}
		else if (base.itemInstance != null && itemInstance == null)
		{
			playerEquipment.Equip(base.itemInstance, this);
		}
		else if (itemInstance != null && base.itemInstance != null)
		{
			playerEquipment.UnEquip(itemInstance);
			playerEquipment.Equip(base.itemInstance, this);
		}
	}

	public override void SetItem(Item_Base newItem, int amount)
	{
		if (playerEquipment == null)
		{
			playerEquipment = ComponentManager<PlayerEquipment>.Value;
		}
		ItemInstance itemInstance = (ValidInstance(base.itemInstance) ? base.itemInstance.Clone() : null);
		base.SetItem(newItem, amount);
		if (playerEquipment == null)
		{
			Debug.LogError("Can't equip item because playerEquipment script could not be found");
		}
		else if (base.itemInstance == null && itemInstance != null)
		{
			playerEquipment.UnEquip(itemInstance);
		}
		else if (base.itemInstance != null && itemInstance == null)
		{
			playerEquipment.Equip(base.itemInstance, this);
		}
		else if (itemInstance != null && base.itemInstance != null)
		{
			playerEquipment.UnEquip(itemInstance);
			playerEquipment.Equip(base.itemInstance, this);
		}
	}

	public static Slot_Equip GetEquipSlotWithTag(EquipSlotType tag)
	{
		if (tag == EquipSlotType.None)
		{
			return null;
		}
		for (int i = 0; i < allEquipSlots.Count; i++)
		{
			Slot_Equip slot_Equip = allEquipSlots[i];
			if (!(slot_Equip == null) && !slot_Equip.IsEmpty && slot_Equip.itemInstance.settings_equipment.EquipType != EquipSlotType.None && slot_Equip.itemInstance.settings_equipment.EquipType == tag)
			{
				return slot_Equip;
			}
		}
		return null;
	}
}
