using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantManager : MonoBehaviour_Network, SceneEventInterface
{
	public enum ObjectModification
	{
		REMOVED,
		PLACED,
		WATERED,
		PLANTED,
		PLANTREMOVED
	}

	public static List<Cropplot> allCropplots = new List<Cropplot>();

	public Action<Cropplot, ObjectModification> OnCropplotModified;

	[SerializeField]
	private List<Plant> allPlants = new List<Plant>();

	public EventReference er_Plant;

	public EventReference er_Harvest;

	public EventReference er_WaterCropplot;

	private Network_Player playerNetwork;

	private PlayerInventory playerInventory;

	private PlayerAnimator playerAnimator;

	private CanvasHelper canvas;

	private WeatherManager weatherManager;

	private bool harvestAim;

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
		playerAnimator = playerNetwork.Animator;
		playerInventory = playerNetwork.Inventory;
		canvas = ComponentManager<CanvasHelper>.Value;
		weatherManager = ComponentManager<WeatherManager>.Value;
		if (Raft_Network.IsHost)
		{
			WeatherManager.OnWeatherChanged = (Action<Weather>)Delegate.Combine(WeatherManager.OnWeatherChanged, new Action<Weather>(OnWeatherChange));
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			allCropplots.Clear();
		}
	}

	private void Update()
	{
		HarvestPlants();
		if (!Cheat.UseCheats || !Input.GetKeyDown(KeyCode.G) || !Input.GetKey(KeyCode.LeftControl))
		{
			return;
		}
		foreach (Cropplot allCropplot in allCropplots)
		{
			foreach (PlantationSlot plantationSlot in allCropplot.plantationSlots)
			{
				if (plantationSlot != null && plantationSlot.plant != null)
				{
					plantationSlot.AddWater();
					plantationSlot.plant.SetGrowTimer(float.MaxValue);
				}
			}
		}
	}

	private void HarvestPlants()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None || !playerNetwork.IsLocalPlayer)
		{
			return;
		}
		if (Helper.HitAtCursor(out var hit, Player.UseDistance, LayerMasks.MASK_Item) && hit.transform.tag == "Plant")
		{
			Plant component = hit.transform.GetComponent<Plant>();
			if (component.FullyGrown() && component.harvestable && component.playerCanHarvest)
			{
				InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Harvest"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
				harvestAim = true;
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
				{
					canvas.displayTextManager.HideDisplayTexts();
					Message_HarvestPlant message = new Message_HarvestPlant(Messages.PlantManager_HarvestPlant, this, component, giveYield: true);
					if (Raft_Network.IsHost)
					{
						playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
						Harvest(component, giveYield: true);
					}
					else
					{
						playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
				}
			}
			else if (harvestAim)
			{
				canvas.displayTextManager.HideDisplayTexts();
				harvestAim = false;
			}
		}
		else if (harvestAim)
		{
			canvas.displayTextManager.HideDisplayTexts();
			harvestAim = false;
		}
	}

	public void OnWeatherChange(Weather weather)
	{
		if (!(weather == null) && weather.so_weather.uniqueWeatherType == UniqueWeatherType.Rain)
		{
			WaterAllPlantsWithRain();
		}
	}

	public void WaterAllPlantsWithRain()
	{
		if (this == null || (allCropplots == null && allCropplots.Count <= 0))
		{
			return;
		}
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		for (int i = 0; i < allCropplots.Count; i++)
		{
			Cropplot cropplot = allCropplots[i];
			if (!(cropplot == null) && cropplot.SlotsNeedWater() && !Helper.CheckForRoofAbovePoint(cropplot.transform.position, cropplot.transform.up, 5))
			{
				cropplot.AddWater(playEffect: false);
				Message_WaterCrop message = new Message_WaterCrop(Messages.PlantManager_WaterPlant, this, cropplot, playSound: false);
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void ForwardTime(float secondsToForward)
	{
		foreach (Cropplot allCropplot in allCropplots)
		{
			foreach (PlantationSlot plantationSlot in allCropplot.plantationSlots)
			{
				if (plantationSlot.CanGrow)
				{
					plantationSlot.plant.IncrementGrowTimer(secondsToForward);
				}
			}
		}
	}

	public Plant GetPlantByIndex(int uniqueItemIndex)
	{
		foreach (Plant allPlant in allPlants)
		{
			if (allPlant.item.UniqueIndex == uniqueItemIndex)
			{
				return allPlant;
			}
		}
		return null;
	}

	public bool PlantSeed(Cropplot cropplot, Plant plantPrefab, uint plantObjectIndex, bool waterCropAswell, bool treatAsPlayerPlanted = true)
	{
		if (cropplot == null || plantPrefab == null)
		{
			return false;
		}
		Plant plant = cropplot.PlantSeed(plantPrefab, plantObjectIndex, waterCropAswell);
		if (plant != null)
		{
			RuntimeManager.PlayOneShot(er_Plant, plant.transform.position);
			if (treatAsPlayerPlanted)
			{
				if (playerNetwork == null)
				{
					playerNetwork = GetComponentInParent<Network_Player>();
				}
				if (playerNetwork.IsLocalPlayer)
				{
					playerNetwork.Inventory.RemoveItem(plantPrefab.item.UniqueName, 1);
					playerNetwork.Animator.SetAnimation(PlayerAnimation.Trigger_Plant, triggering: true);
				}
			}
		}
		return plant != null;
	}

	public void Harvest(Plant plant, bool giveYield)
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (!(plant == null))
		{
			RuntimeManager.PlayOneShot(er_Harvest, plant.transform.position);
			plant.PullRoots();
			if (playerNetwork.IsLocalPlayer & giveYield)
			{
				playerNetwork.PickupScript.PickupItem(plant.pickupComponent, forcePickup: true);
			}
			else
			{
				PickupObjectManager.RemovePickupItem(plant.pickupComponent.networkID);
			}
			if (plant.cropplot != null)
			{
				SendOnCropplotModified(plant.cropplot, ObjectModification.PLANTREMOVED);
			}
		}
	}

	public void WaterCropplot(Cropplot cropplot, bool playSound)
	{
		if (!(cropplot == null) && cropplot.SlotsNeedWater())
		{
			cropplot.AddWater();
			if (playSound)
			{
				RuntimeManager.PlayOneShot(er_WaterCropplot, cropplot.transform.position);
			}
		}
	}

	public void SendOnCropplotModified(Cropplot cropplot, ObjectModification modification)
	{
		OnCropplotModified?.Invoke(cropplot, modification);
	}

	private static Cropplot GetCropplotByIndex(uint objectIndex)
	{
		foreach (Cropplot allCropplot in allCropplots)
		{
			if (allCropplot.ObjectIndex == objectIndex)
			{
				return allCropplot;
			}
		}
		return null;
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_PlantManagerCreate(Messages.PlantManagerCreate, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.PlantManagerCreate:
		{
			Message_PlantManagerCreate message_PlantManagerCreate = msg as Message_PlantManagerCreate;
			if (message_PlantManagerCreate.rgdCropplots != null)
			{
				for (int i = 0; i < message_PlantManagerCreate.rgdCropplots.Count; i++)
				{
					RGD_Cropplot rGD_Cropplot = message_PlantManagerCreate.rgdCropplots[i];
					if (rGD_Cropplot != null)
					{
						Cropplot cropplotByIndex2 = GetCropplotByIndex(rGD_Cropplot.cropplotObjectIndex);
						if (cropplotByIndex2 != null)
						{
							rGD_Cropplot.RestoreCropplot(cropplotByIndex2, this);
						}
					}
				}
			}
			return false;
		}
		case Messages.PlantManager_PlantSeed:
		{
			Message_PlantSeed message_PlantSeed = msg as Message_PlantSeed;
			Plant plantByIndex = GetPlantByIndex(message_PlantSeed.plantUniqueIndex);
			if (!(plantByIndex != null))
			{
				break;
			}
			Cropplot cropplotByIndex4 = GetCropplotByIndex(message_PlantSeed.cropplotObjectIndex);
			if (cropplotByIndex4 != null)
			{
				if (Raft_Network.IsHost)
				{
					message_PlantSeed.plantObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
					WeatherManager value = ComponentManager<WeatherManager>.Value;
					Weather weather = ((value == null) ? null : value.GetCurrentWeather());
					message_PlantSeed.waterPlantedSeed = weather != null && weather.so_weather.uniqueWeatherType == UniqueWeatherType.Rain && cropplotByIndex4.SlotsNeedWater();
				}
				return PlantSeed(cropplotByIndex4, plantByIndex, message_PlantSeed.plantObjectIndex, message_PlantSeed.waterPlantedSeed);
			}
			break;
		}
		case Messages.PlantManager_HarvestPlant:
		{
			Message_HarvestPlant message_HarvestPlant = msg as Message_HarvestPlant;
			PickupItem_Networked networkIDFromObjectIndex = NetworkIDManager.GetNetworkIDFromObjectIndex<PickupItem_Networked>(message_HarvestPlant.plantObjectIndex);
			if (networkIDFromObjectIndex != null)
			{
				Plant component = networkIDFromObjectIndex.GetComponent<Plant>();
				if (component != null)
				{
					Harvest(component, message_HarvestPlant.giveYield);
					return true;
				}
			}
			break;
		}
		case Messages.PlantManager_WaterPlant:
		{
			Message_WaterCrop message_WaterCrop = msg as Message_WaterCrop;
			Cropplot cropplotByIndex3 = GetCropplotByIndex(message_WaterCrop.cropplotObjectIndex);
			if (cropplotByIndex3 != null)
			{
				WaterCropplot(cropplotByIndex3, message_WaterCrop.playSound);
				return true;
			}
			return false;
		}
		case Messages.PlantManager_PlantComplete:
		{
			Message_Plant_Complete message_Plant_Complete = msg as Message_Plant_Complete;
			Cropplot cropplotByIndex = GetCropplotByIndex(message_Plant_Complete.cropplotObjectIndex);
			if (cropplotByIndex != null)
			{
				return cropplotByIndex.SetPlantComplete(message_Plant_Complete.plantationSlotIndex);
			}
			return false;
		}
		}
		return false;
	}
}
