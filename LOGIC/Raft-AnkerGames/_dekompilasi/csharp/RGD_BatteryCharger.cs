using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_BatteryCharger : SerializableBehaviour
{
	public RGD_Battery RGDbatteryL;

	public RGD_Battery RGDbatteryR;

	public float tankAmount;

	public uint objectIndex;

	protected bool Equals(RGD_BatteryCharger other)
	{
		if (object.Equals(RGDbatteryL, other.RGDbatteryL) && object.Equals(RGDbatteryR, other.RGDbatteryR) && tankAmount.Equals(other.tankAmount))
		{
			return objectIndex == other.objectIndex;
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
		return Equals((RGD_BatteryCharger)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(RGDbatteryL, RGDbatteryR, tankAmount, objectIndex);
	}

	public static bool operator ==(RGD_BatteryCharger left, RGD_BatteryCharger right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_BatteryCharger left, RGD_BatteryCharger right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<bool>(RGDbatteryL != null, default(FastBufferWriter.ForPrimitives));
		if (RGDbatteryL != null)
		{
			RGDbatteryL.SerializeFast(writer);
		}
		writer.WriteValueSafe<bool>(RGDbatteryR != null, default(FastBufferWriter.ForPrimitives));
		if (RGDbatteryR != null)
		{
			RGDbatteryR.SerializeFast(writer);
		}
		writer.WriteValueSafe(in tankAmount, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in objectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		bool value = false;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value)
		{
			RGDbatteryL = new RGD_Battery();
			RGDbatteryL.DeserializeFast(reader);
		}
		bool value2 = false;
		reader.ReadValue(out value2, default(FastBufferWriter.ForPrimitives));
		if (value2)
		{
			RGDbatteryR = new RGD_Battery();
			RGDbatteryR.DeserializeFast(reader);
		}
		reader.ReadValue(out tankAmount, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out objectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_BatteryCharger()
	{
	}

	public RGD_BatteryCharger(BatteryCharger batteryCharger)
	{
		tankAmount = ((batteryCharger.fuelTank.CurrentTankAmount > 0f) ? batteryCharger.fuelTank.CurrentTankAmount : (-1f));
		RGDbatteryL = (batteryCharger.batteryL.BatterySlotIsEmpty ? null : new RGD_Battery(batteryCharger.batteryL));
		RGDbatteryR = (batteryCharger.batteryR.BatterySlotIsEmpty ? null : new RGD_Battery(batteryCharger.batteryR));
		objectIndex = batteryCharger.ObjectIndex;
	}

	public void RestoreBatteryCharger(BatteryCharger batteryCharger)
	{
		if (!(batteryCharger == null))
		{
			if (RGDbatteryL != null)
			{
				RGDbatteryL.RestoreBattery(batteryCharger.batteryL);
			}
			if (RGDbatteryR != null)
			{
				RGDbatteryR.RestoreBattery(batteryCharger.batteryR);
			}
			if (tankAmount > 0f)
			{
				batteryCharger.fuelTank.SetTankAmount(tankAmount);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDBatteryL", RGDbatteryL);
		info.AddValue("RGDBatteryR", RGDbatteryR);
		info.AddValue("TankAmount", tankAmount);
		info.AddValue("ObjectIndex", objectIndex);
	}

	public RGD_BatteryCharger(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "BatteryUsesL":
				RGDbatteryL = new RGD_Battery(Helper.ObjToValue<int>(item.Value));
				break;
			case "BatteryUsesR":
				RGDbatteryR = new RGD_Battery(Helper.ObjToValue<int>(item.Value));
				break;
			case "TankAmount":
				tankAmount = Helper.ObjToValue<float>(item.Value);
				break;
			case "ObjectIndex":
				objectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			case "RGDBatteryL":
				RGDbatteryL = (RGD_Battery)item.Value;
				break;
			case "RGDBatteryR":
				RGDbatteryR = (RGD_Battery)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		RGDbatteryL = (RGDbatteryR = null);
		tankAmount = -1f;
		objectIndex = 0u;
	}
}
