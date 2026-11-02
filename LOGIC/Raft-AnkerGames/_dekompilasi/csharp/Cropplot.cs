using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cropplot : Block, IRaycastable
{
	public bool peckable;

	public BoxCollider mainCollider;

	[Space(10f)]
	[Tooltip("Which items are available to plant in this cropplot, unique item names")]
	public List<Item_Base> acceptableItemTypes = new List<Item_Base>();

	[Space(10f)]
	public List<PlantationSlot> plantationSlots = new List<PlantationSlot>();

	[SerializeField]
	private GameObject defaultReplenishItem;

	private Raft_Network network;

	private Network_Player localPlayer;

	private CanvasHelper canvas;

	private bool showingText;

	private PlantManager plantManager;

	public bool IsFull => GetBusySlotCount() == plantationSlots.Count;

	public bool ContainsCrops => GetBusySlotCount() > 0;

	private void Update()
	{
		for (int i = 0; i < plantationSlots.Count; i++)
		{
			PlantationSlot plantationSlot = plantationSlots[i];
			if (plantationSlot.CanGrow)
			{
				plantationSlot.plant.Grow(network, localPlayer);
			}
		}
	}

	protected override void OnDestroy()
	{
		if (hasBeenPlaced)
		{
			plantManager.SendOnCropplotModified(this, PlantManager.ObjectModification.REMOVED);
			base.OnDestroy();
			if (PlantManager.allCropplots.Contains(this))
			{
				PlantManager.allCropplots.Remove(this);
			}
		}
	}

	public void OnIsRayed()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance) || PlayerItemManager.IsBusy)
		{
			if (showingText)
			{
				showingText = false;
				canvas.displayTextManager.HideDisplayTexts();
			}
		}
		else
		{
			if (!ContainsCrops || !SlotsNeedWater())
			{
				return;
			}
			Slot selectedHotbarSlot = localPlayer.Inventory.GetSelectedHotbarSlot();
			if (selectedHotbarSlot == null || selectedHotbarSlot.itemInstance == null)
			{
				return;
			}
			if (selectedHotbarSlot.itemInstance.settings_consumeable.FoodForm == FoodForm.Fluid)
			{
				if (selectedHotbarSlot.itemInstance.settings_consumeable.FoodType == FoodType.Water && selectedHotbarSlot.itemInstance.Uses > 0)
				{
					InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
					showingText = true;
					canvas.displayTextManager.ShowText(Helper.GetTerm("Game/WaterCrops"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
					if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
					{
						showingText = false;
						canvas.displayTextManager.HideDisplayTexts();
						Message_WaterCrop message = new Message_WaterCrop(Messages.PlantManager_WaterPlant, localPlayer.PlantManager, this, playSound: true);
						if (Raft_Network.IsHost)
						{
							localPlayer.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
							localPlayer.PlantManager.WaterCropplot(this, playSound: true);
						}
						else
						{
							localPlayer.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
						}
						localPlayer.Animator.SetAnimation(PlayerAnimation.Trigger_Plant);
						selectedHotbarSlot.IncrementUses(-1);
					}
				}
				else
				{
					showingText = true;
					canvas.displayTextManager.ShowText(Helper.GetTerm("Game/NeedWater"));
				}
			}
			else
			{
				showingText = true;
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/NeedWater"));
			}
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (canvas != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	public override void OnFinishedPlacement()
	{
		localPlayer = ComponentManager<Network_Player>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		network = ComponentManager<Raft_Network>.Value;
		base.OnFinishedPlacement();
		PlantManager.allCropplots.Add(this);
		if (localPlayer != null)
		{
			plantManager = localPlayer.PlantManager;
		}
		if (!SaveAndLoad.IsGameLoading && defaultReplenishItem != null)
		{
			StartCoroutine(RefillSlot(0.05f));
		}
		Invoke("OnFinishPlacementLate", 0.1f);
		plantManager.SendOnCropplotModified(this, PlantManager.ObjectModification.PLACED);
	}

	public virtual void OnFinishPlacementLate()
	{
	}

	public bool SetPlantComplete(int plantSlotIndex)
	{
		PlantationSlot slotFromIndex = GetSlotFromIndex(plantSlotIndex);
		if (slotFromIndex == null)
		{
			return false;
		}
		if (slotFromIndex.plant == null)
		{
			return false;
		}
		slotFromIndex.plant.Complete();
		return true;
	}

	public bool SlotsNeedWater()
	{
		for (int i = 0; i < plantationSlots.Count; i++)
		{
			PlantationSlot plantationSlot = plantationSlots[i];
			if (plantationSlot.busy && !plantationSlot.hasWater)
			{
				return true;
			}
		}
		return false;
	}

	public virtual void AddWater(bool playEffect = true)
	{
		if (plantationSlots == null || plantationSlots.Count == 0)
		{
			return;
		}
		for (int i = 0; i < plantationSlots.Count; i++)
		{
			PlantationSlot plantationSlot = plantationSlots[i];
			if (plantationSlot != null && plantationSlot.busy && !plantationSlot.hasWater)
			{
				plantationSlot.AddWater(playEffect);
			}
		}
		if (plantManager != null)
		{
			plantManager.SendOnCropplotModified(this, PlantManager.ObjectModification.WATERED);
		}
	}

	public IEnumerator AddWater(float delay, bool fromRain, bool playEffect = true)
	{
		yield return new WaitForSeconds(delay);
		if (plantManager != null)
		{
			plantManager.SendOnCropplotModified(this, PlantManager.ObjectModification.WATERED);
		}
		for (int i = 0; i < plantationSlots.Count; i++)
		{
			PlantationSlot plantationSlot = plantationSlots[i];
			if (plantationSlot.busy && !plantationSlot.hasWater)
			{
				plantationSlot.AddWater(playEffect);
			}
		}
	}

	public virtual Plant PlantSeed(Plant plantPrefab, uint plantObjectIndex, bool waterCropAswell, int slotIndex = -1)
	{
		if (IsFull)
		{
			return null;
		}
		if (plantPrefab == null)
		{
			return null;
		}
		PlantationSlot plantationSlot = ((slotIndex == -1) ? GetEmptyPlantationSlot() : plantationSlots[slotIndex]);
		if (plantationSlot == null)
		{
			return null;
		}
		Plant component = Object.Instantiate(plantPrefab.gameObject, plantationSlot.transform.position, plantPrefab.transform.rotation, transform).GetComponent<Plant>();
		if (component != null)
		{
			plantationSlot.plant = component;
			plantationSlot.busy = true;
			component.cropplot = this;
			component.plantationSlotIndex = plantationSlots.IndexOf(plantationSlot);
			component.SubscribeToOnPlantRemove(PlantRemoved);
			component.SetGrowTimer(0f);
			if (waterCropAswell && !Helper.CheckForRoofAbovePoint(transform.position, transform.up, 5))
			{
				StartCoroutine(AddWater(5f, fromRain: false, playEffect: false));
			}
			component.pickupComponent.networkID.ObjectIndex = plantObjectIndex;
			NetworkIDManager.AddNetworkID(component.pickupComponent.networkID);
			if (plantManager != null)
			{
				plantManager.SendOnCropplotModified(this, PlantManager.ObjectModification.PLANTED);
			}
		}
		return component;
	}

	public virtual void PlantRemoved(int slotIndex)
	{
		for (int i = 0; i < plantationSlots.Count; i++)
		{
			if (i == slotIndex)
			{
				PlantationSlot plantationSlot = plantationSlots[i];
				plantationSlot.busy = false;
				plantationSlot.RemoveWater();
				plantationSlot.OnWaterChange = null;
				if (defaultReplenishItem != null)
				{
					RefillSlot();
				}
				break;
			}
		}
	}

	public IEnumerator RefillSlot(float delay)
	{
		yield return new WaitForSeconds(delay);
		RefillSlot();
	}

	public void RefillSlot()
	{
		if (Raft_Network.IsHost)
		{
			Plant component = defaultReplenishItem.GetComponent<Plant>();
			if (!(component == null) && Raft_Network.IsHost)
			{
				Message_PlantSeed message_PlantSeed = new Message_PlantSeed(Messages.PlantManager_PlantSeed, plantManager, this, component, waterPlantedSeed: false);
				message_PlantSeed.plantObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
				message_PlantSeed.waterPlantedSeed = ComponentManager<WeatherManager>.Value.GetCurrentWeatherType() == UniqueWeatherType.Rain;
				plantManager.PlantSeed(this, component, message_PlantSeed.plantObjectIndex, message_PlantSeed.waterPlantedSeed, treatAsPlayerPlanted: false);
				network.RPC(message_PlantSeed, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public bool AcceptsPlantType(Item_Base item)
	{
		foreach (Item_Base acceptableItemType in acceptableItemTypes)
		{
			if (acceptableItemType == item)
			{
				return true;
			}
		}
		return false;
	}

	public List<PlantationSlot> GetSlots()
	{
		return plantationSlots;
	}

	private PlantationSlot GetSlotFromIndex(int slotIndex)
	{
		if (slotIndex < 0 || slotIndex >= plantationSlots.Count)
		{
			return null;
		}
		return plantationSlots[slotIndex];
	}

	private PlantationSlot GetEmptyPlantationSlot()
	{
		for (int i = 0; i < plantationSlots.Count; i++)
		{
			if (!plantationSlots[i].busy)
			{
				return plantationSlots[i];
			}
		}
		return null;
	}

	private int GetBusySlotCount()
	{
		int num = 0;
		for (int i = 0; i < plantationSlots.Count; i++)
		{
			if (plantationSlots[i].busy)
			{
				num++;
			}
		}
		return num;
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Cropplot(RGDType.Block_Cropplot, this);
	}
}
