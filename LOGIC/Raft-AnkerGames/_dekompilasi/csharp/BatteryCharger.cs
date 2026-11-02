using System;
using UnityEngine;

public class BatteryCharger : MonoBehaviour_ID_Network
{
	public Battery batteryL;

	public Battery batteryR;

	public Tank fuelTank;

	public Animator animator;

	public ParticleSystem smokeParticles;

	public float chargeTriggerRate = 10f;

	public float visualBatterChargerUpdateRate = 1f;

	public int chargePerFuelTick = 4;

	[SerializeField]
	protected StudioEventEmitterSustain eventEmitter_charging;

	private Raft_Network network;

	private TimerEventer chargeTimer;

	private TimerEventer updateCanChargeTimer;

	private Network_Player localPlayer;

	private bool hasBeenPlaced;

	private void OnValidate()
	{
		if (animator == null)
		{
			animator = GetComponentInChildren<Animator>();
		}
	}

	protected override void OnDestroy()
	{
		if (hasBeenPlaced)
		{
			if (chargeTimer != null)
			{
				chargeTimer.OnTimerReached = null;
			}
			if (updateCanChargeTimer != null)
			{
				updateCanChargeTimer.OnTimerReached = null;
			}
			NetworkIDManager.RemoveNetworkID(this);
		}
	}

	public void OnBlockPlaced()
	{
		network = ComponentManager<Raft_Network>.Value;
		hasBeenPlaced = true;
		localPlayer = network.GetLocalPlayer();
		if (Raft_Network.IsHost)
		{
			chargeTimer = new TimerEventer(chargeTriggerRate + UnityEngine.Random.Range(-0.2f, 0.2f));
			TimerEventer timerEventer = chargeTimer;
			timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(OnTriggerBatteryCharge));
		}
		updateCanChargeTimer = new TimerEventer(visualBatterChargerUpdateRate);
		TimerEventer timerEventer2 = updateCanChargeTimer;
		timerEventer2.OnTimerReached = (Action)Delegate.Combine(timerEventer2.OnTimerReached, new Action(OnDoRequireChargeCheck));
		NetworkIDManager.AddNetworkID(this);
	}

	private void Update()
	{
		if (hasBeenPlaced)
		{
			if (Raft_Network.IsHost)
			{
				chargeTimer.DoUpdate(Time.deltaTime, autoResetTimer: true);
			}
			updateCanChargeTimer.DoUpdate(Time.deltaTime, autoResetTimer: true);
		}
	}

	private void OnTriggerBatteryCharge()
	{
		if (fuelTank.NormalizedTankAmount > 0f)
		{
			int num = 0;
			if (AttemptRechargeBattery(batteryL))
			{
				num++;
			}
			if (AttemptRechargeBattery(batteryR))
			{
				num++;
			}
			if (num > 0)
			{
				fuelTank.ModifyTankRPC(-num, null);
			}
		}
	}

	private void OnDoRequireChargeCheck()
	{
		if (fuelTank.NormalizedTankAmount > 0f)
		{
			bool flag = false;
			if (DoesRequireCharge(batteryL))
			{
				flag = true;
			}
			if (DoesRequireCharge(batteryR))
			{
				flag = true;
			}
			animator.SetBool("IsOn", flag);
			if (flag)
			{
				smokeParticles.PlaySafe();
				eventEmitter_charging.Play();
			}
			else
			{
				smokeParticles.StopSafe();
				eventEmitter_charging.Stop();
			}
		}
		else
		{
			animator.SetBool("IsOn", value: false);
			smokeParticles.StopSafe();
			eventEmitter_charging.Stop();
		}
	}

	private bool AttemptRechargeBattery(Battery battery)
	{
		bool result = false;
		if (battery != null && !battery.BatterySlotIsEmpty && battery.NormalizedBatteryLeft != 1f)
		{
			RechargeBattery(battery);
			result = true;
		}
		return result;
	}

	private bool DoesRequireCharge(Battery battery)
	{
		ItemInstance itemInstance = null;
		if (battery != null)
		{
			itemInstance = battery.GetIsBatteryValid();
		}
		if (itemInstance != null)
		{
			if (!itemInstance.HasMaxUses)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	private void RechargeBattery(Battery battery)
	{
		if (battery != null)
		{
			battery.AddBatteryUsesNetworked(chargePerFuelTick);
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Block_BatteryCharger(RGDType.Block_BatteryCharger, GetComponent<Block>(), this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Tank:
			return fuelTank.OnTankModifyMessage(msg as Message_Tank);
		case Messages.Tank_SetValue:
			fuelTank.OnTankSetValueMessage(msg as Message_Tank);
			break;
		case Messages.Battery_Insert:
		case Messages.Battery_Take:
		case Messages.Battery_Update:
		{
			Message_Battery message_Battery = msg as Message_Battery;
			if (msg != null)
			{
				if (message_Battery.batteryIndex == batteryL.BatteryIndex)
				{
					return batteryL.OnBatteryMessage(msg);
				}
				return batteryR.OnBatteryMessage(msg);
			}
			break;
		}
		}
		return base.Deserialize(msg, remoteID);
	}
}
