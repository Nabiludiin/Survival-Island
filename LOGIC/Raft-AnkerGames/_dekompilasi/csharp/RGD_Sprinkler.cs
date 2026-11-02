using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Sprinkler : SerializableBehaviour
{
	public RGD_Battery RGDBattery;

	public float waterTankAmount;

	public uint sprinklerObjectIndex;

	protected bool Equals(RGD_Sprinkler other)
	{
		if (object.Equals(RGDBattery, other.RGDBattery) && waterTankAmount.Equals(other.waterTankAmount))
		{
			return sprinklerObjectIndex == other.sprinklerObjectIndex;
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
		return Equals((RGD_Sprinkler)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(RGDBattery, waterTankAmount, sprinklerObjectIndex);
	}

	public static bool operator ==(RGD_Sprinkler left, RGD_Sprinkler right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Sprinkler left, RGD_Sprinkler right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<bool>(RGDBattery != null, default(FastBufferWriter.ForPrimitives));
		if (RGDBattery != null)
		{
			RGDBattery.SerializeFast(writer);
		}
		writer.WriteValueSafe(in waterTankAmount, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in sprinklerObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		bool value = false;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value)
		{
			RGDBattery = new RGD_Battery();
			RGDBattery.DeserializeFast(reader);
		}
		reader.ReadValue(out waterTankAmount, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out sprinklerObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Sprinkler()
	{
	}

	public RGD_Sprinkler(Sprinkler sprinkler)
	{
		waterTankAmount = ((sprinkler.waterTank.CurrentTankAmount > 0f) ? sprinkler.waterTank.CurrentTankAmount : (-1f));
		RGDBattery = (sprinkler.battery.BatterySlotIsEmpty ? null : new RGD_Battery(sprinkler.battery));
		sprinklerObjectIndex = sprinkler.ObjectIndex;
	}

	public void RestoreSprinkler(Sprinkler sprinkler)
	{
		if (!(sprinkler == null))
		{
			if (RGDBattery != null)
			{
				RGDBattery.RestoreBattery(sprinkler.battery);
			}
			if (waterTankAmount > 0f)
			{
				sprinkler.waterTank.SetTankAmount(waterTankAmount);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDBattery", RGDBattery);
		info.AddValue("WaterTankAmount", waterTankAmount);
		info.AddValue("SprinklerObjectIndex", sprinklerObjectIndex);
	}

	public RGD_Sprinkler(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "BatteryUses":
				RGDBattery = new RGD_Battery(Helper.ObjToValue<int>(item.Value));
				break;
			case "WaterTankAmount":
				waterTankAmount = Helper.ObjToValue<float>(item.Value);
				break;
			case "SprinklerObjectIndex":
				sprinklerObjectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			case "RGDBattery":
				RGDBattery = (RGD_Battery)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		RGDBattery = null;
		waterTankAmount = -1f;
		sprinklerObjectIndex = 0u;
	}
}
