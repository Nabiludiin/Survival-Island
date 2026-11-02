using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
	[HideInInspector]
	public Network_Player playerNetwork;

	[SerializeField]
	private InputActionReference inputRight;

	[SerializeField]
	private InputActionReference inputLeft;

	[SerializeField]
	private GameObject keyboardLayout;

	[SerializeField]
	private GameObject gamepadLayout;

	[SerializeField]
	private GameObject gamepadLayout2;

	[SerializeField]
	private GameObject gamepadLayoutLb;

	[SerializeField]
	private GameObject gamepadLayoutRb;

	[SerializeField]
	private GameObject gamepadIcons;

	[SerializeField]
	private GameObject gamepadHints;

	[SerializeField]
	private GameObject addAllowed;

	[SerializeField]
	private GameObject selectAllowed;

	[SerializeField]
	private GameObject moveAllowed;

	[SerializeField]
	private GameObject dropAllowed;

	[SerializeField]
	private GameObject dropItemAllowed;

	[SerializeField]
	private GameObject moveOneAllowed;

	[SerializeField]
	private GameObject moveHalfAllowed;

	[SerializeField]
	private GameObject moveRepatAllowed;

	[SerializeField]
	private GameObject craftAllowed;

	[SerializeField]
	private GameObject quickMoveAllowed;

	[SerializeField]
	private GameObject swapAllowed;

	[SerializeField]
	private GameObject researchAllowed;

	[SerializeField]
	private GameObject storeAllowed;

	[SerializeField]
	private GameObject sortAllowed;

	[SerializeField]
	private GameObject trashAllowed;

	[SerializeField]
	private GameObject hotbarBackground;

	[SerializeField]
	private GameObject hotbarForeground;

	private Slot selectedSlot;

	private int slotIndex;

	private string[] hotbarKeys;

	private PlayerInventory inventory;

	private CanvasHelper canvasHelper;

	private GamepadCursor cursor;

	private void Awake()
	{
		Raft_Network.OnPlayerInitialized += OnPlayerInitialize;
	}

	public void Start()
	{
		hotbarKeys = new string[10] { "Hotbar0", "Hotbar1", "Hotbar2", "Hotbar3", "Hotbar4", "Hotbar5", "Hotbar6", "Hotbar7", "Hotbar8", "Hotbar9" };
		inventory = ComponentManager<PlayerInventory>.Value;
		canvasHelper = ComponentManager<CanvasHelper>.Value;
		cursor = canvasHelper.GetComponent<GamepadCursor>();
		CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
		instance.OnControlsChangedEvent = (Action<bool>)Delegate.Combine(instance.OnControlsChangedEvent, new Action<bool>(OnControlsChanged));
		OnControlsChanged(CanvasHelper.Gamepad);
	}

	private void OnControlsChanged(bool gamepad)
	{
		if (gamepad)
		{
			keyboardLayout.SetActive(value: false);
			gamepadLayout.SetActive(value: true);
			gamepadLayout2.SetActive(value: true);
			gamepadIcons.SetActive(value: true);
		}
		else
		{
			keyboardLayout.SetActive(value: true);
			gamepadLayout.SetActive(value: false);
			gamepadLayout2.SetActive(value: false);
			gamepadIcons.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (playerNetwork == null || !playerNetwork.IsLocalPlayer)
		{
			return;
		}
		HandleHotbarSelection();
		bool active = GeneralSettingsBox.HelpfulUIVisible && CanvasHelper.ActiveMenu == MenuType.None && playerNetwork.PlayerItemManager.CanSwitch();
		gamepadLayoutLb.SetActive(active);
		gamepadLayoutRb.SetActive(active);
		gamepadIcons.SetActive(active);
		gamepadHints.SetActive(CanvasHelper.ActiveMenu == MenuType.Inventory);
		UiMagnet magnetUnderCursor = cursor.MagnetUnderCursor;
		cursor.SetMask(uint.MaxValue);
		if (Inventory.fromSlot != null && Inventory.fromSlot.itemInstance != null)
		{
			if (Inventory.fromSlot.itemInstance.settings_equipment.EquipType != EquipSlotType.None)
			{
				cursor.SetMask(4u);
			}
			else
			{
				cursor.SetMask(2u);
			}
			if (Inventory.hoverSlot == null)
			{
				addAllowed.SetActive(value: false);
				selectAllowed.SetActive(value: false);
				moveAllowed.SetActive(value: false);
				dropAllowed.SetActive(GeneralSettingsBox.HelpfulUIVisible);
				dropItemAllowed.SetActive(value: false);
				moveOneAllowed.SetActive(value: false);
				moveHalfAllowed.SetActive(value: false);
				moveRepatAllowed.SetActive(value: false);
				quickMoveAllowed.SetActive(value: false);
				swapAllowed.SetActive(value: false);
			}
			else
			{
				addAllowed.SetActive(value: false);
				selectAllowed.SetActive(value: false);
				moveAllowed.SetActive(GeneralSettingsBox.HelpfulUIVisible);
				dropAllowed.SetActive(value: false);
				dropItemAllowed.SetActive(value: false);
				moveOneAllowed.SetActive(GeneralSettingsBox.HelpfulUIVisible && Inventory.fromSlot.itemInstance.Amount > 1 && (Inventory.hoverSlot.IsEmpty || (Inventory.hoverSlot.itemInstance.UniqueIndex == Inventory.fromSlot.itemInstance.UniqueIndex && !Inventory.hoverSlot.StackIsFull() && Inventory.fromSlot != Inventory.hoverSlot)));
				moveHalfAllowed.SetActive(value: false);
				moveRepatAllowed.SetActive(value: false);
				quickMoveAllowed.SetActive(value: false);
				swapAllowed.SetActive(value: false);
			}
		}
		else if (Inventory.hoverSlot != null)
		{
			addAllowed.SetActive(value: false);
			selectAllowed.SetActive(value: false);
			moveAllowed.SetActive(GeneralSettingsBox.HelpfulUIVisible && !Inventory.hoverSlot.IsEmpty);
			dropAllowed.SetActive(value: false);
			dropItemAllowed.SetActive(GeneralSettingsBox.HelpfulUIVisible && !Inventory.hoverSlot.IsEmpty);
			moveOneAllowed.SetActive(value: false);
			moveHalfAllowed.SetActive(GeneralSettingsBox.HelpfulUIVisible && !Inventory.hoverSlot.IsEmpty && Inventory.hoverSlot.itemInstance.Amount > 1);
			moveRepatAllowed.SetActive(GeneralSettingsBox.HelpfulUIVisible && !Inventory.hoverSlot.IsEmpty && Inventory.hoverSlot.itemInstance.Amount > 1);
			quickMoveAllowed.SetActive(GeneralSettingsBox.HelpfulUIVisible && !Inventory.hoverSlot.IsEmpty && inventory.CanShiftMove(Inventory.hoverSlot));
			swapAllowed.SetActive(GeneralSettingsBox.HelpfulUIVisible && !Inventory.hoverSlot.IsEmpty);
		}
		else
		{
			if (magnetUnderCursor != null && magnetUnderCursor.UserInt == 1)
			{
				LocalizationParameters.itemX = magnetUnderCursor.GetComponent<BuildingUI_Costbox_Sub_Crafting>().item.settings_Inventory.DisplayName;
				addAllowed.GetComponentInChildren<Text>().text = Helper.GetTerm("Game/CraftX", applyParameters: true);
				addAllowed.SetActive(GeneralSettingsBox.HelpfulUIVisible);
				selectAllowed.SetActive(value: false);
			}
			else
			{
				addAllowed.SetActive(value: false);
				selectAllowed.SetActive(GeneralSettingsBox.HelpfulUIVisible && magnetUnderCursor != null && magnetUnderCursor.UserInt != 2);
			}
			moveAllowed.SetActive(value: false);
			dropAllowed.SetActive(value: false);
			dropItemAllowed.SetActive(value: false);
			moveOneAllowed.SetActive(value: false);
			moveHalfAllowed.SetActive(value: false);
			moveRepatAllowed.SetActive(value: false);
			quickMoveAllowed.SetActive(value: false);
			swapAllowed.SetActive(value: false);
		}
		if (ComponentManager<CraftingMenu>.ValueConsole.selectedRecipeBox.craftButton.interactable && (bool)ComponentManager<CraftingMenu>.ValueConsole.selectedRecipeBox.ItemToCraft)
		{
			craftAllowed.SetActive(value: true);
		}
		else
		{
			craftAllowed.SetActive(value: false);
		}
		if (Inventory_ResearchTable.ResearchItem != null && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			researchAllowed.SetActive(value: true);
		}
		else
		{
			researchAllowed.SetActive(value: false);
		}
		bool flag = (bool)playerNetwork.StorageManager.currentStorage && playerNetwork.StorageManager.currentStorage.IsOpen;
		sortAllowed.SetActive(flag);
		if (flag)
		{
			if (playerNetwork.StorageManager.currentStorage.buildableItem.UniqueIndex == 389)
			{
				trashAllowed.SetActive(value: true);
				storeAllowed.SetActive(value: false);
			}
			else
			{
				trashAllowed.SetActive(value: false);
				storeAllowed.SetActive(value: true);
			}
		}
		else
		{
			trashAllowed.SetActive(value: false);
			storeAllowed.SetActive(value: false);
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			hotbarBackground.SetActive(CanvasHelper.ActiveMenu != MenuType.Piano);
			hotbarForeground.SetActive(CanvasHelper.ActiveMenu != MenuType.Piano);
		}
		else
		{
			hotbarBackground.SetActive(value: true);
			hotbarForeground.SetActive(value: true);
		}
	}

	private void OnDestroy()
	{
		playerNetwork = null;
		inventory = null;
		canvasHelper = null;
		Raft_Network.OnPlayerInitialized -= OnPlayerInitialize;
		if ((bool)SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance)
		{
			CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
			instance.OnControlsChangedEvent = (Action<bool>)Delegate.Remove(instance.OnControlsChangedEvent, new Action<bool>(OnControlsChanged));
		}
	}

	private void OnPlayerInitialize(Network_Player player)
	{
		if (player.IsLocalPlayer)
		{
			playerNetwork = player;
		}
	}

	public void SelectHotslot(Slot slot)
	{
		if (playerNetwork.IsLocalPlayer)
		{
			if (selectedSlot != null)
			{
				selectedSlot.Focus(value: false);
				selectedSlot = null;
			}
			if (slot != null)
			{
				slot.Focus(value: true);
				selectedSlot = slot;
			}
			if (slot == null || slot.IsEmpty)
			{
				SelectItem(null);
			}
			else
			{
				SelectItem(slot.itemInstance.baseItem);
			}
		}
	}

	public void ReselectCurrentSlot()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			Slot slot = playerNetwork.Inventory.GetSlot(slotIndex);
			SelectHotslot(slot);
		}
	}

	public int GetSelectedSlotIndex()
	{
		return slotIndex;
	}

	public void SetSelectedSlotIndex(int index)
	{
		if (playerNetwork.IsLocalPlayer)
		{
			slotIndex = index;
		}
	}

	public bool IsSelectedHotSlot(Slot slot)
	{
		if (slot == null)
		{
			return false;
		}
		return playerNetwork.Inventory.GetSlot(slotIndex) == slot;
	}

	public bool ContainsSlot(Slot slot)
	{
		for (int i = 0; i < playerNetwork.Inventory.hotslotCount; i++)
		{
			if (playerNetwork.Inventory.allSlots[i] == slot)
			{
				return true;
			}
		}
		return false;
	}

	public Slot GetSelectedSlot()
	{
		return selectedSlot;
	}

	private void HandleHotbarSelection()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None || playerNetwork.PlayerItemManager == null || Player.LocalPlayerIsDead || playerNetwork.RessurectComponent.IsCarrying || !playerNetwork.PlayerItemManager.CanSwitch() || playerNetwork.BlockCreator.IsRotating)
		{
			return;
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			if (inputRight.action.WasPressedThisFrame())
			{
				int num = GetSelectedSlotIndex() + 1;
				if (num == hotbarKeys.Length)
				{
					num = 0;
				}
				slotIndex = num;
				Slot slot = playerNetwork.Inventory.GetSlot(num);
				SelectHotslot(slot);
			}
			else if (inputLeft.action.WasPressedThisFrame())
			{
				int num2 = GetSelectedSlotIndex() - 1;
				if (num2 < 0)
				{
					num2 = hotbarKeys.Length - 1;
				}
				slotIndex = num2;
				Slot slot2 = playerNetwork.Inventory.GetSlot(num2);
				SelectHotslot(slot2);
			}
			return;
		}
		for (int i = 0; i < hotbarKeys.Length; i++)
		{
			if (MyInput.GetButtonDown(hotbarKeys[i]))
			{
				slotIndex = i;
				Slot slot3 = playerNetwork.Inventory.GetSlot(slotIndex);
				SelectHotslot(slot3);
			}
		}
		if (Input.GetKey(KeyCode.LeftAlt))
		{
			return;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			slotIndex--;
		}
		else if (axis < 0f)
		{
			slotIndex++;
		}
		if (axis > 0f || axis < 0f)
		{
			int hotslotCount = playerNetwork.Inventory.hotslotCount;
			if (slotIndex > hotslotCount - 1)
			{
				slotIndex = 0;
			}
			else if (slotIndex < 0)
			{
				slotIndex = hotslotCount - 1;
			}
			Slot slot4 = playerNetwork.Inventory.GetSlot(slotIndex);
			SelectHotslot(slot4);
		}
	}

	private void SelectItem(Item_Base item)
	{
		if (!(playerNetwork.PlayerItemManager == null))
		{
			if (item == null || !item.settings_usable.IsUsable())
			{
				playerNetwork.PlayerItemManager.SelectUsable(null);
			}
			else
			{
				playerNetwork.PlayerItemManager.SelectUsable(item);
			}
		}
	}
}
