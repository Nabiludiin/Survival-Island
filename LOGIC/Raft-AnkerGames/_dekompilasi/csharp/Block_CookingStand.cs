using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
public class Block_CookingStand : Block, IRaycastable
{
	[Serializable]
	public class CookingSlotCollection
	{
		public CookingSlot[] cookingSlots;

		public bool CanCookItem(Item_Base itemToCook)
		{
			for (int i = 0; i < cookingSlots.Length; i++)
			{
				CookingSlot cookingSlot = cookingSlots[i];
				if (cookingSlot.IsBusy || !cookingSlot.CanCookItem(itemToCook))
				{
					return false;
				}
			}
			return true;
		}
	}

	public Fuel fuel;

	public GameObject particleController;

	[SerializeField]
	private CookingSlotCollection[] cookingSlotCollections;

	[HideInInspector]
	public CookingSlot[] cookingSlots;

	[SerializeField]
	private StudioEventEmitter soundEmitter;

	[SerializeField]
	private bool endSoundEmitterWithCue;

	[SerializeField]
	private StudioEventEmitter scndSoundEmitter;

	[SerializeField]
	private bool endScndSoundEmitterWithCue;

	public EventReference er_pourWater;

	public EventReference er_fillWater;

	public EventReference er_placeItem;

	protected Network_Player playerNetwork;

	protected DisplayTextManager displayTextManager;

	protected override void Awake()
	{
		base.Awake();
		if (particleController != null)
		{
			particleController.SetActive(value: false);
		}
		cookingSlots = GetComponentsInChildren<CookingSlot>();
	}

	protected override void Start()
	{
		base.Start();
		playerNetwork = ComponentManager<Network_Player>.Value;
	}

