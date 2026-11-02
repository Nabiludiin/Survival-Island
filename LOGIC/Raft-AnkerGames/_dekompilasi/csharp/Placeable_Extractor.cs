using System;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class Placeable_Extractor : MonoBehaviour_ID_Network
{
	public Tank processingTank;

	public Tank outputTank;

	public BlockButton outputButton;

	public Item_Base itemTypeOnHarvest;

	[SerializeField]
	protected StudioEventEmitter emitter_processTank;

	[SerializeField]
	protected Collider outputCollider;

	[SerializeField]
	protected Renderer outputRenderer;

	[SerializeField]
	protected FillContainerComponent fillContainer_processedResourceTank;

	[SerializeField]
	protected FillContainerComponent fillContainer_processing;

	[SerializeField]
	protected int processingTankDrainAtMerge = 10;

	[SerializeField]
	protected float processCooldown = 1f;

	[SerializeField]
	protected float processingTankAdditionAmount = 1f;

	protected Tank[] allTanks = new Tank[3];

	protected float processProgress;

	protected bool hasBeenPlaced;

	protected Raft_Network network;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Tank tank = outputTank;
		tank.OnTankAmountChange = (Action)Delegate.Remove(tank.OnTankAmountChange, new Action(OutputChangedCall));
		Tank tank2 = processingTank;
		tank2.OnTankAmountChange = (Action)Delegate.Remove(tank2.OnTankAmountChange, new Action(ProcessingTankChangedCall));
		BlockButton blockButton = outputButton;
		blockButton.OnButtonPressed = (Action)Delegate.Remove(blockButton.OnButtonPressed, new Action(ButtonHarvestOutput));
		NetworkIDManager.RemoveNetworkID(this);
	}

	protected virtual void Update()
	{
		if (!hasBeenPlaced)
		{
			return;
		}
		bool flag = CanProcessTanks();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		processProgress += Time.deltaTime;
		if (processProgress >= processCooldown)
		{
			processProgress = 0f;
			if (flag)
			{
				SendProcessExtractorMessageNetworked();
			}
		}
	}

	private void SendProcessExtractorMessageNetworked()
	{
		if (Raft_Network.IsHost)
		{
			ProcessTanks();
			Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.ExtractorProcessTanks, network.NetworkIDManager, ObjectIndex, network.LocalSteamID);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public override RGD Serialize_Save()
	{
		throw new NotImplementedException();
	}

	public virtual RGD_Extractor GetSave()
	{
		throw new NotImplementedException();
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Tank:
		case Messages.Tank_SetValue:
		{
			Message_Tank message_Tank = msg as Message_Tank;
			Tank[] array = allTanks;
			foreach (Tank tank in array)
			{
				if (tank.tankID == message_Tank.tankID)
				{
					if (msg.Type == Messages.Tank)
					{
						return tank.OnTankModifyMessage(message_Tank);
					}
					if (msg.Type == Messages.Tank_SetValue)
					{
						tank.OnTankSetValueMessage(message_Tank);
					}
				}
			}
			break;
		}
		case Messages.ExtractorProcessTanks:
			if (Raft_Network.IsHost)
			{
				Debug.Log("shouldnt be able to get this as host");
				return false;
			}
			ProcessTanks();
			return false;
		case Messages.ExtractorHarvestOutput:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			if (Raft_Network.IsHost)
			{
				if (CanHarvestOutputtedFuel())
				{
					HarvestOutput(network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
					return true;
				}
				return false;
			}
			HarvestOutput(network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
			return false;
		}
		}
		return base.Deserialize(msg, remoteID);
	}

	public void RestoreExtractor(RGD_Extractor rgdExtractor)
	{
		rgdExtractor.RestoreExtractor(this);
	}

	private void ButtonHarvestOutput()
	{
		if (Raft_Network.IsHost)
		{
			HarvestOutput(network.GetLocalPlayer());
			return;
		}
		Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.ExtractorHarvestOutput, network.NetworkIDManager, ObjectIndex, network.LocalSteamID);
		network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
	}

	private void ProcessingTankChangedCall()
	{
		if (fillContainer_processing != null)
		{
			fillContainer_processing.SetNormalizedValue(processingTank.NormalizedTankAmount, instant: false);
		}
	}

	protected virtual void OutputChangedCall()
	{
		if (outputTank.IsConsideredHarvestable())
		{
			SetOutPutEnabled(value: true);
		}
		else
		{
			SetOutPutEnabled(value: false);
		}
		if (fillContainer_processedResourceTank != null)
		{
			fillContainer_processedResourceTank.SetNormalizedValue(outputTank.NormalizedTankAmount, instant: false);
		}
	}

	protected virtual void OnBlockPlaced()
	{
		if (!hasBeenPlaced)
		{
			network = ComponentManager<Raft_Network>.Value;
			allTanks[0] = processingTank;
			allTanks[1] = outputTank;
			NetworkIDManager.AddNetworkID(this, typeof(Placeable_Extractor));
			hasBeenPlaced = true;
			Tank tank = outputTank;
			tank.OnTankAmountChange = (Action)Delegate.Combine(tank.OnTankAmountChange, new Action(OutputChangedCall));
			Tank tank2 = processingTank;
			tank2.OnTankAmountChange = (Action)Delegate.Combine(tank2.OnTankAmountChange, new Action(ProcessingTankChangedCall));
			BlockButton blockButton = outputButton;
			blockButton.OnButtonPressed = (Action)Delegate.Combine(blockButton.OnButtonPressed, new Action(ButtonHarvestOutput));
			SetOutPutEnabled(value: false);
		}
	}

	protected virtual bool CanProcessTanks()
	{
		return processingTank.CurrentTankAmount >= (float)processingTankDrainAtMerge;
	}

	protected virtual void ProcessTanks()
	{
		processingTank.ModifyTank(null, -processingTankDrainAtMerge);
		outputTank.ModifyTank(null, processingTankAdditionAmount);
	}

	protected virtual void HandleSounds(bool isActive)
	{
		if (isActive)
		{
			emitter_processTank.PlaySafe();
		}
		else
		{
			emitter_processTank.StopSafe();
		}
	}

	protected virtual int HarvestAmount()
	{
		return 1;
	}

	protected bool CanHarvestOutputtedFuel()
	{
		return outputTank.IsConsideredHarvestable();
	}

	protected virtual void HandlePartialHarvest(Network_Player player)
	{
		outputTank.ModifyTankRPC(0f - outputTank.minimumHarvestAmount, player);
	}

	private void HarvestOutput(Network_Player player)
	{
		if (player != null)
		{
			player.Animator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
		}
		if (player.IsLocalPlayer)
		{
			player.Inventory.AddItem(itemTypeOnHarvest.UniqueName, HarvestAmount());
		}
		switch (outputTank.harvestSetting)
		{
		default:
			outputTank.ModifyTankRPC(0f - outputTank.CurrentTankAmount, player);
			break;
		case Tank.HarvestSetting.PARTIAL:
			HandlePartialHarvest(player);
			break;
		}
		if (fillContainer_processedResourceTank != null)
		{
			fillContainer_processedResourceTank.SetNormalizedValue(0f, instant: true);
		}
	}

	private void SetOutPutEnabled(bool value)
	{
		outputCollider.enabled = value;
		if (outputRenderer != null)
		{
			outputRenderer.enabled = value;
		}
	}
}
