using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class PlayerInventory : Inventory
{
	[Header("Hotbar settings")]
	[HideInInspector]
	public Hotbar hotbar;

	[HideInInspector]
	public int hotslotCount;

	[Header("Equipment settings")]
	public List<Slot> backpackSlots = new List<Slot>();

	public List<Slot_Equip> equipSlots = new List<Slot_Equip>();

	[Header("Item description")]
	public Text itemNameText;

	public Text itemDescriptionText;

	public Image itemImage;

	private InventoryPickup inventoryPickup;

	private Network_Player playerNetwork;

	private bool backpackEquipped;

	[SerializeField]
	private InputActionReference InputA;

	[SerializeField]
	private InputActionReference inputY;

	private bool splitStackProgress;

	protected override void Awake()
	{
		base.Awake();
		ComponentManager<PlayerInventory>.Value = this;
		inventoryPickup = Object.FindObjectOfType<InventoryPickup>();
		Text text = itemNameText;
		string text2 = (itemDescriptionText.text = string.Empty);
		text.text = text2;
		itemImage.enabled = false;
	}

	protected override void Start()
	{
		base.Start();
		playerNetwork = ComponentManager<Network_Player>.Value;
		InputA.action.performed += OnAPerformed;
		InputA.action.canceled += OnACanceled;
		inputY.action.performed += OnYPerformed;
	}

	private void OnDestroy()
	{
		inventoryPickup = null;
		playerNetwork = null;
		InputA.action.performed -= OnAPerformed;
		InputA.action.canceled -= OnACanceled;
		inputY.action.performed -= OnYPerformed;
	}

	protected override void InitializeSlots()
	{
		hotbar = Object.FindObjectOfType<Hotbar>();
		allSlots.AddRange(hotbar.GetComponentsInChildren<Slot>());
		base.InitializeSlots();
		hotslotCount = 0;
		for (int i = 0; i < allSlots.Count; i++)
		{
			Slot slot = allSlots[i];
			if (slot.slotType == SlotType.Hotbar)
			{
				hotslotCount++;
			}
			if (slot.slotType == SlotType.Backpack)
			{
				backpackSlots.Add(slot);
			}
			if (slot.slotType == SlotType.Equipment)
			{
				equipSlots.Add(slot as Slot_Equip);
				allSlots.RemoveAt(i);
				i--;
			}
		}
		allSlots = allSlots.OrderBy((Slot s) => s.slotType).ToList();
		SetBackpackActiveSlots(0);
	}

	public void GiveStartingItems()
	{
		if (GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.giveStartingItems)
		{
			AddItem("Hammer", 1);
			AddItem("Axe", 1);
			AddItem("PaintBrush", 1);
		}
		else
		{
			AddItem("Hook_Plastic", 1);
		}
		if (playerNetwork != null && playerNetwork.IsLocalPlayer && Cheat.IsUserDev(playerNetwork.steamID))
		{
			AddItemAndEquip("DevHat");
			AddItemAndEquip("Backpack_Advanced");
			AddItemAndEquip("ZiplineTool_Electric");
		}
	}

	private void OnACanceled(InputAction.CallbackContext obj)
	{
		if (splitStackProgress)
		{
			CancelInvoke("ButtonHoldTimerTick");
			splitStackProgress = false;
		}
	}

	private void ButtonHoldTimerTick()
	{
		if (splitStackProgress && Inventory.dragAmount < Inventory.fromSlot.itemInstance.Amount)
		{
			Inventory.dragAmount++;
		}
	}

	private void OnAPerformed(InputAction.CallbackContext obj)
	{
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			return;
		}
		if (obj.interaction is HoldInteraction)
		{
			if (PlayerInput.GetPlayerByIndex(0).actions["TriggerLeft"].IsPressed())
			{
				if (Inventory.hoverSlot != null && !Inventory.hoverSlot.IsEmpty)
				{
					splitStackProgress = true;
					MoveItem(Inventory.hoverSlot, PointerEventData.InputButton.Right);
					InvokeRepeating("ButtonHoldTimerTick", 0.5f, 0.4f);
				}
			}
			else if (Inventory.hoverSlot != null && !Inventory.hoverSlot.IsEmpty && Inventory.hoverSlot.itemInstance.settings_equipment.EquipType != EquipSlotType.Backpack)
			{
				MoveItem(Inventory.hoverSlot, PointerEventData.InputButton.Middle);
			}
		}
		else if (Inventory.hoverSlot != null && obj.interaction != null)
		{
			MoveItem(Inventory.hoverSlot, PointerEventData.InputButton.Left);
		}
		else if (Inventory.hoverSlot == null && Inventory.fromSlot != null && Inventory.fromSlot.itemInstance != null)
		{
			DropItem(Inventory.fromSlot);
		}
	}

	private void OnYPerformed(InputAction.CallbackContext obj)
	{
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad && obj.interaction is HoldInteraction)
		{
			if (Inventory.hoverSlot != null && !Inventory.hoverSlot.IsEmpty)
			{
				SwitchSlots(Inventory.hoverSlot, hotbar.GetSelectedSlot());
			}
		}
		else
		{
			if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				return;
			}
			if (Inventory.fromSlot != null && Inventory.fromSlot.itemInstance != null)
			{
				if (Inventory.hoverSlot == null)
				{
					DropItem(Inventory.fromSlot);
				}
				else if (Inventory.hoverSlot.IsEmpty)
				{
					MoveSlotToEmpty(Inventory.fromSlot, Inventory.hoverSlot, 1);
					Inventory.dragAmount--;
					if (Inventory.dragAmount == 0)
					{
						Inventory.fromSlot = null;
					}
				}
				else if (Inventory.fromSlot.itemInstance.UniqueIndex == Inventory.hoverSlot.itemInstance.UniqueIndex && !Inventory.hoverSlot.StackIsFull() && Inventory.fromSlot != Inventory.hoverSlot)
				{
					StackSlots(Inventory.fromSlot, Inventory.hoverSlot, 1);
					Inventory.dragAmount--;
					if (Inventory.dragAmount == 0)
					{
						Inventory.fromSlot = null;
					}
				}
			}
			else if (Inventory.hoverSlot != null && !Inventory.hoverSlot.IsEmpty)
			{
				if (PlayerInput.GetPlayerByIndex(0).actions["TriggerLeft"].IsPressed())
				{
					DropItem(Inventory.hoverSlot);
				}
				else
				{
					Inventory.hoverSlot.inventory.ShiftMoveItem(Inventory.hoverSlot);
				}
			}
		}
	}

	public new bool CanShiftMove(Slot slot)
	{
		Inventory inventory = slot.inventory.secondInventory;
		if (inventory == null)
		{
			if (((!hotbar.ContainsSlot(slot)) ? FindSuitableSlot(0, hotslotCount, slot.itemInstance.baseItem) : FindSuitableSlot(hotslotCount, allSlots.Count, slot.itemInstance.baseItem)) == null)
			{
				return false;
			}
			return true;
		}
		if (!slot.IsEmpty && inventory is Inventory_ResearchTable inventory_ResearchTable)
		{
			return inventory_ResearchTable.activeTable.Slot.IsEmpty;
		}
		return inventory.FindSuitableSlot(slot.itemInstance.baseItem) != null;
	}

	protected override void Update()
	{
		base.Update();
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad && Inventory.hoverSlot != null && !Inventory.hoverSlot.IsEmpty && Inventory.hoverSlot.itemInstance.settings_equipment.EquipType != EquipSlotType.Backpack && MyInput.GetButtonDown("Drop"))
		{
			DropItem(Inventory.hoverSlot);
		}
	}

	public void AddItemAndEquip(string uniqueItemName)
	{
		if (ItemManager.GetItemByName(uniqueItemName) == null || equipSlots == null)
		{
			return;
		}
		for (int i = 0; i < equipSlots.Count; i++)
		{
			Slot_Equip slot_Equip = equipSlots[i];
			if (slot_Equip != null && slot_Equip.IsEmpty)
			{
				AddItem(uniqueItemName, slot_Equip, 1);
				return;
			}
		}
		AddItem(uniqueItemName, 1);
	}

	public override int AddItem(string uniqueItemName, int amount)
	{
		if (amount == 0)
		{
			return 0;
		}
		Slot selectedHotbarSlot = GetSelectedHotbarSlot();
		bool isEmpty = selectedHotbarSlot.IsEmpty;
		int result = base.AddItem(uniqueItemName, amount);
		if (isEmpty && !selectedHotbarSlot.IsEmpty)
		{
			hotbar.ReselectCurrentSlot();
		}
		inventoryPickup.ShowItem(uniqueItemName, amount);
		HandleNewItemAdded(uniqueItemName, amount);
		return result;
	}

	public override void AddItem(string uniqueItemName, Slot slot, int amount)
	{
		if (amount != 0)
		{
			Slot selectedHotbarSlot = GetSelectedHotbarSlot();
			bool isEmpty = selectedHotbarSlot.IsEmpty;
			base.AddItem(uniqueItemName, slot, amount);
			if (isEmpty && !selectedHotbarSlot.IsEmpty)
			{
				hotbar.ReselectCurrentSlot();
			}
			inventoryPickup.ShowItem(uniqueItemName, amount);
			HandleNewItemAdded(uniqueItemName, amount);
		}
	}

	public override void AddItem(ItemInstance itemInstance, bool dropIfFull = true)
	{
		Slot selectedHotbarSlot = GetSelectedHotbarSlot();
		bool isEmpty = selectedHotbarSlot.IsEmpty;
		int amount = itemInstance.Amount;
		base.AddItem(itemInstance, dropIfFull);
		if (isEmpty && !selectedHotbarSlot.IsEmpty)
		{
			hotbar.ReselectCurrentSlot();
		}
		if (dropIfFull)
		{
			inventoryPickup.ShowItem(itemInstance.UniqueName, amount);
		}
		HandleNewItemAdded(itemInstance.UniqueName, amount);
	}

	public void HandleNewItemAdded(string uniqueItemName, int amountAdded)
	{
		if (playerNetwork == null)
		{
			playerNetwork = ComponentManager<Network_Player>.Value;
		}
		Item_Base itemByName = ItemManager.GetItemByName(uniqueItemName);
		if (!(itemByName != null) || !itemByName.settings_recipe.IsBlueprint)
		{
			return;
		}
		Inventory_ResearchTable value = ComponentManager<Inventory_ResearchTable>.Value;
		if (value != null && value.CanResearchBlueprint(itemByName))
		{
			Message_ResearchTable_ResearchOrLearn message = new Message_ResearchTable_ResearchOrLearn(Messages.ResearchBlueprint, playerNetwork, playerNetwork.steamID, itemByName.UniqueIndex);
			if (Raft_Network.IsHost)
			{
				value.ResearchBlueprint(itemByName);
				value.network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				value.network.SendP2P(value.network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public override void Clear()
	{
		base.Clear();
		foreach (Slot_Equip equipSlot in equipSlots)
		{
			equipSlot.Reset();
		}
	}

	public void ClearInventoryLeaveSome()
	{
		List<Slot> list = new List<Slot>();
		list.AddRange(allSlots);
		list.AddRange(equipSlots);
		if (list == null || list.Count <= 0)
		{
			return;
		}
		foreach (Slot item in list)
		{
			if (item == null || item.IsEmpty)
			{
				continue;
			}
			ItemInstance itemInstance = item.itemInstance;
			if (itemInstance.baseItem.settings_Inventory.Stackable && item.slotType != SlotType.Equipment)
			{
				int num = Mathf.FloorToInt((float)itemInstance.Amount / 3f);
				if (num <= 0)
				{
					item.Reset();
				}
				else
				{
					item.SetItem(itemInstance.baseItem, num);
				}
			}
			else if (itemInstance.BaseItemMaxUses > 1)
			{
				int num2 = Mathf.FloorToInt((float)itemInstance.BaseItemMaxUses * 0.5f);
				int num3 = itemInstance.Uses - num2;
				if (num3 <= 0)
				{
					item.Reset();
				}
				else
				{
					item.SetUses(num3);
				}
			}
		}
	}

	public override void ShiftMoveItem(Slot slot)
	{
		if (secondInventory == null && !slot.IsEmpty)
		{
			Slot slot2 = ((!hotbar.ContainsSlot(slot)) ? FindSuitableSlot(0, hotslotCount, slot.itemInstance.baseItem) : FindSuitableSlot(hotslotCount, allSlots.Count, slot.itemInstance.baseItem));
			if (!(slot2 == null))
			{
				if (slot2.IsEmpty)
				{
					MoveSlotToEmpty(slot, slot2, slot.itemInstance.Amount);
				}
				else if (slot.itemInstance.settings_Inventory.Stackable && slot.itemInstance.UniqueIndex == slot2.itemInstance.UniqueIndex)
				{
					StackSlots(slot, slot2, slot.itemInstance.Amount);
				}
			}
		}
		else if (!slot.IsEmpty && secondInventory is Inventory_ResearchTable)
		{
			Slot slot3 = (secondInventory as Inventory_ResearchTable).activeTable.Slot;
			if (slot3.IsEmpty)
			{
				MoveSlotToEmpty(slot, slot3, slot.itemInstance.Amount);
			}
			else
			{
				SwitchSlots(slot, slot3);
			}
		}
		else
		{
			base.ShiftMoveItem(slot);
		}
	}

	protected override void MoveSlotToEmpty(Slot fromSlot, Slot toSlot, int amount)
	{
		if (toSlot.slotType != SlotType.Backpack || backpackEquipped)
		{
			base.MoveSlotToEmpty(fromSlot, toSlot, amount);
			if (hotbar.IsSelectedHotSlot(fromSlot) || hotbar.IsSelectedHotSlot(toSlot))
			{
				hotbar.ReselectCurrentSlot();
			}
		}
	}

	protected override void StackSlots(Slot fromSlot, Slot toSlot, int dragAmount)
	{
		base.StackSlots(fromSlot, toSlot, dragAmount);
		if (hotbar.IsSelectedHotSlot(fromSlot) || hotbar.IsSelectedHotSlot(toSlot))
		{
			hotbar.ReselectCurrentSlot();
		}
	}

	protected override void SwitchSlots(Slot fromSlot, Slot toSlot)
	{
		base.SwitchSlots(fromSlot, toSlot);
		if (hotbar.IsSelectedHotSlot(fromSlot) || hotbar.IsSelectedHotSlot(toSlot))
		{
			hotbar.ReselectCurrentSlot();
		}
	}

	protected override bool ContainsSlot(Slot slot)
	{
		for (int i = 0; i < equipSlots.Count; i++)
		{
			if (equipSlots[i] == slot)
			{
				return true;
			}
		}
		return base.ContainsSlot(slot);
	}

	public void SetBackpackActiveSlots(int numberOfSlotsToActivate)
	{
		backpackEquipped = numberOfSlotsToActivate > 0;
		int num = 0;
		foreach (Slot allSlot in allSlots)
		{
			if (allSlot.slotType == SlotType.Backpack)
			{
				bool flag = num < numberOfSlotsToActivate;
				allSlot.gameObject.SetActiveSafe(flag);
				allSlot.active = flag;
				num++;
			}
		}
	}

	public bool HasEquipmentOfTypeEquipped(EquipSlotType equipmentType)
	{
		return GetEquipmentSlotFromEquipmentType(equipmentType) != null;
	}

	public Slot_Equip GetEquipmentSlotFromEquipmentType(EquipSlotType equipmentType)
	{
		if (equipSlots.ContainsItems())
		{
			for (int i = 0; i < equipSlots.Count; i++)
			{
				Slot_Equip slot_Equip = equipSlots[i];
				if (!(slot_Equip == null) && slot_Equip.HasValidItemInstance() && slot_Equip.itemInstance.settings_equipment.EquipType == equipmentType)
				{
					return slot_Equip;
				}
			}
		}
		return null;
	}

	public bool RemoveDurabillityFromHotSlot(int durabilityStacksToRemove = 1)
	{
		if (!GameModeValueManager.GetCurrentGameModeValue().toolVariables.areToolsIndestructible)
		{
			int stacksToRemove = (int)((float)durabilityStacksToRemove * GameModeValueManager.GetCurrentGameModeValue().toolVariables.toolDurabilityLossMultiplier);
			return RemoveStacksFromHotSlot(stacksToRemove);
		}
		return false;
	}

	public bool RemoveDurabillityFromEquipment(EquipSlotType equipmentType, int durabilityStacksToRemove = 1)
	{
		if (!GameModeValueManager.GetCurrentGameModeValue().toolVariables.areToolsIndestructible)
		{
			int stacksToRemove = (int)((float)durabilityStacksToRemove * GameModeValueManager.GetCurrentGameModeValue().toolVariables.toolDurabilityLossMultiplier);
			return RemoveStacksFromSlot(GetEquipmentSlotFromEquipmentType(equipmentType), stacksToRemove);
		}
		return false;
	}

	private bool RemoveStacksFromSlot(Slot slot, int stacksToRemove = 1)
	{
		bool result = false;
		if (slot != null && !slot.IsEmpty)
		{
			if (slot.itemInstance.Uses - stacksToRemove <= 0 && slot.itemInstance.Amount <= 1)
			{
				result = true;
				if (slot.itemInstance.baseItem.settings_usable.EventRef_Break.IsValid())
				{
					SoundManager value = ComponentManager<SoundManager>.Value;
					Network_Player value2 = ComponentManager<Network_Player>.Value;
					if (value != null && value2 != null)
					{
						RuntimeManager.PlayOneShot(slot.itemInstance.baseItem.settings_usable.EventRef_Break, value2.transform.position);
					}
				}
			}
			slot.IncrementUses(-stacksToRemove);
		}
		return result;
	}

	private bool RemoveStacksFromHotSlot(int stacksToRemove = 1)
	{
		Slot selectedHotbarSlot = GetSelectedHotbarSlot();
		return RemoveStacksFromSlot(selectedHotbarSlot, stacksToRemove);
	}

	public Slot GetSelectedHotbarSlot()
	{
		return GetSlot(hotbar.GetSelectedSlotIndex());
	}

	public ItemInstance GetSelectedHotbarItem()
	{
		return GetSlot(hotbar.GetSelectedSlotIndex()).itemInstance;
	}

	public RGD_Slot[] GetRGDSlotsEquip()
	{
		List<RGD_Slot> list = new List<RGD_Slot>();
		for (int i = 0; i < equipSlots.Count; i++)
		{
			Slot_Equip slot_Equip = equipSlots[i];
			if (!(slot_Equip == null) && !slot_Equip.IsEmpty)
			{
				list.Add(new RGD_Slot(slot_Equip, i));
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray();
		}
		return null;
	}

	public void SetEquipSlotsFromRGD(RGD_Slot[] slots)
	{
		if (slots == null)
		{
			return;
		}
		for (int i = 0; i < equipSlots.Count; i++)
		{
			Slot slot = equipSlots[i];
			if (slot == null)
			{
				continue;
			}
			bool flag = false;
			foreach (RGD_Slot rGD_Slot in slots)
			{
				if (!(rGD_Slot == null) && rGD_Slot.slotIndex == i)
				{
					flag = true;
					rGD_Slot.RestoreSlot(slot);
					break;
				}
			}
			if (!flag)
			{
				slot.SetItem(null);
			}
		}
	}

	public void SetItemDescription(Item_Base item)
	{
		if (item != null)
		{
			itemNameText.text = item.settings_Inventory.DisplayName;
			itemDescriptionText.text = item.settings_Inventory.Description;
			itemImage.enabled = true;
			itemImage.sprite = item.settings_Inventory.Sprite;
		}
		else
		{
			Text text = itemNameText;
			string text2 = (itemDescriptionText.text = string.Empty);
			text.text = text2;
			itemImage.enabled = false;
		}
	}

	public void RemoveSelectedHotSlotItem(int amount)
	{
		Slot slot = GetSlot(hotbar.GetSelectedSlotIndex());
		if (!(slot == null) && !slot.IsEmpty)
		{
			slot.RemoveItem(amount);
			if (slot.IsEmpty)
			{
				hotbar.ReselectCurrentSlot();
			}
		}
	}

	public void DropItem(Slot slot)
	{
		if (!(slot == null) && !slot.IsEmpty)
		{
			Helper.DropItem(slot.itemInstance, hotbar.playerNetwork.transform.position, hotbar.playerNetwork.CameraTransform.forward, hotbar.playerNetwork.PersonController.HasRaftAsParent);
			slot.SetItem(null, 0);
			if (hotbar.IsSelectedHotSlot(slot))
			{
				hotbar.ReselectCurrentSlot();
			}
			Inventory.fromSlot = null;
		}
	}

	public void DropItem(Item_Base item, int amount)
	{
		Helper.DropItem(new ItemInstance(item, amount, item.MaxUses), hotbar.playerNetwork.transform.position, hotbar.playerNetwork.CameraTransform.forward, hotbar.playerNetwork.PersonController.HasRaftAsParent);
	}

	public void DropItem(ItemInstance instance)
	{
		Helper.DropItem(instance, hotbar.playerNetwork.transform.position, hotbar.playerNetwork.CameraTransform.forward, hotbar.playerNetwork.PersonController.HasRaftAsParent);
	}

	public void DropCurrentItem()
	{
		Slot selectedHotbarSlot = GetSelectedHotbarSlot();
		if (!selectedHotbarSlot.IsEmpty)
		{
			DropItem(selectedHotbarSlot);
		}
	}

	private Slot FindSuitableSlot(int startSlotIndex, int endSlotIndex, Item_Base stackableItem = null)
	{
		bool flag = stackableItem != null && stackableItem.settings_Inventory.Stackable;
		Slot slot = null;
		for (int i = startSlotIndex; i < endSlotIndex; i++)
		{
			if (i >= 0 && i < allSlots.Count)
			{
				Slot slot2 = allSlots[i];
				if (slot2.IsEmpty && slot == null && slot2.gameObject.activeSelf)
				{
					slot = slot2;
				}
				if (flag && !slot2.StackIsFull() && !slot2.IsEmpty && slot2.itemInstance.UniqueIndex == stackableItem.UniqueIndex && slot2.gameObject.activeSelf)
				{
					return slot2;
				}
			}
		}
		return slot;
	}
}
