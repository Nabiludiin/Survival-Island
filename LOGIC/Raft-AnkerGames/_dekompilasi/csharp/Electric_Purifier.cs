using System;
using UnityEngine;

public class Electric_Purifier : MonoBehaviour_ID_Network, IBlockPlaced
{
	public Tank tank;

	public Battery battery;

	public ObjectScaler waterGaugeFill;

	public TimerEventer pumpTimer;

	[SerializeField]
	private int batteryDrainPerPump = 1;

	[SerializeField]
	private int tankFillPerPump = 1;

	[SerializeField]
	private Animator pumpAnimator;

	[SerializeField]
	private StudioEventEmitterSustain emitter_Running;

	private bool isPlaced;

	private bool isOn;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this, typeof(Electric_Purifier));
	}

	public void Update()
	{
		if (!isPlaced)
		{
			return;
		}
		if (battery.CanGiveElectricity && battery.BatteryUses >= batteryDrainPerPump && !tank.IsFull && tank.CurrentTankAmount <= tank.maxCapacity)
		{
			if (Raft_Network.IsHost)
			{
				pumpTimer.DoUpdate(Time.deltaTime, autoResetTimer: true);
			}
			SetMachineState(value: true);
		}
		else
		{
			SetMachineState(value: false);
		}
	}

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		isPlaced = true;
		NetworkIDManager.AddNetworkID(this, typeof(Electric_Purifier));
		Tank tank = this.tank;
		tank.OnTankAmountChange = (Action)Delegate.Combine(tank.OnTankAmountChange, new Action(OnTankAmountChanged));
		if (Raft_Network.IsHost)
		{
			TimerEventer timerEventer = pumpTimer;
			timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(OnSuccessfullPump));
		}
	}

	private void OnSuccessfullPump()
	{
		if (battery != null)
		{
			battery.AddBatteryUsesNetworked(-batteryDrainPerPump);
		}
		if (tank != null)
		{
			tank.ModifyTankRPC(tankFillPerPump, null);
		}
	}

	private void OnTankAmountChanged()
	{
		UpdateWaterMeterFill();
	}

	private void UpdateWaterMeterFill()
	{
		waterGaugeFill.SetNormalizedValue(tank.NormalizedTankAmount, instant: false);
	}

	private void SetMachineState(bool value)
	{
		if (isOn != value)
		{
			isOn = value;
			if (pumpAnimator != null)
			{
				pumpAnimator.SetBool("On", isOn);
			}
			if (isOn)
			{
				emitter_Running?.Play();
			}
			else
			{
				emitter_Running?.Stop();
			}
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Block_ElectricPurifier(this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Tank:
			return tank.OnTankModifyMessage(msg as Message_Tank);
		case Messages.Tank_SetValue:
			tank.OnTankSetValueMessage(msg as Message_Tank);
			break;
		case Messages.Battery_Insert:
		case Messages.Battery_Take:
		case Messages.Battery_Update:
		{
			Message_Battery message_Battery = msg as Message_Battery;
			if (msg != null)
			{
				_ = message_Battery.batteryIndex;
				_ = battery.BatteryIndex;
				return battery.OnBatteryMessage(msg);
			}
			break;
		}
		}
		return base.Deserialize(msg, remoteID);
	}
}
