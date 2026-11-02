using System;
using FMODUnity;
using UnityEngine;

public class Placeable_RecyclerExtractor : Placeable_Extractor
{
	public Battery battery;

	public int batteryDrainPerMerge = 1;

	public Animator animator;

	public AnimatorMessageForwarder animForwarder;

	public EventReference er_crusherDown;

	public EventReference er_crusherReset;

	public void Start()
	{
		AnimatorMessageForwarder animatorMessageForwarder = animForwarder;
		animatorMessageForwarder.OnAnimatorMessage = (Action<string>)Delegate.Combine(animatorMessageForwarder.OnAnimatorMessage, new Action<string>(OnAnimatorMessage));
	}

	protected override void Update()
	{
		base.Update();
		animator.SetBool("IsActive", CanProcessTanks());
	}

	protected override bool CanProcessTanks()
	{
		if (battery.CanGiveElectricity && !outputTank.IsFull)
		{
			return base.CanProcessTanks();
		}
		return false;
	}

	protected override void ProcessTanks()
	{
		battery.RemoveBatteryUsesNetworked(batteryDrainPerMerge);
		base.ProcessTanks();
	}

	protected override int HarvestAmount()
	{
		if (Raft_Network.IsHost)
		{
			return Mathf.FloorToInt(outputTank.CurrentTankAmount);
		}
		return Mathf.FloorToInt(outputTank.previousTankAmount);
	}

	protected override void HandlePartialHarvest(Network_Player player)
	{
		outputTank.ModifyTankRPC(0f - outputTank.CurrentTankAmount, player);
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Block_Recycler(this);
	}

	public override RGD_Extractor GetSave()
	{
		return new RGD_Recycler(this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if ((uint)(type - 116) <= 2u)
		{
			Message_Battery message_Battery = msg as Message_Battery;
			if (msg != null && message_Battery.batteryIndex == battery.BatteryIndex)
			{
				return battery.OnBatteryMessage(msg);
			}
		}
		return base.Deserialize(msg, remoteID);
	}

	private void OnAnimatorMessage(string msg)
	{
		if (!(msg == "Up"))
		{
			if (msg == "Down")
			{
				RuntimeManager.PlayOneShotAttached(er_crusherDown, gameObject);
			}
		}
		else
		{
			RuntimeManager.PlayOneShotAttached(er_crusherReset, gameObject);
		}
	}
}
