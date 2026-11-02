using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class WindTurbine : MonoBehaviour_ID_Network
{
	public List<Battery> batteries = new List<Battery>();

	[SerializeField]
	private int batteryChargesPerTick = 1;

	[SerializeField]
	private TimerEventer rechargeTimer;

	[SerializeField]
	private Transform propellerBlades;

	[SerializeField]
	[Range(0f, 1f)]
	private float minEfficiancyValue = 0.33f;

	[SerializeField]
	private float maxEfficianyBlockHeight = 1f;

	[SerializeField]
	private float rotationSpeed = 1.5f;

	[SerializeField]
	private Gauge gauge;

	[SerializeField]
	private Interval_Int gaugeMinMaxAngle;

	[SerializeField]
	private StudioEventEmitter emitter_charge;

	[SerializeField]
	private StudioEventEmitter emitter_spin;

	private bool hasBeenPlaced;

	private float efficiancy;

	public void OnBlockPlaced()
	{
		if (Raft_Network.IsHost)
		{
			TimerEventer timerEventer = rechargeTimer;
			timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(ChargeBatteries));
		}
		Vector3 worldPosition = transform.position;
		BlockCreator.SnapBuildPositionWorldToLocalLockedPivot(ref worldPosition);
		efficiancy = Mathf.Clamp(worldPosition.y / maxEfficianyBlockHeight, minEfficiancyValue, 1f);
		gauge.SetTargetAngle(Mathf.Lerp(gaugeMinMaxAngle.minValue, gaugeMinMaxAngle.maxValue, efficiancy));
		NetworkIDManager.AddNetworkID(this);
		hasBeenPlaced = true;
		emitter_spin.PlaySafe();
		emitter_spin.SetParameter("speed", efficiancy);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this);
	}

	public void Update()
	{
		if (hasBeenPlaced)
		{
			if (propellerBlades != null)
			{
				propellerBlades.Rotate(Vector3.forward, efficiancy * rotationSpeed * (0f - Time.deltaTime));
			}
			if (Raft_Network.IsHost)
			{
				rechargeTimer.DoUpdate(Time.deltaTime * efficiancy, autoResetTimer: true);
				HandleChargeSound();
			}
		}
	}

	private void HandleChargeSound()
	{
		if (emitter_charge == null)
		{
			return;
		}
		if (batteries != null)
		{
			bool flag = false;
			foreach (Battery battery in batteries)
			{
				if (battery != null && !battery.BatterySlotIsEmpty && battery.NormalizedBatteryLeft != 1f)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				emitter_charge.PlaySafe();
			}
			else
			{
				emitter_charge.StopSafe();
			}
		}
		else
		{
			emitter_charge.StopSafe();
		}
	}

	private void ChargeBatteries()
	{
		foreach (Battery battery in batteries)
		{
			RechargeBattery(battery);
		}
	}

	private void RechargeBattery(Battery battery)
	{
		if (battery != null && !battery.BatterySlotIsEmpty && battery.NormalizedBatteryLeft != 1f)
		{
			battery.AddBatteryUsesNetworked(batteryChargesPerTick);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if ((uint)(type - 116) <= 2u)
		{
			Message_Battery message_Battery = msg as Message_Battery;
			if (msg != null)
			{
				foreach (Battery battery in batteries)
				{
					if (battery.BatteryIndex == message_Battery.batteryIndex)
					{
						return battery.OnBatteryMessage(msg);
					}
				}
			}
		}
		return base.Deserialize(msg, remoteID);
	}
}
