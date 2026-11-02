using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_BatteryCharger : RGD_Block
{
	public RGD_BatteryCharger rgdBatteryCharger;

	protected bool Equals(RGD_Block_BatteryCharger other)
	{
		if (Equals((RGD_Block)other))
		{
			return object.Equals(rgdBatteryCharger, other.rgdBatteryCharger);
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
		return Equals((RGD_Block_BatteryCharger)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdBatteryCharger);
	}

	public static bool operator ==(RGD_Block_BatteryCharger left, RGD_Block_BatteryCharger right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_BatteryCharger left, RGD_Block_BatteryCharger right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdBatteryCharger.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdBatteryCharger = new RGD_BatteryCharger();
		rgdBatteryCharger.DeserializeFast(reader);
	}

	public RGD_Block_BatteryCharger()
	{
	}

	public RGD_Block_BatteryCharger(RGDType type, Block block, BatteryCharger batteryCharger)
		: base(type, block)
	{
		rgdBatteryCharger = new RGD_BatteryCharger(batteryCharger);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDBatteryCharger", rgdBatteryCharger);
	}

	public RGD_Block_BatteryCharger(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RGDBatteryCharger")
			{
				rgdBatteryCharger = (RGD_BatteryCharger)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdBatteryCharger = null;
	}
}
