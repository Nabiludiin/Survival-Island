using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsumeComponent : MonoBehaviour
{
	private Network_Player playerNetwork;

	private bool showingText;

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	private void Update()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None || PlayerItemManager.IsBusy || !playerNetwork.IsLocalPlayer)
		{
			if (showingText)
			{
				showingText = false;
				playerNetwork.canvas.displayTextManager.HideDisplayTexts();
			}
			return;
		}
		ItemInstance selectedHotbarItem = playerNetwork.Inventory.GetSelectedHotbarItem();
		if (selectedHotbarItem == null || selectedHotbarItem.Uses <= 0)
		{
			if (showingText)
			{
				showingText = false;
				playerNetwork.canvas.displayTextManager.HideDisplayTexts();
			}
			return;
		}
		FoodForm foodForm = selectedHotbarItem.baseItem.settings_consumeable.FoodForm;
		if (foodForm != FoodForm.None)
		{
			string term = string.Empty;
			switch (foodForm)
			{
			case FoodForm.Solid:
				term = "Game/Eat";
				break;
			case FoodForm.Fluid:
				term = "Game/Drink";
				break;
			}
			if (selectedHotbarItem.baseItem.settings_consumeable.ThirstYield != 0f || selectedHotbarItem.baseItem.settings_consumeable.HungerYield != 0f || selectedHotbarItem.baseItem.settings_consumeable.OxygenYield != 0f || selectedHotbarItem.baseItem.settings_consumeable.BonusThirstYield != 0f || selectedHotbarItem.baseItem.settings_consumeable.BonusHungerYield != 0f)
			{
				string bindingDisplayString = PlayerInput.GetPlayerByIndex(0).actions["Fire"].GetBindingDisplayString();
				playerNetwork.canvas.displayTextManager.ShowText(Helper.GetTerm(term), bindingDisplayString, MyInput.Keybinds[selectedHotbarItem.settings_usable.GetUseButtonName()].MainKey, 2, 0, clearAllTexts: false);
				showingText = true;
			}
		}
	}

	public void OnUse()
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		ItemInstance selectedHotbarItem = playerNetwork.Inventory.GetSelectedHotbarItem();
		if (selectedHotbarItem != null && (selectedHotbarItem.Valid || selectedHotbarItem.baseItem.settings_consumeable.FoodType != FoodType.None) && selectedHotbarItem.Uses > 0 && (selectedHotbarItem.baseItem.settings_consumeable.ThirstYield != 0f || selectedHotbarItem.baseItem.settings_consumeable.HungerYield != 0f || selectedHotbarItem.baseItem.settings_consumeable.OxygenYield != 0f || selectedHotbarItem.baseItem.settings_consumeable.BonusThirstYield != 0f || selectedHotbarItem.baseItem.settings_consumeable.BonusHungerYield != 0f))
		{
			Message_Consume message = new Message_Consume(Messages.Consume, playerNetwork, selectedHotbarItem.baseItem);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				playerNetwork.Stats.Consume(selectedHotbarItem.baseItem);
			}
			else
			{
				playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void OnDeSelect()
	{
		if (playerNetwork != null)
		{
			playerNetwork.canvas.displayTextManager.HideDisplayTexts();
		}
		showingText = false;
	}
}