	public override void OnFinishedPlacement()
	{
		base.OnFinishedPlacement();
		displayTextManager = ComponentManager<DisplayTextManager>.Value;
		if (Raft_Network.IsHost)
		{
			if (fuel != null)
			{
				fuel.OnFuelRemovedEvent += OnFuelRemoved;
			}
			CookingSlot[] array = cookingSlots;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnChangeEvent += OnCookingSlotChanged;
			}
		}
	}

	protected virtual void Update()
	{
		if (playerNetwork == null)
		{
			playerNetwork = ComponentManager<Network_Player>.Value;
			return;
		}
		if (playerNetwork.Network == null)
		{
			playerNetwork.Network = ComponentManager<Raft_Network>.Value;
		}
		if (Raft_Network.IsHost && fuel != null)
		{
			fuel.UpdateFuel();
		}
		if (RawItemIsOnSlot() && CanStartEnergySource())
		{
			StartEnergySource();
			if (soundEmitter != null)
			{
				soundEmitter.Play();
			}
			if (scndSoundEmitter != null)
			{
				scndSoundEmitter.Play();
			}
			if (particleController != null)
			{
				particleController.SetActive(value: true);
			}
		}
		if (IsUsingEnergySource())
		{
			if (!RawItemIsOnSlot())
			{
				StopEnergySource();
				FinishSounds();
				if (particleController != null)
				{
					particleController.SetActive(value: false);
				}
			}
			else
			{
				if (!Raft_Network.IsHost)
				{
					return;
				}
				CookingSlot[] array = cookingSlots;
				foreach (CookingSlot cookingSlot in array)
				{
					if (cookingSlot.IsBusy && !cookingSlot.IsComplete)
					{
						cookingSlot.UpdateSlot();
					}
				}
			}
		}
		else if (particleController != null && particleController.activeInHierarchy)
		{
			StopEnergySource();
			particleController.SetActive(value: false);
			FinishSounds();
		}
	}

	public bool InsertItem(Item_Base itemToInsert, CookingSlot[] emptySlots, Network_Player player)
	{
		if (itemToInsert == null || emptySlots == null || player == null)
		{
			return false;
		}
		emptySlots[0].StartCooking(itemToInsert, emptySlots);
		if (player.IsLocalPlayer)
		{
			player.Inventory.GetSelectedHotbarSlot().IncrementUses(-1);
		}
		player.Animator.SetAnimation(PlayerAnimation.Trigger_Plant, triggering: true);
		if (itemToInsert.settings_consumeable.FoodType == FoodType.SaltWater)
		{
			RuntimeManager.PlayOneShot(er_pourWater, playerNetwork.transform.position);
		}
		if (!er_placeItem.IsNull)
		{
			RuntimeManager.PlayOneShot(er_placeItem, transform.position);
		}
		return true;
	}

	public bool IncrementFuel(int incrementAmount, Network_Player player)
	{
		if (player == null)
		{
			Debug.Log("NULL, could not increment fuel... playernetwork is null");
			return false;
		}
		if (fuel.HasMaxFuel() && incrementAmount > 0)
		{
			return false;
		}
		fuel.AddFuel(incrementAmount);
		if (player.IsLocalPlayer && incrementAmount > 0)
		{
			player.Inventory.RemoveItem(fuel.fuelItem.name, incrementAmount);
		}
		player.Animator.SetAnimation(PlayerAnimation.Trigger_Plant, triggering: true);
		return true;
	}

	public virtual bool CollectItem(CookingSlot slot, Network_Player player)
	{
		Item_Base currentItem = slot.CurrentItem;
		if (currentItem == null)
		{
			Debug.LogWarning("Tried to collect from slot with null item", gameObject);
			return false;
		}
		Cost cookingResult = currentItem.settings_cookable.CookingResult;
		if (cookingResult.item == null)
		{
			Debug.LogWarning("When trying to collect complete item, the cookingresult was null", gameObject);
			return false;
		}
		if (player.IsLocalPlayer)
		{
			player.Inventory.AddItem(cookingResult.item.UniqueName, cookingResult.amount);
		}
		if (slot.connectedSlot != null)
		{
			slot.connectedSlot.ResetSlot();
		}
		else
		{
			slot.ResetSlot();
		}
		player.Animator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
		if (this is Block_CookingStand_Purifier)
		{
			RuntimeManager.PlayOneShot(er_fillWater, player.transform.position);
		}
		return true;
	}

	public CookingSlot[] GetCookingSlotsForItem(Item_Base itemToInsert)
	{
		if (itemToInsert == null)
		{
			return null;
		}
		List<CookingSlot> list = new List<CookingSlot>();
		if (itemToInsert.settings_cookable.CookingSlotsRequired > 1)
		{
			for (int i = 0; i < cookingSlotCollections.Length; i++)
			{
				CookingSlotCollection cookingSlotCollection = cookingSlotCollections[i];
				if (cookingSlotCollection.cookingSlots.Length == itemToInsert.settings_cookable.CookingSlotsRequired && cookingSlotCollection.CanCookItem(itemToInsert))
				{
					return cookingSlotCollection.cookingSlots;
				}
			}
		}
		else
		{
			for (int j = 0; j < cookingSlots.Length; j++)
			{
				CookingSlot cookingSlot = cookingSlots[j];
				if (!cookingSlot.IsBusy && cookingSlot.CanCookItem(itemToInsert))
				{
					list.Add(cookingSlot);
					return list.ToArray();
				}
			}
		}
		return null;
	}

	public int GetIndexOfCookingSlot(CookingSlot slot)
	{
		for (int i = 0; i < cookingSlots.Length; i++)
		{
			if (slot == cookingSlots[i])
			{
				return i;
			}
		}
		return -1;
	}

	protected virtual void CheckForCollecting(CookingSlot slot)
	{
		Cost cookingResult = slot.CurrentItem.settings_cookable.CookingResult;
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		LocalizationParameters.itemX = cookingResult.item.settings_Inventory.DisplayName;
		displayTextManager.ShowText(Helper.GetTerm("Game/PickUpX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(new Message_CookingStand_Collect(Messages.CookingStand_CollectItem, playerNetwork.Network.NetworkIDManager, playerNetwork.steamID, this, slot), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				CollectItem(slot, playerNetwork);
			}
			else
			{
				playerNetwork.Network.SendP2P(playerNetwork.Network.HostID, new Message_CookingStand_Collect(Messages.CookingStand_CollectItem, playerNetwork.Network.NetworkIDManager, playerNetwork.steamID, this, slot), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	protected virtual bool CanStartEnergySource()
	{
		if (fuel != null && !fuel.IsBurning() && fuel.GetFuelCount() != 0)
		{
			return true;
		}
		return false;
	}

	protected virtual bool IsUsingEnergySource()
	{
		if (fuel != null && fuel.IsBurning())
		{
			return true;
		}
		return false;
	}

	protected virtual void StartEnergySource()
	{
		if (fuel != null)
		{
			fuel.StartBurning();
		}
	}

	protected virtual void StopEnergySource()
	{
		if (fuel != null)
		{
			fuel.StopBurning();
		}
	}

	private void FinishSounds()
	{
		if (soundEmitter != null)
		{
			if (endSoundEmitterWithCue)
			{
				soundEmitter.EventInstance.keyOff();
			}
			else
			{
				soundEmitter.Stop();
			}
		}
		if (scndSoundEmitter != null)
		{
			if (endScndSoundEmitterWithCue)
			{
				scndSoundEmitter.EventInstance.keyOff();
			}
			else
			{
				scndSoundEmitter.Stop();
			}
		}
	}

	private bool CookingSlotIsOurs(CookingSlot slot)
	{
		CookingSlot[] array = cookingSlots;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == slot)
			{
				return true;
			}
		}
		return false;
	}

	private void OnFuelRemoved()
	{
		if (Raft_Network.IsHost)
		{
			playerNetwork.Network.RPC(new Message_CookingStand_IncrementFuel(Messages.CookingStand_IncrementFuel, playerNetwork.Network.NetworkIDManager, playerNetwork.steamID, this, -1), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void OnCookingSlotChanged(CookingSlot slot)
	{
		if (!(slot == null) && Raft_Network.IsHost)
		{
			if (playerNetwork == null)
			{
				playerNetwork = ComponentManager<Network_Player>.Value;
			}
			if (slot.IsComplete && playerNetwork != null && playerNetwork.Network != null)
			{
				playerNetwork.Network.RPC(new Message_CookingStand_SlotComplete(Messages.CookingStand_SlotComplete, playerNetwork.Network.NetworkIDManager, playerNetwork.steamID, this, GetIndexOfCookingSlot(slot)), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	private CookingSlot GetEmptyCookingSlot()
	{
		CookingSlot[] array = cookingSlots;
		foreach (CookingSlot cookingSlot in array)
		{
			if (!cookingSlot.IsBusy)
			{
				return cookingSlot;
			}
		}
		return null;
	}

	private bool RawItemIsOnSlot()
	{
		CookingSlot[] array = cookingSlots;
		foreach (CookingSlot cookingSlot in array)
		{
			if (cookingSlot.IsBusy && !cookingSlot.IsComplete)
			{
				return true;
			}
		}
		return false;
	}

	public override RGD Serialize_Save()
	{
		return new RGD_CookingStand(RGDType.Block_CookingStand, this);
	}

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_CookingStand(RGDType.Block_CookingStand, this);
	}

	public void OnIsRayed()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
		{
			displayTextManager.HideDisplayTexts();
			return;
		}
		if (playerNetwork == null)
		{
			playerNetwork = ComponentManager<Network_Player>.Value;
			return;
		}
		if (Helper.HitAtCursor(out var hit, Player.UseDistance, LayerMasks.MASK_Item))
		{
			CookingSlot component = hit.transform.GetComponent<CookingSlot>();
			if (CookingSlotIsOurs(component) && component != null && component.IsBusy && component.IsComplete)
			{
				CheckForCollecting(component);
				return;
			}
		}
		ItemInstance selectedHotbarItem = playerNetwork.Inventory.GetSelectedHotbarItem();
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		CookingSlot[] array = ((selectedHotbarItem != null) ? GetCookingSlotsForItem(selectedHotbarItem.baseItem) : null);
		if (array != null && !PlayerItemManager.IsBusy)
		{
			displayTextManager.ShowText(Helper.GetTerm("Game/PlaceItem", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				Message_CookingStand_Insert message_CookingStand_Insert = new Message_CookingStand_Insert(Messages.CookingStand_InsertItem, playerNetwork.Network.NetworkIDManager, playerNetwork.steamID, this, selectedHotbarItem.baseItem, array);
				displayTextManager.HideDisplayTexts();
				if (Raft_Network.IsHost)
				{
					playerNetwork.Network.RPC(message_CookingStand_Insert, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					InsertItem(selectedHotbarItem.baseItem, array, playerNetwork);
				}
				else
				{
					message_CookingStand_Insert.emptySlotIndexes = null;
					playerNetwork.Network.SendP2P(playerNetwork.Network.HostID, message_CookingStand_Insert, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
		else if (fuel != null && playerNetwork.Inventory.GetItemCount(fuel.fuelItem) > 0 && fuel != null && !fuel.HasMaxFuel())
		{
			LocalizationParameters.itemX = fuel.fuelItem.settings_Inventory.DisplayName;
			displayTextManager.ShowText(Helper.GetTerm("Game/PlaceItemX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				displayTextManager.HideDisplayTexts();
				Message_CookingStand_IncrementFuel message_CookingStand_IncrementFuel = new Message_CookingStand_IncrementFuel(Messages.CookingStand_IncrementFuel, playerNetwork.Network.NetworkIDManager, playerNetwork.steamID, this, 1);
				if (Raft_Network.IsHost)
				{
					playerNetwork.Network.RPC(message_CookingStand_IncrementFuel, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					IncrementFuel(message_CookingStand_IncrementFuel.incrementAmount, playerNetwork);
				}
				else
				{
					playerNetwork.Network.SendP2P(playerNetwork.Network.HostID, message_CookingStand_IncrementFuel, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
		else if (selectedHotbarItem != null && selectedHotbarItem.baseItem != null && selectedHotbarItem.baseItem.settings_cookable.CookingSlotsRequired > 1)
		{
			displayTextManager.ShowText(Helper.GetTerm("Game/CookingToBig"));
		}
		else
		{
			displayTextManager.HideDisplayTexts();
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		displayTextManager.HideDisplayTexts();
	}
}
