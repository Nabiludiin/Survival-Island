using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, SceneEventInterface
{
	[Header("Prefabs")]
	public GameObject slotPrefab;

	[Header("Components")]
	public Inventory secondInventory;

	public RectTransform hoverTransform;

	public Image hooverImage;

	public RectTransform darkenedTransform;

	public Image draggingImage;

	public Text draggingText;

	public GridLayoutGroup gridLayoutGroup;

	public List<Slot> allSlots = new List<Slot>();

	protected RectTransform invRectTransform;

	public static Transform Parent;

	public static Slot fromSlot;

	private static Slot toSlot;

	public static Slot hoverSlot;

	private static bool dragging;

	private static bool canQuickEquip = true;

	private static PlayerInventory localPlayerInventory;

	private SoundManager soundManager;

	private CanvasHelper canvas;

	protected static int dragAmount = 1;

	[SerializeField]
	private Sprite normalSprite;

	[SerializeField]
	private Sprite backbackSprite;

	[SerializeField]
	private GameObject sortButton;

	[SerializeField]
	private GameObject dumpButton;

	protected virtual void Awake()
	{
		invRectTransform = GetComponent<RectTransform>();
		hoverTransform.sizeDelta = gridLayoutGroup.cellSize;
		darkenedTransform.sizeDelta = gridLayoutGroup.cellSize;
		draggingImage.rectTransform.sizeDelta = gridLayoutGroup.cellSize * 0.75f;
		InitializeSlots();
	}

	protected virtual void Start()
	{
		Hide();
		if (!GameManager.IsLeavingGame)
		{
			CanvasHelper.MenuCloseEvent += OnMenuClose;
		}
		localPlayerInventory = ComponentManager<PlayerInventory>.Value;
		soundManager = ComponentManager<SoundManager>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
	}

	protected virtual void Update()
	{
		if (sortButton != null)
		{
			sortButton.SetActive(!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
		}
		if (dumpButton != null)
		{
			dumpButton.SetActive(!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
		}
		if (localPlayerInventory == null)
		{
			localPlayerInventory = ComponentManager<PlayerInventory>.Value;
		}
		dragging = fromSlot != null && !fromSlot.IsEmpty && (ContainsSlot(fromSlot) || (secondInventory != null && secondInventory.ContainsSlot(fromSlot)));
		hoverTransform.gameObject.SetActive(hoverSlot != null && (ContainsSlot(hoverSlot) || (hoverSlot is Slot_Equip && this is PlayerInventory)));
		if (hoverTransform.gameObject.activeSelf)
		{
			hooverImage.sprite = ((hoverSlot.slotType == SlotType.Backpack) ? backbackSprite : normalSprite);
		}
		if (hoverTransform.gameObject.activeInHierarchy)
		{
			hoverTransform.position = hoverSlot.rectTransform.position;
		}
		darkenedTransform.gameObject.SetActive(dragging && ContainsSlot(fromSlot));
		if (dragging)
		{
			if (hoverSlot == fromSlot)
			{
				hoverTransform.gameObject.SetActive(value: false);
			}
			darkenedTransform.position = fromSlot.rectTransform.position;
		}
		draggingImage.gameObject.SetActive(dragging);
		if (dragging)
		{
			if (draggingText != null)
			{
				draggingText.text = dragAmount.ToString();
			}
			draggingImage.sprite = fromSlot.itemInstance.settings_Inventory.Sprite;
			draggingImage.rectTransform.position = ComponentManager<CanvasHelper>.Value.CursorPos - new Vector3(draggingImage.rectTransform.sizeDelta.x / 4f, (0f - draggingImage.rectTransform.sizeDelta.y) / 4f);
			draggingImage.rectTransform.sizeDelta = ((hoverSlot != null) ? hoverSlot.rectTransform.sizeDelta : fromSlot.rectTransform.sizeDelta);
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				if (!canvas.dropText.gameObject.activeInHierarchy)
				{
					canvas.dropText.gameObject.SetActive(value: true);
				}
				canvas.dropText.rectTransform.position = ComponentManager<CanvasHelper>.Value.CursorPos + new Vector3(0f, canvas.dropText.rectTransform.sizeDelta.y / 2f, 0f) * 1.5f;
				LocalizationParameters.itemX = fromSlot.GetItemBase().settings_Inventory.DisplayName;
				canvas.dropText.text = Helper.GetTerm("Game/DropX", applyParameters: true);
			}
			else if (canvas.dropText.gameObject.activeInHierarchy)
			{
				canvas.dropText.gameObject.SetActive(value: false);
			}
			if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad && this == localPlayerInventory)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis < 0f)
				{
					dragAmount = Mathf.Clamp(dragAmount - 1, 1, fromSlot.itemInstance.Amount);
				}
				else if (axis > 0f)
				{
					dragAmount = Mathf.Clamp(dragAmount + 1, 1, fromSlot.itemInstance.Amount);
				}
			}
		}
		else if (canvas.dropText.gameObject.activeInHierarchy)
		{
			canvas.dropText.gameObject.SetActive(value: false);
		}
		if (!canQuickEquip || dragging || !(hoverSlot != null))
		{
			return;
		}
		for (int i = 0; i < 10; i++)
		{
			if (!MyInput.GetButtonDown("Hotbar" + i))
			{
				continue;
			}
			int index = i;
			Slot slot = localPlayerInventory.GetSlot(index);
			if (hoverSlot != null && !hoverSlot.IsEmpty && slot != null && hoverSlot.itemInstance.settings_equipment.EquipType != EquipSlotType.Backpack)
			{
				if (slot.IsEmpty)
				{
					MoveSlotToEmpty(hoverSlot, slot, hoverSlot.itemInstance.Amount);
				}
				else
				{
					SwitchSlots(hoverSlot, slot);
				}
				localPlayerInventory.hotbar.ReselectCurrentSlot();
				canQuickEquip = false;
				Invoke("ResetQuickEquip", 0.05f);
			}
			break;
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			fromSlot = (toSlot = (hoverSlot = null));
			dragging = false;
			canQuickEquip = true;
			localPlayerInventory = null;
		}
	}

	public void Sort()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		List<ItemInstance> list = new List<ItemInstance>();
		for (int i = 0; i < allSlots.Count; i++)
		{
			Slot slot = allSlots[i];
			if (!(slot == null) && slot.HasValidItemInstance())
			{
				int uniqueIndex = slot.itemInstance.baseItem.UniqueIndex;
				if (!slot.itemInstance.HasMaxUses || slot.itemInstance.Amount > slot.itemInstance.baseItem.settings_Inventory.StackSize)
				{
					list.Add(slot.itemInstance.Clone());
				}
				else if (dictionary.ContainsKey(uniqueIndex))
				{
					dictionary[uniqueIndex] += slot.itemInstance.Amount;
				}
				else
				{
					dictionary.Add(uniqueIndex, slot.itemInstance.Amount);
				}
			}
		}
		Clear();
		foreach (KeyValuePair<int, int> item in dictionary)
		{
			Item_Base itemByIndex = ItemManager.GetItemByIndex(item.Key);
			int num = Mathf.FloorToInt((float)item.Value / (float)itemByIndex.settings_Inventory.StackSize);
			int num2 = item.Value % itemByIndex.settings_Inventory.StackSize;
			for (int j = 0; j < num; j++)
			{
				AddItem(itemByIndex.UniqueName, itemByIndex.settings_Inventory.StackSize);
			}
			if (num2 > 0)
			{
				AddItem(itemByIndex.UniqueName, num2);
			}
		}
		foreach (ItemInstance item2 in list.OrderBy((ItemInstance itemInstance) => itemInstance.baseItem.UniqueIndex))
		{
			AddItem(item2);
		}
	}

	public void SortPlayerInventoryIntoStorage()
	{
		if (secondInventory == null || !(secondInventory is PlayerInventory))
		{
			return;
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		for (int i = 0; i < allSlots.Count; i++)
		{
			Slot slot = allSlots[i];
			if (slot == null || !slot.HasValidItemInstance())
			{
				continue;
			}
			int uniqueIndex = slot.itemInstance.baseItem.UniqueIndex;
			if (slot.itemInstance.HasMaxUses && slot.itemInstance.Amount <= slot.itemInstance.baseItem.settings_Inventory.StackSize && (slot.itemInstance.BaseItemMaxUses <= 1 || slot.itemInstance.settings_Inventory.StackSize != 1))
			{
				if (dictionary.ContainsKey(uniqueIndex))
				{
					dictionary[uniqueIndex] += slot.itemInstance.Amount;
				}
				else
				{
					dictionary.Add(uniqueIndex, slot.itemInstance.Amount);
				}
			}
		}
		Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
		for (int j = 0; j < secondInventory.allSlots.Count; j++)
		{
			Slot slot2 = secondInventory.allSlots[j];
			if (slot2 == null || !slot2.HasValidItemInstance())
			{
				continue;
			}
			int uniqueIndex2 = slot2.itemInstance.baseItem.UniqueIndex;
			if (!slot2.itemInstance.HasMaxUses || slot2.itemInstance.Amount > slot2.itemInstance.baseItem.settings_Inventory.StackSize || (slot2.itemInstance.BaseItemMaxUses > 1 && slot2.itemInstance.settings_Inventory.StackSize == 1))
			{
				continue;
			}
			foreach (KeyValuePair<int, int> item in dictionary)
			{
				if (item.Key == uniqueIndex2)
				{
					if (dictionary2.ContainsKey(uniqueIndex2))
					{
						dictionary2[uniqueIndex2] += slot2.itemInstance.Amount;
					}
					else
					{
						dictionary2.Add(uniqueIndex2, slot2.itemInstance.Amount);
					}
				}
			}
		}
		foreach (KeyValuePair<int, int> item2 in dictionary2)
		{
			Item_Base itemByIndex = ItemManager.GetItemByIndex(item2.Key);
			int num = Mathf.FloorToInt((float)item2.Value / (float)itemByIndex.settings_Inventory.StackSize);
			int num2 = item2.Value % itemByIndex.settings_Inventory.StackSize;
			for (int k = 0; k < num; k++)
			{
				int num3 = AddItem(itemByIndex.UniqueName, itemByIndex.settings_Inventory.StackSize);
				secondInventory.RemoveItem(itemByIndex.UniqueName, itemByIndex.settings_Inventory.StackSize - num3);
			}
			if (num2 > 0)
			{
				int num4 = AddItem(itemByIndex.UniqueName, num2);
				secondInventory.RemoveItem(itemByIndex.UniqueName, num2 - num4);
			}
		}
	}

	public virtual int AddItem(string uniqueItemName, int amount)
	{
		if (localPlayerInventory == null)
		{
			localPlayerInventory = ComponentManager<PlayerInventory>.Value;
		}
		if (amount == 0)
		{
			return 0;
		}
		Item_Base itemByName = ItemManager.GetItemByName(uniqueItemName);
		if (itemByName == null)
		{
			return 0;
		}
		int result = amount;
		Slot slot = FindSuitableSlot(itemByName);
		if (slot != null)
		{
			if (slot.IsEmpty)
			{
				slot.SetItem(itemByName, amount);
				result = 0;
				if (localPlayerInventory.hotbar.IsSelectedHotSlot(slot))
				{
					localPlayerInventory.hotbar.ReselectCurrentSlot();
				}
			}
			else
			{
				result = AddItemRepetetivly(itemByName, amount);
			}
		}
		else if (localPlayerInventory == this)
		{
			localPlayerInventory.DropItem(itemByName, amount);
			result = 0;
		}
		if (soundManager != null)
		{
			soundManager.PlayUI_MoveItem();
		}
		return result;
	}

	public virtual void AddItem(string uniqueItemName, Slot slot, int amount)
	{
		if (localPlayerInventory == null)
		{
			localPlayerInventory = ComponentManager<PlayerInventory>.Value;
		}
		if (amount == 0)
		{
			return;
		}
		Item_Base itemByName = ItemManager.GetItemByName(uniqueItemName);
		if (itemByName == null)
		{
			return;
		}
		if (itemByName.settings_Inventory.Stackable)
		{
			AddItemRepetetivly(itemByName, amount);
		}
		else if (slot.IsEmpty)
		{
			slot.SetItem(itemByName, amount);
			if (localPlayerInventory.hotbar.IsSelectedHotSlot(slot))
			{
				localPlayerInventory.hotbar.ReselectCurrentSlot();
			}
		}
		if (soundManager != null)
		{
			soundManager.PlayUI_MoveItem();
		}
	}

	public virtual void AddItem(ItemInstance itemInstance, bool dropIfFull = true)
	{
		if (localPlayerInventory == null)
		{
			localPlayerInventory = ComponentManager<PlayerInventory>.Value;
		}
		if (itemInstance == null)
		{
			return;
		}
		Slot slot = FindSuitableSlot(itemInstance.baseItem);
		if (slot != null)
		{
			int num = itemInstance.Uses + (itemInstance.Amount - 1) * itemInstance.baseItem.MaxUses;
			if (slot.IsEmpty)
			{
				slot.SetItem(itemInstance.Clone());
				num = 0;
				if (localPlayerInventory.hotbar.IsSelectedHotSlot(slot))
				{
					localPlayerInventory.hotbar.ReselectCurrentSlot();
				}
			}
			else
			{
				while (num > 0)
				{
					if (slot.StackIsFull())
					{
						slot = FindSuitableSlot(itemInstance.baseItem);
					}
					if (slot == null)
					{
						break;
					}
					if (slot.IsEmpty)
					{
						int amount = Mathf.CeilToInt((float)num / (float)itemInstance.baseItem.MaxUses);
						int num2 = num % itemInstance.baseItem.MaxUses;
						if (num2 == 0)
						{
							num2 = itemInstance.baseItem.MaxUses;
						}
						itemInstance.Amount = amount;
						itemInstance.Uses = num2;
						slot.SetItem(itemInstance.Clone());
						num = 0;
					}
					else
					{
						int maxStackUses = slot.GetMaxStackUses();
						int currentTotalUses = slot.GetCurrentTotalUses();
						int num3 = maxStackUses - currentTotalUses;
						if (num3 >= num)
						{
							SetSlotAmountFromUses(slot, currentTotalUses + num);
							num = 0;
						}
						else
						{
							SetSlotAmountFromUses(slot, maxStackUses);
							num -= num3;
						}
					}
				}
			}
			if (num > 0)
			{
				int amount2 = Mathf.CeilToInt((float)num / (float)itemInstance.baseItem.MaxUses);
				int num4 = num % itemInstance.baseItem.MaxUses;
				if (num4 == 0)
				{
					num4 = itemInstance.baseItem.MaxUses;
				}
				itemInstance.Amount = amount2;
				itemInstance.Uses = num4;
				if (dropIfFull && localPlayerInventory == this)
				{
					localPlayerInventory.DropItem(itemInstance);
				}
			}
			else
			{
				itemInstance.Amount = 0;
				itemInstance.Uses = 0;
			}
		}
		else if (dropIfFull)
		{
			localPlayerInventory.DropItem(itemInstance);
		}
		if (soundManager != null)
		{
			soundManager.PlayUI_MoveItem();
		}
	}

	public void RemoveItem(string uniqueItemName, int amount)
	{
		if (amount == 0)
		{
			return;
		}
		Item_Base itemByName = ItemManager.GetItemByName(uniqueItemName);
		if (itemByName == null)
		{
			return;
		}
		Slot slot = null;
		foreach (Slot allSlot in allSlots)
		{
			if (amount > 0 && !allSlot.IsEmpty && allSlot.itemInstance.UniqueIndex == itemByName.UniqueIndex)
			{
				slot = allSlot;
				if (allSlot.itemInstance.Amount >= amount)
				{
					allSlot.RemoveItem(amount);
					break;
				}
				amount -= allSlot.itemInstance.Amount;
				allSlot.RemoveItem(allSlot.itemInstance.Amount);
			}
		}
		if (localPlayerInventory.hotbar.IsSelectedHotSlot(slot))
		{
			localPlayerInventory.hotbar.ReselectCurrentSlot();
		}
	}

	public void RemoveItemUses(string uniqueItemName, int usesToRemove, bool addItemAfterUseToInventory = true)
	{
		if (usesToRemove == 0)
		{
			return;
		}
		Item_Base itemByName = ItemManager.GetItemByName(uniqueItemName);
		if (itemByName == null)
		{
			return;
		}
		Slot slot = null;
		foreach (Slot allSlot in allSlots)
		{
			if (usesToRemove > 0 && !allSlot.IsEmpty && allSlot.itemInstance.UniqueIndex == itemByName.UniqueIndex)
			{
				slot = allSlot;
				if (allSlot.itemInstance.UsesInStack >= usesToRemove)
				{
					allSlot.IncrementUses(-usesToRemove, addItemAfterUseToInventory);
					break;
				}
				usesToRemove -= allSlot.itemInstance.UsesInStack;
				allSlot.IncrementUses(-allSlot.itemInstance.UsesInStack, addItemAfterUseToInventory);
			}
		}
		if (localPlayerInventory.hotbar.IsSelectedHotSlot(slot))
		{
			localPlayerInventory.hotbar.ReselectCurrentSlot();
		}
	}

	public void RemoveCostMultiple(CostMultiple[] costMultiple, bool manipulateCostAmount = false)
	{
		foreach (CostMultiple costMultiple2 in costMultiple)
		{
			int num = costMultiple2.amount;
			for (int j = 0; j < costMultiple2.items.Length; j++)
			{
				int itemCount = GetItemCount(costMultiple2.items[j].UniqueName);
				if (itemCount >= num)
				{
					RemoveItem(costMultiple2.items[j].UniqueName, num);
					num = 0;
					if (manipulateCostAmount)
					{
						costMultiple2.amount = 0;
					}
				}
				else
				{
					RemoveItem(costMultiple2.items[j].UniqueName, itemCount);
					num -= itemCount;
					if (manipulateCostAmount)
					{
						costMultiple2.amount -= itemCount;
					}
				}
				if (num <= 0)
				{
					break;
				}
			}
		}
	}

	public void RemoveCostMultipleIncludeSecondaryInventories(CostMultiple[] costMultiple)
	{
		if (secondInventory == null)
		{
			RemoveCostMultiple(costMultiple);
			return;
		}
		CostMultiple[] array = new CostMultiple[costMultiple.Length];
		for (int i = 0; i < costMultiple.Length; i++)
		{
			array[i] = new CostMultiple(costMultiple[i].items, costMultiple[i].amount);
		}
		RemoveCostMultiple(array, manipulateCostAmount: true);
		secondInventory.RemoveCostMultiple(array, manipulateCostAmount: true);
	}

	private int AddItemRepetetivly(Item_Base item, int amount)
	{
		bool flag = false;
		Slot slot = FindSuitableSlot(item);
		while (amount > 0)
		{
			if (slot.StackIsFull())
			{
				slot = FindSuitableSlot(item);
			}
			if (slot == null)
			{
				break;
			}
			slot.AddItem(item);
			amount--;
			if (localPlayerInventory.hotbar.IsSelectedHotSlot(slot))
			{
				flag = true;
			}
		}
		bool flag2 = false;
		if (amount > 0 && this is PlayerInventory)
		{
			if (slot == null)
			{
				localPlayerInventory.DropItem(item, amount);
			}
			else
			{
				localPlayerInventory.DropItem(slot.itemInstance.Clone());
			}
			flag2 = true;
		}
		if (flag)
		{
			localPlayerInventory.hotbar.ReselectCurrentSlot();
		}
		if (flag2)
		{
			return 0;
		}
		return amount;
	}

	protected virtual void InitializeSlots()
	{
		allSlots.AddRange(GetComponentsInChildren<Slot>());
		foreach (Slot allSlot in allSlots)
		{
			allSlot.InitializeEventListeners(this);
		}
	}

	public void ResetMoveSlots()
	{
		fromSlot = (toSlot = null);
	}

	public void HoverEnter(Slot slot, PointerEventData eventData)
	{
		if (soundManager == null)
		{
			soundManager = ComponentManager<SoundManager>.Value;
		}
		if (localPlayerInventory == null)
		{
			localPlayerInventory = ComponentManager<PlayerInventory>.Value;
		}
		if (!dragging || !(fromSlot != null) || fromSlot.slotType != SlotType.Equipment || fromSlot.itemInstance.settings_equipment.EquipType != EquipSlotType.Backpack || slot.slotType != SlotType.Backpack)
		{
			hoverSlot = slot;
			if (hoverSlot != null && (!hoverSlot.IsEmpty || dragging) && soundManager != null)
			{
				soundManager.PlayUI_Highlight();
			}
			if (fromSlot != null && fromSlot != toSlot)
			{
				toSlot = slot;
			}
			if (localPlayerInventory != null)
			{
				localPlayerInventory.SetItemDescription(hoverSlot.GetItemBase());
			}
		}
	}

	public void HoverExit(Slot slot, PointerEventData eventData)
	{
		if (fromSlot != null && toSlot == slot)
		{
			toSlot = null;
		}
		if (toSlot == null)
		{
			hoverSlot = null;
		}
		localPlayerInventory.SetItemDescription(null);
	}

	public void MoveItem(Slot slot, PointerEventData.InputButton button)
	{
		if (CanvasHelper.ActiveMenu == MenuType.PauseMenu)
		{
			return;
		}
		if (fromSlot == null)
		{
			fromSlot = slot;
			if (!fromSlot.IsEmpty)
			{
				switch (button)
				{
				case PointerEventData.InputButton.Left:
					dragAmount = fromSlot.itemInstance.Amount;
					break;
				case PointerEventData.InputButton.Right:
					dragAmount = 1;
					break;
				case PointerEventData.InputButton.Middle:
					dragAmount = Mathf.CeilToInt((float)fromSlot.itemInstance.Amount / 2f);
					break;
				}
			}
			else
			{
				fromSlot = null;
				dragAmount = 0;
			}
		}
		else if (toSlot == null)
		{
			if (!EventSystem.current.IsPointerOverGameObject() && !fromSlot.IsEmpty)
			{
				if (dragAmount == fromSlot.itemInstance.Amount)
				{
					localPlayerInventory.DropItem(fromSlot);
					fromSlot?.SetItem(null);
				}
				else
				{
					ItemInstance itemInstance = fromSlot.itemInstance.Clone();
					itemInstance.Amount = dragAmount;
					fromSlot.RemoveItem(dragAmount);
					localPlayerInventory.DropItem(itemInstance);
				}
			}
			fromSlot = (toSlot = null);
		}
		else if (fromSlot == toSlot)
		{
			fromSlot = (toSlot = null);
		}
		else
		{
			if (!(fromSlot != null) || !(toSlot != null))
			{
				return;
			}
			if (fromSlot.IsEmpty || (fromSlot.IsEmpty && toSlot.IsEmpty))
			{
				fromSlot = (toSlot = null);
				return;
			}
			bool flag = toSlot.slotType == SlotType.Equipment;
			bool flag2 = fromSlot.slotType == SlotType.Equipment;
			if (toSlot.IsEmpty)
			{
				if (!flag | flag2)
				{
					MoveSlotToEmpty(fromSlot, toSlot, dragAmount);
				}
				else
				{
					if (fromSlot.itemInstance.settings_equipment.EquipType == EquipSlotType.None)
					{
						fromSlot = (toSlot = null);
						return;
					}
					Slot_Equip equipSlotWithTag = Slot_Equip.GetEquipSlotWithTag(fromSlot.itemInstance.settings_equipment.EquipType);
					if (equipSlotWithTag != null)
					{
						if (fromSlot.itemInstance.settings_equipment.EquipType == EquipSlotType.Backpack)
						{
							fromSlot = (toSlot = null);
							return;
						}
						ItemInstance itemInstance2 = equipSlotWithTag.itemInstance.Clone();
						equipSlotWithTag.SetItem(null);
						toSlot.SetItem(fromSlot.itemInstance);
						toSlot.itemInstance.Amount = 1;
						fromSlot.RemoveItem(1);
						if (fromSlot.IsEmpty)
						{
							fromSlot.SetItem(itemInstance2);
						}
						else
						{
							AddItem(itemInstance2);
						}
					}
					else
					{
						toSlot.SetItem(fromSlot.itemInstance);
						toSlot.itemInstance.Amount = 1;
						fromSlot.RemoveItem(1);
					}
					if (soundManager != null)
					{
						soundManager.PlayUI_MoveItem();
					}
				}
			}
			else
			{
				if (((toSlot.itemInstance != null) & flag) && toSlot.itemInstance.settings_equipment.EquipType == EquipSlotType.Backpack)
				{
					fromSlot = (toSlot = null);
					return;
				}
				if (fromSlot.itemInstance.settings_Inventory.Stackable && fromSlot.itemInstance.UniqueIndex == toSlot.itemInstance.UniqueIndex && !flag)
				{
					if (!toSlot.StackIsFull())
					{
						StackSlots(fromSlot, toSlot, dragAmount);
					}
				}
				else
				{
					SwitchSlots(fromSlot, toSlot);
				}
			}
			if (localPlayerInventory.hotbar.IsSelectedHotSlot(fromSlot) || localPlayerInventory.hotbar.IsSelectedHotSlot(toSlot))
			{
				localPlayerInventory.hotbar.ReselectCurrentSlot();
			}
			fromSlot = (toSlot = null);
		}
	}

	public virtual void ShiftMoveItem(Slot slot)
	{
		if (secondInventory == null || slot.IsEmpty)
		{
			return;
		}
		secondInventory.AddItem(slot.itemInstance, dropIfFull: false);
		if (slot.itemInstance.Amount == 0)
		{
			slot.SetItem(null);
			if (localPlayerInventory.hotbar.IsSelectedHotSlot(slot))
			{
				localPlayerInventory.hotbar.ReselectCurrentSlot();
			}
		}
		else
		{
			slot.RefreshComponents();
		}
	}

	public virtual void CanShiftMove(Slot slot)
	{
		if (secondInventory == null || slot.IsEmpty)
		{
			return;
		}
		secondInventory.AddItem(slot.itemInstance, dropIfFull: false);
		if (slot.itemInstance.Amount == 0)
		{
			slot.SetItem(null);
			if (localPlayerInventory.hotbar.IsSelectedHotSlot(slot))
			{
				localPlayerInventory.hotbar.ReselectCurrentSlot();
			}
		}
		else
		{
			slot.RefreshComponents();
		}
	}

	protected virtual void StackSlots(Slot fromSlot, Slot toSlot, int dragAmount)
	{
		int maxUses = toSlot.itemInstance.baseItem.MaxUses;
		int num = toSlot.itemInstance.baseItem.settings_Inventory.StackSize * maxUses;
		int num2 = fromSlot.itemInstance.Uses + (dragAmount - 1) * maxUses;
		int num3 = toSlot.itemInstance.Uses + (toSlot.itemInstance.Amount - 1) * maxUses;
		int num4 = num2 + num3;
		if (num4 <= num)
		{
			int totalUses = fromSlot.itemInstance.Uses + (fromSlot.itemInstance.Amount - 1) * maxUses - num2;
			SetSlotAmountFromUses(fromSlot, totalUses);
			SetSlotAmountFromUses(toSlot, num4);
		}
		else
		{
			int num5 = num4 - num;
			if (fromSlot.itemInstance.BaseItemMaxUses == 1 && dragAmount < fromSlot.itemInstance.Amount)
			{
				int num6 = fromSlot.itemInstance.Amount - dragAmount;
				SetSlotAmountFromUses(fromSlot, num6 * maxUses + num5);
			}
			else
			{
				SetSlotAmountFromUses(fromSlot, num5);
			}
			SetSlotAmountFromUses(toSlot, num);
		}
		if (soundManager != null)
		{
			soundManager.PlayUI_MoveItem();
		}
	}

	protected virtual void MoveSlotToEmpty(Slot fromSlot, Slot toSlot, int amount)
	{
		if (fromSlot.slotType != SlotType.Equipment || toSlot.slotType != SlotType.Backpack || fromSlot.itemInstance.settings_equipment.EquipType != EquipSlotType.Backpack)
		{
			ItemInstance itemInstance = fromSlot.itemInstance.Clone();
			itemInstance.Amount = amount;
			if (!fromSlot.itemInstance.HasMaxUses && fromSlot.itemInstance.Amount - amount > 0)
			{
				itemInstance.SetUsesToMax();
			}
			fromSlot.RemoveItem(amount);
			toSlot.SetItem(itemInstance);
			if (soundManager != null)
			{
				soundManager.PlayUI_MoveItem();
			}
		}
	}

	protected virtual void SwitchSlots(Slot fromSlot, Slot toSlot)
	{
		bool flag = toSlot.slotType == SlotType.Equipment;
		bool flag2 = fromSlot.slotType == SlotType.Equipment;
		bool flag3 = fromSlot.itemInstance.settings_equipment.EquipType != EquipSlotType.None && toSlot.itemInstance.settings_equipment.EquipType != EquipSlotType.None;
		if (!flag2 && !flag)
		{
			ItemInstance item = fromSlot.itemInstance.Clone();
			if (toSlot.itemInstance != null)
			{
				fromSlot.SetItem(toSlot.itemInstance.Clone());
			}
			else
			{
				fromSlot.SetItem(null);
			}
			toSlot.SetItem(item);
			if (soundManager != null)
			{
				soundManager.PlayUI_MoveItem();
			}
		}
		else if (flag3 && ((flag2 && flag) || !(Slot_Equip.GetEquipSlotWithTag(((!flag) ? toSlot : fromSlot).itemInstance.settings_equipment.EquipType) != null)))
		{
			ItemInstance itemInstance = toSlot.itemInstance;
			ItemInstance itemInstance2 = fromSlot.itemInstance;
			toSlot.SetItem(null);
			fromSlot.SetItem(null);
			toSlot.SetItem(itemInstance2);
			fromSlot.SetItem(itemInstance);
			if (soundManager != null)
			{
				soundManager.PlayUI_MoveItem();
			}
		}
	}

	private void SetSlotAmountFromUses(Slot slot, int totalUses)
	{
		int maxUses = slot.itemInstance.baseItem.MaxUses;
		int num = Mathf.CeilToInt((float)totalUses / (float)maxUses);
		int num2 = totalUses % maxUses;
		if (num2 == 0)
		{
			num2 = maxUses;
		}
		if (num == 0)
		{
			slot.SetItem(null);
			return;
		}
		slot.itemInstance.Amount = num;
		slot.itemInstance.Uses = num2;
		slot.RefreshComponents();
	}

	public int GetItemCount(string uniqueItemName)
	{
		if (GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.unlimitedResources)
		{
			return int.MaxValue;
		}
		int num = 0;
		foreach (Slot allSlot in allSlots)
		{
			if (!allSlot.IsEmpty && allSlot.itemInstance.UniqueName == uniqueItemName)
			{
				num += allSlot.itemInstance.Amount;
			}
		}
		return num;
	}

	public int GetItemCount(Item_Base item)
	{
		if (GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.unlimitedResources)
		{
			return int.MaxValue;
		}
		if (item == null)
		{
			return 0;
		}
		int num = 0;
		foreach (Slot allSlot in allSlots)
		{
			if (!allSlot.IsEmpty && allSlot.itemInstance.baseItem.UniqueIndex == item.UniqueIndex)
			{
				num += allSlot.itemInstance.Amount;
			}
		}
		return num;
	}

	public float GetNormalizedFullAmount()
	{
		if (!allSlots.ContainsItems())
		{
			return 0f;
		}
		int num = 0;
		for (int i = 0; i < allSlots.Count; i++)
		{
			Slot slot = allSlots[i];
			if (slot != null && !slot.IsEmpty)
			{
				num++;
			}
		}
		return Mathf.Clamp01((float)num / (float)allSlots.Count);
	}

	public int GetSlotCount()
	{
		return allSlots.Count;
	}

	public Slot GetSlot(int index)
	{
		if (index >= 0 && index < allSlots.Count)
		{
			return allSlots[index];
		}
		return null;
	}

	public RGD_Slot[] GetRGDSlots()
	{
		List<RGD_Slot> list = new List<RGD_Slot>();
		for (int i = 0; i < allSlots.Count; i++)
		{
			Slot slot = allSlots[i];
			if (!(slot == null) && !slot.IsEmpty)
			{
				list.Add(new RGD_Slot(slot, i));
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray();
		}
		return null;
	}

	public void SetSlotsFromRGD(RGD_Slot[] slots)
	{
		if (slots == null)
		{
			Clear();
			return;
		}
		for (int i = 0; i < allSlots.Count; i++)
		{
			Slot slot = allSlots[i];
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

	public void Show()
	{
		if (this != null && gameObject != null)
		{
			gameObject.SetActive(value: true);
		}
	}

	public void Hide()
	{
		if (this != null && gameObject != null)
		{
			gameObject.SetActive(value: false);
		}
	}

	public void SetLocalPosition(Vector3 position)
	{
		invRectTransform.localPosition = position;
	}

	public virtual void Clear()
	{
		foreach (Slot allSlot in allSlots)
		{
			allSlot.Reset();
		}
	}

	protected virtual Slot FindFirstEmptySlot()
	{
		foreach (Slot allSlot in allSlots)
		{
			if (allSlot.IsEmpty && allSlot.active)
			{
				return allSlot;
			}
		}
		return null;
	}

	private Slot FindFirstStackableSlot(int uniqueItemIndex)
	{
		foreach (Slot allSlot in allSlots)
		{
			if (!allSlot.IsEmpty && allSlot.itemInstance.UniqueIndex == uniqueItemIndex && !allSlot.StackIsFull())
			{
				return allSlot;
			}
		}
		return null;
	}

	public virtual Slot FindSuitableSlot(Item_Base stackableItem = null)
	{
		bool flag = stackableItem != null && stackableItem.settings_Inventory.Stackable;
		Slot slot = null;
		foreach (Slot allSlot in allSlots)
		{
			if (allSlot.IsEmpty && allSlot.active && slot == null)
			{
				slot = allSlot;
			}
			if (flag && allSlot.active && !allSlot.StackIsFull() && !allSlot.IsEmpty && allSlot.itemInstance.UniqueIndex == stackableItem.UniqueIndex)
			{
				return allSlot;
			}
		}
		return slot;
	}

	private void ResetQuickEquip()
	{
		canQuickEquip = true;
	}

	protected virtual bool ContainsSlot(Slot slot)
	{
		return allSlots.Contains(slot);
	}

	private void OnMenuClose(MenuType menuType)
	{
		if (menuType == MenuType.Inventory)
		{
			toSlot = null;
			fromSlot = null;
			hoverSlot = null;
			canvas.dropText.gameObject.SetActive(value: false);
		}
	}
}
