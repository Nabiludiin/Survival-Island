using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_CookingTable_Juicer : RGD_CookingPot
{
	public RGD_Battery rgdBattery;

	protected bool Equals(RGD_CookingTable_Juicer other)
	{
		return object.Equals(rgdBattery, other.rgdBattery);
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
		return Equals((RGD_CookingTable_Juicer)obj);
	}

	public override int GetHashCode()
	{
		if (!(rgdBattery != null))
		{
			return 0;
		}
		return rgdBattery.GetHashCode();
	}

	public static bool operator ==(RGD_CookingTable_Juicer left, RGD_CookingTable_Juicer right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_CookingTable_Juicer left, RGD_CookingTable_Juicer right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<bool>(rgdBattery != null, default(FastBufferWriter.ForPrimitives));
		if (rgdBattery != null)
		{
			rgdBattery.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		bool value = false;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value)
		{
			rgdBattery = new RGD_Battery();
			rgdBattery.DeserializeFast(reader);
		}
	}

	public RGD_CookingTable_Juicer()
	{
	}

	public RGD_CookingTable_Juicer(CookingTable_Juicer cookingJuicer)
		: base(cookingJuicer)
	{
		if (cookingJuicer != null && cookingJuicer.battery != null && !cookingJuicer.battery.BatterySlotIsEmpty)
		{
			rgdBattery = new RGD_Battery(cookingJuicer.battery);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDBattery", rgdBattery);
	}

	public RGD_CookingTable_Juicer(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "Battery"))
			{
				if (name == "RGDBattery")
				{
					rgdBattery = (RGD_Battery)item.Value;
				}
			}
			else
			{
				rgdBattery = new RGD_Battery(Helper.ObjToValue<int>(item.Value));
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdBattery = null;
	}
}
