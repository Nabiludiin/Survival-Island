using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
public class CookingTable_Slot : MonoBehaviour, IRaycastable
{
	public Action<Network_Player, CookingTable_Slot, ItemInstance> OnInsertItem;

	public Action<Network_Player, CookingTable_Slot, ItemInstance> OnPickupItem;

	[SerializeField]
	private int index;

	[SerializeField]
	private ItemObjectEnabler objectEnabler;

	[SerializeField]
	private Animator anim;

	[SerializeField]
	private float hideItemDelay;

	public EventReference er_plums;

	public EventReference er_placeItem;

	private ItemInstance currentItem;

	private CookingTable cookingPot;

	private bool initialized;

	public ItemInstance CurrentItem => currentItem;

	public bool HasItem => CurrentItem != null;

	public int Index => index;

	public void Initialize(CookingTable cookingPot)
	{
		initialized = true;
		this.cookingPot = cookingPot;
	}

	private void HandlePickup()
	{
		LocalizationParameters.itemX = ItemManager.GetItemByIndex(currentItem.UniqueIndex).settings_Inventory.DisplayName;
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		cookingPot.displayText.ShowText(Helper.GetTerm("Game/PickUpX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			cookingPot.displayText.HideDisplayTexts();
			OnPickupItem(cookingPot.localPlayer, this, currentItem);
		}
	}

	private void HandleInsertion()
	{
		ItemInstance selectedHotbarItem = cookingPot.localPlayer.Inventory.GetSelectedHotbarItem();
		if (selectedHotbarItem == null || !selectedHotbarItem.Valid)
		{
			return;
		}
		if (objectEnabler.DoesAcceptItem(selectedHotbarItem.baseItem))
		{
			LocalizationParameters.itemX = selectedHotbarItem.baseItem.settings_Inventory.DisplayName;
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			cookingPot.displayText.ShowText(Helper.GetTerm("Game/PlaceItemX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				OnInsertItem(cookingPot.localPlayer, this, selectedHotbarItem);
			}
		}
		else if (selectedHotbarItem.baseItem.settings_consumeable.FoodType == FoodType.Food)
		{
			cookingPot.displayText.ShowText(Helper.GetTerm("Game/CookingToBig"));
		}
	}

	public void Animation_HitWater()
	{
		if (!er_plums.IsNull)
		{
			RuntimeManager.PlayOneShot(er_plums, transform.position);
		}
	}

	public void InsertItem(Network_Player player, ItemInstance itemInstance)
	{
		currentItem = itemInstance.Clone();
		currentItem.Amount = 1;
		objectEnabler.ActivateModel(currentItem.baseItem);
		if (!er_placeItem.IsNull)
		{
			RuntimeManager.PlayOneShot(er_placeItem, transform.position);
		}
		if (player != null && player.IsLocalPlayer)
		{
			player.Inventory.RemoveItemUses(itemInstance.UniqueName, itemInstance.Uses, addItemAfterUseToInventory: false);
			cookingPot.displayText.HideDisplayTexts();
			player.Animator.SetAnimation(PlayerAnimation.Trigger_Plant);
		}
	}

	public void RemoveItem(Network_Player player)
	{
		if (player != null && player.IsLocalPlayer)
		{
			if (HasItem)
			{
				player.Inventory.AddItem(currentItem);
			}
			player.Animator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
		}
		ClearItem();
	}

	public void ClearItem()
	{
		currentItem = null;
		objectEnabler.DeActivateAllModels();
	}

	public void ThrowItemIntoPotAnimation()
	{
		if (anim != null)
		{
			anim.SetTrigger("StartCooking_" + Index);
		}
		Invoke("ClearItem", hideItemDelay);
	}

	public void OnIsRayed()
	{
		if (initialized)
		{
			if (CanvasHelper.ActiveMenu != MenuType.None || PlayerItemManager.IsBusy || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
			{
				cookingPot.displayText.HideDisplayTexts();
			}
			else if (HasItem)
			{
				HandlePickup();
			}
			else
			{
				HandleInsertion();
			}
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (initialized && cookingPot.displayText != null)
		{
			cookingPot.displayText.HideDisplayTexts();
		}
	}
}
