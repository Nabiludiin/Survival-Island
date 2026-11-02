using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_BatteryCharger_Create : Message_NetworkBehaviour
{
	public RGD_BatteryCharger[] rgdBatteryChargers;

	protected bool Equals(Message_BatteryCharger_Create other)
	{
		if (rgdBatteryChargers == null && other.rgdBatteryChargers != null)
		{
			return false;
		}
		if (rgdBatteryChargers != null && other.rgdBatteryChargers == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdBatteryChargers != null || other.rgdBatteryChargers != null)
			{
				return Enumerable.SequenceEqual(rgdBatteryChargers, other.rgdBatteryChargers);
			}
			return true;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((Message_BatteryCharger_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdBatteryChargers);
	}

	public static bool operator ==(Message_BatteryCharger_Create left, Message_BatteryCharger_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_BatteryCharger_Create left, Message_BatteryCharger_Create right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdBatteryChargers != null) ? rgdBatteryChargers.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdBatteryChargers != null)
		{
			RGD_BatteryCharger[] array = rgdBatteryChargers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SerializeFast(writer);
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			rgdBatteryChargers = new RGD_BatteryCharger[value];
			for (int i = 0; i < value; i++)
			{
				rgdBatteryChargers[i] = new RGD_BatteryCharger();
				rgdBatteryChargers[i].DeserializeFast(reader);
			}
		}
	}

	public Message_BatteryCharger_Create()
	{
	}

	public Message_BatteryCharger_Create(Messages type, MonoBehaviour_Network behaviour, BatteryCharger[] batteryChargers)
		: base(type, behaviour)
	{
		if (batteryChargers == null)
		{
			return;
		}
		List<RGD_BatteryCharger> list = new List<RGD_BatteryCharger>();
		for (int i = 0; i < batteryChargers.Length; i++)
		{
			BatteryCharger batteryCharger = batteryChargers[i];
			if (batteryCharger != null && (!batteryCharger.batteryL.BatterySlotIsEmpty || !batteryCharger.batteryR.BatterySlotIsEmpty || batteryCharger.fuelTank.CurrentTankAmount != 0f))
			{
				list.Add(new RGD_BatteryCharger(batteryChargers[i]));
			}
		}
		rgdBatteryChargers = list.ToArray();
	}

	public void RestoreBatteryChargers(BatteryCharger[] batteryChargers)
	{
		if (batteryChargers == null || !batteryChargers.ContainsItems() || rgdBatteryChargers == null || !rgdBatteryChargers.ContainsItems())
		{
			return;
		}
		for (int i = 0; i < rgdBatteryChargers.Length; i++)
		{
			RGD_BatteryCharger rGD_BatteryCharger = rgdBatteryChargers[i];
			foreach (BatteryCharger batteryCharger in batteryChargers)
			{
				if (rGD_BatteryCharger.objectIndex == batteryCharger.ObjectIndex)
				{
					rGD_BatteryCharger.RestoreBatteryCharger(batteryCharger);
					break;
				}
			}
		}
	}
}
