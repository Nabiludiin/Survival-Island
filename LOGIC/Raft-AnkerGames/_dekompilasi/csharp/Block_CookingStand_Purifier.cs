using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Block_CookingStand_Purifier : Block_CookingStand
{
	[SerializeField]
	private FluidComponent freshWater;

	[SerializeField]
	private FluidComponent saltWater;

	protected override void Update()
	{
		base.Update();
		if (saltWater != null)
		{
			int activeSlotCount = GetActiveSlotCount();
			if (activeSlotCount == 0)
			{
				saltWater.HideFluid();
			}
			else
			{
				saltWater.SetStep(activeSlotCount);
			}
		}
		if (freshWater != null)
		{
			int completeSlotCount = GetCompleteSlotCount();
			if (completeSlotCount == 0)
			{
				freshWater.HideFluid();
			}
			else
			{
				freshWater.SetStep(completeSlotCount);
			}
		}
	}

	public override bool CollectItem(CookingSlot cookingSlot, Network_Player player)
	{
		if (cookingSlot == null)
		{
			return false;
		}
		if (player != null && !player.IsLocalPlayer)
		{
			if (cookingSlot.connectedSlot != null)
			{
				cookingSlot.connectedSlot.ResetSlot();
			}
			else
			{
				cookingSlot.ResetSlot();
			}
			return true;
		}
		if (player == null || player.Inventory == null)
		{
			return false;
		}
		ItemInstance selectedHotbarItem = player.Inventory.GetSelectedHotbarItem();
		if (selectedHotbarItem == null || selectedHotbarItem.settings_consumeable == null || selectedHotbarItem.settings_consumeable.FoodForm != FoodForm.Fluid)
		{
			return false;
		}
		if (selectedHotbarItem.settings_consumeable.FoodType == FoodType.None)
		{
			Item_Base item = selectedHotbarItem.settings_cookable.CookingResult.item;
			Slot selectedHotbarSlot = player.Inventory.GetSelectedHotbarSlot();
			if (item == null || selectedHotbarSlot == null)
			{
				return false;
			}
			player.Inventory.RemoveSelectedHotSlotItem(1);
			player.Inventory.AddItem(item.UniqueName, selectedHotbarSlot, 1);
			selectedHotbarSlot.SetUses(1);
			if (cookingSlot.connectedSlot != null)
			{
				cookingSlot.connectedSlot.ResetSlot();
			}
			else
			{
				cookingSlot.ResetSlot();
			}
			return true;
		}
		if (selectedHotbarItem == null)
		{
			return false;
		}
		Cost cookingResult = cookingSlot.CurrentItem.settings_cookable.CookingResult;
		if (selectedHotbarItem.settings_consumeable.FoodType == cookingResult.item.settings_consumeable.FoodType && !selectedHotbarItem.HasMaxUses)
		{
			player.Inventory.GetSelectedHotbarSlot().IncrementUses(1);
			if (cookingSlot.connectedSlot != null)
			{
				cookingSlot.connectedSlot.ResetSlot();
			}
			else
			{
				cookingSlot.ResetSlot();
			}
			return true;
		}
		return false;
	}

	protected override void CheckForCollecting(CookingSlot slot)
	{
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		ItemInstance selectedHotbarItem = playerNetwork.Inventory.GetSelectedHotbarItem();
		if (selectedHotbarItem != null && selectedHotbarItem.settings_consumeable.FoodForm == FoodForm.Fluid)
		{
			Cost cookingResult = slot.CurrentItem.settings_cookable.CookingResult;
			if (selectedHotbarItem.settings_consumeable.FoodType == FoodType.None)
			{
				displayTextManager.ShowText(Helper.GetTerm("Game/FillWater", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
				{
					displayTextManager.HideDisplayTexts();
					Message_CookingStand_Collect message = new Message_CookingStand_Collect(Messages.CookingStand_CollectItem, playerNetwork.Network.NetworkIDManager, playerNetwork.steamID, this, slot);
					if (Raft_Network.IsHost)
					{
						playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
						CollectItem(slot, playerNetwork);
					}
					else
					{
						playerNetwork.Network.SendP2P(playerNetwork.Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
				}
			}
			else
			{
				if (selectedHotbarItem.settings_consumeable.FoodType != cookingResult.item.settings_consumeable.FoodType || selectedHotbarItem.HasMaxUses)
				{
					return;
				}
				displayTextManager.ShowText(Helper.GetTerm("Game/FillWater", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
				{
					displayTextManager.HideDisplayTexts();
					Message_CookingStand_Collect message2 = new Message_CookingStand_Collect(Messages.CookingStand_CollectItem, playerNetwork.Network.NetworkIDManager, playerNetwork.steamID, this, slot);
					if (Raft_Network.IsHost)
					{
						playerNetwork.Network.RPC(message2, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
						CollectItem(slot, playerNetwork);
					}
					else
					{
						playerNetwork.Network.SendP2P(playerNetwork.Network.HostID, message2, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
				}
			}
		}
		else
		{
			displayTextManager.HideDisplayTexts();
		}
	}

	private int GetActiveSlotCount()
	{
		int num = 0;
		for (int i = 0; i < cookingSlots.Length; i++)
		{
			if (cookingSlots[i].IsBusy && !cookingSlots[i].IsComplete)
			{
				num++;
			}
		}
		return num;
	}

	private int GetCompleteSlotCount()
	{
		int num = 0;
		for (int i = 0; i < cookingSlots.Length; i++)
		{
			if (cookingSlots[i].IsComplete)
			{
				num++;
			}
		}
		return num;
	}
}
