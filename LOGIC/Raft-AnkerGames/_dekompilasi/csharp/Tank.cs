using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tank : MonoBehaviour, IRaycastable, IBlockPlaced
{
	public enum TankAcceptance
	{
		None,
		Input,
		Output,
		Both
	}

	public enum HarvestSetting
	{
		FULL,
		PARTIAL,
		NONE
	}

	public Action OnTankAmountChange;

	public Action OnTankPlayerManualInteraction;

	[SerializeField]
	private float currentTankAmount;

	public float previousTankAmount;

	public string localization_requiresFilled = "Game/RequiredItemX";

	public int tankID;

	public float maxCapacity;

	[SerializeField]
	private MonoBehaviour_ID_Network messageReciever;

	[SerializeField]
	private InteractableButton_Tank outputButton;

	[SerializeField]
	private List<Item_Base> acceptableTypes = new List<Item_Base>();

	[SerializeField]
	private List<Item_Base> acceptableOutputTypes = new List<Item_Base>();

	public Item_Base defaultFuelToAdd;

	public FuelValue_FiltrationType tankFiltrationType;

	public TextMeshPro testText;

	public EventReference er_placeFuelManually;

	public EventReference er_takeFuelManually;

	public HarvestSetting harvestSetting;

	public float minimumHarvestAmount = 1f;

	private Raft_Network network;

	private Network_Player localPlayer;

	private DisplayTextManager displayText;

	[SerializeField]
	private bool hasBeenPlaced;

	[SerializeField]
	private TankAcceptance tankAcceptance;

	public float NormalizedTankAmount => Mathf.Clamp01(CurrentTankAmount / maxCapacity);

	public bool IsFull => (float)Mathf.RoundToInt(CurrentTankAmount) >= maxCapacity;

	public float CurrentTankAmount
	{
		get
		{
			return currentTankAmount;
		}
		set
		{
			previousTankAmount = CurrentTankAmount;
			currentTankAmount = value;
		}
	}

	public void OnIsRayed()
	{
		if (!hasBeenPlaced)
		{
			return;
		}
		if (!hasBeenPlaced || CanvasHelper.ActiveMenu != MenuType.None || PlayerItemManager.IsBusy || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
		{
			displayText.HideDisplayTexts();
			return;
		}
		switch (tankAcceptance)
		{
		case TankAcceptance.Input:
			HandleAddFuel();
			break;
		case TankAcceptance.Both:
			HandleAddFuel();
			break;
		case TankAcceptance.None:
		case TankAcceptance.Output:
			break;
		}
	}

	private void InitAcceptance(ref TankAcceptance acceptance)
	{
		bool flag = TankHasInput();
		bool flag2 = TankHasOutput();
		if (flag & flag2)
		{
			acceptance = TankAcceptance.Both;
		}
		else if (flag)
		{
			acceptance = TankAcceptance.Input;
		}
		else if (flag2)
		{
			acceptance = TankAcceptance.Output;
		}
		else
		{
			acceptance = TankAcceptance.None;
		}
	}

	private void HandleAddFuel()
	{
		if (IsFull)
		{
			displayText.HideDisplayTexts();
			return;
		}
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		ItemInstance selectedHotbarItem = localPlayer.Inventory.GetSelectedHotbarItem();
		Item_Base item_Base = null;
		if (selectedHotbarItem == null || !acceptableTypes.Contains(selectedHotbarItem.baseItem))
		{
			if (!(defaultFuelToAdd != null) || localPlayer.Inventory.GetItemCount(defaultFuelToAdd) <= 0)
			{
				bool applyParameters = false;
				if (defaultFuelToAdd != null)
				{
					LocalizationParameters.itemX = defaultFuelToAdd.settings_Inventory.DisplayName;
					applyParameters = true;
				}
				displayText.ShowText(Helper.GetTerm(localization_requiresFilled, applyParameters));
				return;
			}
			item_Base = defaultFuelToAdd;
		}
		else
		{
			if (selectedHotbarItem.Uses <= 0)
			{
				if (selectedHotbarItem != null)
				{
					item_Base = selectedHotbarItem.baseItem;
				}
				if (item_Base != null)
				{
					LocalizationParameters.itemX = item_Base.settings_Inventory.DisplayName;
					displayText.ShowText(Helper.GetTerm(localization_requiresFilled, applyParameters: true));
				}
				return;
			}
			item_Base = selectedHotbarItem.baseItem;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		LocalizationParameters.itemX = item_Base.settings_Inventory.DisplayName;
		displayText.ShowText(Helper.GetTerm("Game/PlaceItemX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			return;
		}
		SO_FuelValue filteredFuelValueSoFromItem = LiquidFuelManager.GetFilteredFuelValueSoFromItem(item_Base, tankFiltrationType);
		if (filteredFuelValueSoFromItem == null)
		{
			Debug.Log("filtered fuel value is null with " + item_Base, gameObject);
			return;
		}
		int fuelValueOfType = filteredFuelValueSoFromItem.fuelValueOfType;
		displayText.HideDisplayTexts();
		if (fuelValueOfType == -1)
		{
			Debug.Log("Fuel value is -1 probably something wrong with" + item_Base?.ToString() + " or its connection in liquid fuel manager");
			return;
		}
		Message_Tank message = new Message_Tank(Messages.Tank, network.NetworkIDManager, messageReciever.ObjectIndex, (localPlayer == null) ? ((Network_UserId)0uL) : localPlayer.steamID, fuelValueOfType, tankID, item_Base.UniqueIndex);
		if (Raft_Network.IsHost)
		{
			ModifyTank(localPlayer, fuelValueOfType, item_Base);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void HandleTakeFuel()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		ItemInstance selectedHotbarItem = localPlayer.Inventory.GetSelectedHotbarItem();
		Item_Base item_Base = null;
		if (selectedHotbarItem != null)
		{
			item_Base = selectedHotbarItem.baseItem;
		}
		if (selectedHotbarItem == null || item_Base == null || !acceptableOutputTypes.Contains(item_Base))
		{
			return;
		}
		LocalizationParameters.itemX = selectedHotbarItem.settings_Inventory.DisplayName;
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		displayText.ShowText(Helper.GetTerm("Game/PlaceItemX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			return;
		}
		SO_FuelValue filteredFuelValueSoFromItem = LiquidFuelManager.GetFilteredFuelValueSoFromItem(item_Base, tankFiltrationType);
		if (filteredFuelValueSoFromItem == null)
		{
			return;
		}
		int num = -filteredFuelValueSoFromItem.fuelValueOfType;
		if (CurrentTankAmount + (float)num < 0f || !CollectItem(localPlayer))
		{
			return;
		}
		displayText.HideDisplayTexts();
		if (num != -1)
		{
			Message_Tank message = new Message_Tank(Messages.Tank, network.NetworkIDManager, messageReciever.ObjectIndex, (localPlayer == null) ? ((Network_UserId)0uL) : localPlayer.steamID, num, tankID);
			if (Raft_Network.IsHost)
			{
				ModifyTank(localPlayer, num);
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void SendTankSetValueMessage(Network_Player player)
	{
		if (Raft_Network.IsHost)
		{
			Message_Tank message = new Message_Tank(Messages.Tank_SetValue, network.NetworkIDManager, messageReciever.ObjectIndex, (player == null) ? ((Network_UserId)0uL) : player.steamID, CurrentTankAmount, tankID);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (displayText != null)
		{
			displayText.HideDisplayTexts();
		}
	}

	public void OnBlockPlacedInterface()
	{
		network = ComponentManager<Raft_Network>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
		displayText = ComponentManager<DisplayTextManager>.Value;
		InitAcceptance(ref tankAcceptance);
		switch (tankAcceptance)
		{
		case TankAcceptance.Output:
		{
			InteractableButton_Tank interactableButton_Tank2 = outputButton;
			interactableButton_Tank2.OnButtonPressed = (Action)Delegate.Combine(interactableButton_Tank2.OnButtonPressed, new Action(HandleTakeFuel));
			break;
		}
		case TankAcceptance.Both:
		{
			InteractableButton_Tank interactableButton_Tank = outputButton;
			interactableButton_Tank.OnButtonPressed = (Action)Delegate.Combine(interactableButton_Tank.OnButtonPressed, new Action(HandleTakeFuel));
			break;
		}
		}
		hasBeenPlaced = true;
	}

	public bool OnTankModifyMessage(Message_Tank msgTank)
	{
		Network_Player playerFromID = network.GetPlayerFromID(msgTank.SteamID);
		Item_Base itemByIndex = ItemManager.GetItemByIndex(msgTank.removedItemIndex);
		return ModifyTank(playerFromID, msgTank.tankIncrement, itemByIndex);
	}

	public void OnTankSetValueMessage(Message_Tank msgTank)
	{
		SetTankAmount(msgTank.tankIncrement);
	}

	public void ModifyTankRPC(float amount, Network_Player player)
	{
		if (Raft_Network.IsHost && ModifyTank(player, amount))
		{
			if (messageReciever == null)
			{
				Debug.Log("reciever in tank is null, cant send message :(");
				return;
			}
			Message_Tank message = new Message_Tank(Messages.Tank, network.NetworkIDManager, messageReciever.ObjectIndex, (player == null) ? ((Network_UserId)0uL) : player.steamID, amount, tankID);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void SetTankAmount(float newAmount)
	{
		CurrentTankAmount = Mathf.Clamp(newAmount, 0f, maxCapacity);
		if (OnTankAmountChange != null)
		{
			OnTankAmountChange();
		}
		LocalTankAmountChange();
	}

	public bool CollectItem(Network_Player player)
	{
		if (player == null || player.Inventory == null)
		{
			return false;
		}
		ItemInstance selectedHotbarItem = player.Inventory.GetSelectedHotbarItem();
		if (!IsItemAcceptableOutput(selectedHotbarItem.baseItem))
		{
			return false;
		}
		if (selectedHotbarItem == null || selectedHotbarItem.settings_consumeable == null || selectedHotbarItem.settings_consumeable.FoodForm != FoodForm.Fluid)
		{
			return false;
		}
		if (selectedHotbarItem.settings_consumeable.FoodType == FoodType.None)
		{
			Item_Base item = selectedHotbarItem.settings_cookable.CookingResult.item;
			Slot selectedHotbarSlot = player.Inventory.GetSelectedHotbarSlot();
			if (item == null)
			{
				return false;
			}
			player.Inventory.RemoveSelectedHotSlotItem(1);
			player.Inventory.AddItem(item.UniqueName, selectedHotbarSlot, 1);
			selectedHotbarSlot.SetUses(1);
			return true;
		}
		if (!selectedHotbarItem.HasMaxUses)
		{
			player.Inventory.GetSelectedHotbarSlot().IncrementUses(1);
			return true;
		}
		return false;
	}

	public bool HasEnoughFuelComparedToHandItem()
	{
		ItemInstance selectedHotbarItem = localPlayer.Inventory.GetSelectedHotbarItem();
		Item_Base item_Base = null;
		if (selectedHotbarItem != null)
		{
			item_Base = selectedHotbarItem.baseItem;
		}
		if (selectedHotbarItem == null || item_Base == null || !acceptableOutputTypes.Contains(item_Base))
		{
			return false;
		}
		SO_FuelValue filteredFuelValueSoFromItem = LiquidFuelManager.GetFilteredFuelValueSoFromItem(item_Base, tankFiltrationType);
		if (filteredFuelValueSoFromItem == null)
		{
			Debug.Log("filtered fuel value is null with " + item_Base, gameObject);
			return false;
		}
		int num = -filteredFuelValueSoFromItem.fuelValueOfType;
		if (CurrentTankAmount + (float)num >= 0f)
		{
			return true;
		}
		return false;
	}

	public bool PlayerHandItemIsAcceptedOutput()
	{
		ItemInstance selectedHotbarItem = localPlayer.Inventory.GetSelectedHotbarItem();
		Item_Base item_Base = null;
		if (selectedHotbarItem != null)
		{
			item_Base = selectedHotbarItem.baseItem;
		}
		if (selectedHotbarItem == null || item_Base == null || !acceptableOutputTypes.Contains(item_Base) || (selectedHotbarItem.BaseItemMaxUses > 1 && selectedHotbarItem.HasMaxUses))
		{
			return false;
		}
		return true;
	}

	public bool TankHasOutput()
	{
		if (acceptableOutputTypes.Count > 0 && outputButton != null)
		{
			return true;
		}
		return false;
	}

	public bool TankHasInput()
	{
		if (acceptableTypes.Count > 0)
		{
			return true;
		}
		return false;
	}

	public bool IsItemAcceptableOutput(Item_Base item)
	{
		if (acceptableOutputTypes.Contains(item))
		{
			return true;
		}
		return false;
	}

	public bool IsConsideredHarvestable()
	{
		return harvestSetting switch
		{
			HarvestSetting.FULL => IsFull, 
			HarvestSetting.PARTIAL => CurrentTankAmount >= minimumHarvestAmount, 
			HarvestSetting.NONE => false, 
			_ => false, 
		};
	}

	private void TempUpdateText()
	{
		if (testText != null)
		{
			testText.text = CurrentTankAmount + " / " + maxCapacity;
		}
	}

	protected virtual void LocalTankAmountChange()
	{
		TempUpdateText();
	}

	public virtual bool ModifyTank(Network_Player player, float amount, Item_Base itemType = null)
	{
		if (Raft_Network.IsHost && ((IsFull && amount > 0f) || (CurrentTankAmount <= 0f && amount <= 0f)))
		{
			return false;
		}
		if (amount != 0f)
		{
			float num = maxCapacity - CurrentTankAmount;
			if (amount > num)
			{
				amount = num;
			}
			if (player != null)
			{
				if (amount > 0f)
				{
					if (!er_placeFuelManually.IsNull)
					{
						RuntimeManager.PlayOneShot(er_placeFuelManually, player.transform.position);
					}
				}
				else if (amount < 0f && !er_takeFuelManually.IsNull)
				{
					RuntimeManager.PlayOneShot(er_takeFuelManually, player.transform.position);
				}
				if (player.IsLocalPlayer && itemType != null)
				{
					Slot selectedHotbarSlot = player.Inventory.GetSelectedHotbarSlot();
					if (selectedHotbarSlot.itemInstance != null && selectedHotbarSlot.itemInstance.baseItem != null && selectedHotbarSlot.itemInstance.baseItem.UniqueIndex == itemType.UniqueIndex)
					{
						selectedHotbarSlot.IncrementUses(-1);
					}
					else
					{
						player.Inventory.RemoveItemUses(itemType.UniqueName, 1);
					}
				}
				HandlePlayerTankModificationAnimation(player);
			}
		}
		SetTankAmount(CurrentTankAmount + amount);
		return true;
	}

	private void HandlePlayerTankModificationAnimation(Network_Player player)
	{
		switch (tankAcceptance)
		{
		case TankAcceptance.Input:
			player.Animator.SetAnimation(PlayerAnimation.Trigger_Plant, triggering: true);
			OnTankPlayerManualInteraction?.Invoke();
			break;
		case TankAcceptance.Both:
			player.Animator.SetAnimation(PlayerAnimation.Trigger_Plant, triggering: true);
			OnTankPlayerManualInteraction?.Invoke();
			break;
		case TankAcceptance.None:
		case TankAcceptance.Output:
			break;
		}
	}
}
