using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class BeeHive : MonoBehaviour_ID_Network
{
	[HideInInspector]
	public int currentHoneyLevelIndex = -1;

	public ObjectEnabler honeyModelAlternator;

	public ParticleSystem beeParticleSystem;

	public LayerMask cropplotLayer;

	[SerializeField]
	private BlockButton_PickupItemText harvestBlockButton;

	[SerializeField]
	private Beehive_HoneyLevel[] honeyLevels;

	[SerializeField]
	private float _blockAmountMaxDistanceToFlowerCropplots = 4f;

	[SerializeField]
	private float productionCooldown = 5f;

	[SerializeField]
	private float checkParticleCooldown = 5f;

	[SerializeField]
	private StudioEventEmitter beeAmbienceEmitter;

	private List<Cropplot> plantationCropplots = new List<Cropplot>();

	private Raft_Network network;

	private Network_Player localPlayer;

	[HideInInspector]
	public TimerEventer productionTimer;

	private TimerEventer checkParticleTimer;

	private bool isPlaced;

	private int flowersInPlantationSlots;

	public float FlowerFindDistance => 1.5f * _blockAmountMaxDistanceToFlowerCropplots;

	public Beehive_HoneyLevel CurrentHoneyLevel
	{
		get
		{
			if (currentHoneyLevelIndex >= 0 && currentHoneyLevelIndex < honeyLevels.Length)
			{
				return honeyLevels[currentHoneyLevelIndex];
			}
			return null;
		}
	}

	public void CheckFlowerCount()
	{
		Debug.Log(GetWateredFlowerPlantationSlotCount() + " flowers in my slots");
	}

	protected override void OnDestroy()
	{
		if (isPlaced)
		{
			NetworkIDManager.RemoveNetworkID(this);
			if (checkParticleTimer != null)
			{
				TimerEventer timerEventer = checkParticleTimer;
				timerEventer.OnTimerReached = (Action)Delegate.Remove(timerEventer.OnTimerReached, new Action(RecheckBeeParticles));
			}
			if (productionTimer != null)
			{
				TimerEventer timerEventer2 = productionTimer;
				timerEventer2.OnTimerReached = (Action)Delegate.Remove(timerEventer2.OnTimerReached, new Action(AttemptProduction));
			}
			if (harvestBlockButton != null)
			{
				BlockButton_PickupItemText blockButton_PickupItemText = harvestBlockButton;
				blockButton_PickupItemText.OnButtonPressed = (Action)Delegate.Remove(blockButton_PickupItemText.OnButtonPressed, new Action(ButtonHarvestYield));
			}
			if (localPlayer != null)
			{
				PlantManager plantManager = localPlayer.PlantManager;
				plantManager.OnCropplotModified = (Action<Cropplot, PlantManager.ObjectModification>)Delegate.Remove(plantManager.OnCropplotModified, new Action<Cropplot, PlantManager.ObjectModification>(HandleCropplotModified));
			}
			productionTimer = null;
			checkParticleTimer = null;
		}
	}

	protected void Update()
	{
		if (isPlaced)
		{
			checkParticleTimer.DoUpdate(Time.deltaTime, autoResetTimer: true);
			if (Raft_Network.IsHost)
			{
				productionTimer.DoUpdate(Time.deltaTime, autoResetTimer: true);
			}
		}
	}

	public void OnBlockPlaced()
	{
		NetworkIDManager.AddNetworkID(this);
		isPlaced = true;
		network = ComponentManager<Raft_Network>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
		if (localPlayer != null)
		{
			PlantManager plantManager = localPlayer.PlantManager;
			plantManager.OnCropplotModified = (Action<Cropplot, PlantManager.ObjectModification>)Delegate.Combine(plantManager.OnCropplotModified, new Action<Cropplot, PlantManager.ObjectModification>(HandleCropplotModified));
		}
		else
		{
			Debug.LogWarning("Plant manager null on beehive placed, something wrong", gameObject);
		}
		if (harvestBlockButton != null)
		{
			BlockButton_PickupItemText blockButton_PickupItemText = harvestBlockButton;
			blockButton_PickupItemText.OnButtonPressed = (Action)Delegate.Combine(blockButton_PickupItemText.OnButtonPressed, new Action(ButtonHarvestYield));
		}
		else
		{
			Debug.LogWarning("harvest block button null  on beehive placed, something wrong", gameObject);
		}
		plantationCropplots.AddRange(Helper.FindNearbyCropplots(transform.position, FlowerFindDistance, cropplotLayer));
		flowersInPlantationSlots = GetWateredFlowerPlantationSlotCount();
		RecheckBeeParticles();
		checkParticleTimer = new TimerEventer(checkParticleCooldown + UnityEngine.Random.Range(0f, 1f));
		TimerEventer timerEventer = checkParticleTimer;
		timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(RecheckBeeParticles));
		if (Raft_Network.IsHost)
		{
			productionTimer = new TimerEventer(productionCooldown + UnityEngine.Random.Range(0f, 0.3f));
			TimerEventer timerEventer2 = productionTimer;
			timerEventer2.OnTimerReached = (Action)Delegate.Combine(timerEventer2.OnTimerReached, new Action(AttemptProduction));
		}
	}

	protected void AttemptProduction()
	{
		int num = CalculateHoneyLevelIndex(flowersInPlantationSlots);
		if (num > currentHoneyLevelIndex)
		{
			UpdateHoneyLevel(num);
			if (CurrentHoneyLevel != null)
			{
				Message_FillYield message = new Message_FillYield(Messages.BeeHiveFillYield, network.NetworkIDManager, currentHoneyLevelIndex, ObjectIndex);
				if (Raft_Network.IsHost)
				{
					network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
		productionTimer.ResetTimer();
	}

	protected void RecheckBeeParticles()
	{
		if (!Raft_Network.IsHost)
		{
			flowersInPlantationSlots = GetWateredFlowerPlantationSlotCount();
		}
		int num = CalculateHoneyLevelIndex(flowersInPlantationSlots);
		if (num >= 0 && num < honeyLevels.Length)
		{
			ParticleSystem.EmissionModule emission = beeParticleSystem.emission;
			emission.rateOverTime = honeyLevels[num].beeParticleEmissionRate;
			SetBeeAmbienceSoundActive(value: true);
		}
		else
		{
			ParticleSystem.EmissionModule emission2 = beeParticleSystem.emission;
			emission2.rateOverTime = 0f;
			SetBeeAmbienceSoundActive(value: false);
		}
	}

	protected void ButtonHarvestYield()
	{
		if (CanHarvest())
		{
			Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.BeeHiveHarvestOutput, network.NetworkIDManager, ObjectIndex, network.LocalSteamID);
			if (Raft_Network.IsHost)
			{
				HarvestYield(network.GetLocalPlayer());
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void UpdateHoneyLevel(int honeyLevelIndex)
	{
		currentHoneyLevelIndex = honeyLevelIndex;
		if (currentHoneyLevelIndex == -1)
		{
			honeyModelAlternator.DeactivateAllObject();
		}
		else
		{
			honeyModelAlternator.ActivateIndex(currentHoneyLevelIndex, deactivateOthers: true);
		}
	}

	private void HarvestYield(Network_Player player)
	{
		if (productionTimer != null)
		{
			productionTimer.ResetTimer();
		}
		if (player != null && player.IsLocalPlayer)
		{
			player.Animator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
			Beehive_HoneyLevel currentHoneyLevel = CurrentHoneyLevel;
			if (currentHoneyLevel != null)
			{
				for (int i = 0; i < currentHoneyLevel.yield.yieldAssets.Count; i++)
				{
					Cost cost = currentHoneyLevel.yield.yieldAssets[i];
					if (cost != null)
					{
						player.Inventory.AddItem(cost.item.UniqueName, cost.amount);
					}
				}
			}
		}
		UpdateHoneyLevel(-1);
		RecheckBeeParticles();
	}

	private int CalculateHoneyLevelIndex(int currentFlowerCount)
	{
		int num = -1;
		for (int i = 0; i < honeyLevels.Length; i++)
		{
			Beehive_HoneyLevel beehive_HoneyLevel = honeyLevels[i];
			if (beehive_HoneyLevel != null && currentFlowerCount >= beehive_HoneyLevel.minFlowerCount)
			{
				if (num == -1)
				{
					num = i;
				}
				else if (honeyLevels[num].minFlowerCount < beehive_HoneyLevel.minFlowerCount)
				{
					num = i;
				}
			}
		}
		return num;
	}

	private bool CanHarvest()
	{
		return CurrentHoneyLevel != null;
	}

	private void SetBeeAmbienceSoundActive(bool value)
	{
		if (value)
		{
			beeAmbienceEmitter.PlaySafe();
		}
		else
		{
			beeAmbienceEmitter.StopSafe();
		}
	}

	private int GetWateredFlowerPlantationSlotCount()
	{
		int num = 0;
		foreach (Cropplot plantationCropplot in plantationCropplots)
		{
			if (plantationCropplot == null)
			{
				continue;
			}
			foreach (PlantationSlot plantationSlot in plantationCropplot.plantationSlots)
			{
				if (plantationSlot != null && IsPlantationSlotFlower(plantationSlot) && plantationSlot.hasWater)
				{
					num++;
				}
			}
		}
		return num;
	}

	private bool IsPlantationSlotFlower(PlantationSlot slot)
	{
		if (slot != null && slot.plant != null)
		{
			Item_Base item = slot.plant.item;
			if (item != null && IsItemBaseFlower(item))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsItemBaseFlower(Item_Base itemInstance)
	{
		int uniqueIndex = itemInstance.UniqueIndex;
		if (uniqueIndex == 119 || uniqueIndex == 120 || uniqueIndex == 121 || uniqueIndex == 122 || uniqueIndex == 123)
		{
			return true;
		}
		return false;
	}

	private void HandleCropplotModified(Cropplot plot, PlantManager.ObjectModification modification)
	{
		if (plot == null || !(Vector3.Distance(plot.transform.position, transform.position) < FlowerFindDistance))
		{
			return;
		}
		bool flag = false;
		switch (modification)
		{
		case PlantManager.ObjectModification.REMOVED:
			flag = true;
			if (plantationCropplots.Contains(plot))
			{
				plantationCropplots.Remove(plot);
			}
			break;
		case PlantManager.ObjectModification.PLACED:
			if (!plantationCropplots.Contains(plot))
			{
				plantationCropplots.Add(plot);
			}
			break;
		case PlantManager.ObjectModification.WATERED:
		case PlantManager.ObjectModification.PLANTREMOVED:
			flag = true;
			break;
		}
		if (flag)
		{
			flowersInPlantationSlots = GetWateredFlowerPlantationSlotCount();
			if (modification != PlantManager.ObjectModification.REMOVED)
			{
				RecheckBeeParticles();
			}
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Block_BeeHive(RGDType.Block_BeeHive, GetComponent<Block>(), this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.BeeHiveHarvestOutput:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			if (Raft_Network.IsHost)
			{
				if (CanHarvest())
				{
					HarvestYield(network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
					return true;
				}
				return false;
			}
			HarvestYield(network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
			return false;
		}
		case Messages.BeeHiveFillYield:
		{
			Message_FillYield message_FillYield = msg as Message_FillYield;
			if (Raft_Network.IsHost)
			{
				Debug.Log("Host should not recieve this message");
				return false;
			}
			UpdateHoneyLevel(message_FillYield.currentHoneyLevelIndex);
			break;
		}
		}
		return base.Deserialize(msg, remoteID);
	}
}
