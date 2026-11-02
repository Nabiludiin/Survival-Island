using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Cookingstand_Smelter_Electric : RGD_CookingStand
{
	public RGD_Battery rgdBattery;

	protected bool Equals(RGD_Cookingstand_Smelter_Electric other)
	{
		if (Equals((RGD_CookingStand)other))
		{
			return object.Equals(rgdBattery, other.rgdBattery);
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
		return Equals((RGD_Cookingstand_Smelter_Electric)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdBattery);
	}

	public static bool operator ==(RGD_Cookingstand_Smelter_Electric left, RGD_Cookingstand_Smelter_Electric right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Cookingstand_Smelter_Electric left, RGD_Cookingstand_Smelter_Electric right)
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
		reader.ReadValue(out bool value, default(FastBufferWriter.ForPrimitives));
		if (value)
		{
			rgdBattery = new RGD_Battery();
			rgdBattery.DeserializeFast(reader);
		}
	}

	public RGD_Cookingstand_Smelter_Electric()
	{
	}

	public RGD_Cookingstand_Smelter_Electric(RGDType type, Block_CookingStand stand)
		: base(type, stand)
	{
		if (stand != null)
		{
			Battery battery = (stand as Block_CookingStand_Electric).battery;
			if (battery != null)
			{
				rgdBattery = (battery.BatterySlotIsEmpty ? null : new RGD_Battery(battery));
			}
		}
	}

	public override void RestoreStand(Block_CookingStand stand)
	{
		base.RestoreStand(stand);
		if (rgdBattery == null || !(stand != null))
		{
			return;
		}
		Block_CookingStand_Electric block_CookingStand_Electric = stand as Block_CookingStand_Electric;
		if (block_CookingStand_Electric != null && block_CookingStand_Electric.battery != null)
		{
			Battery battery = block_CookingStand_Electric.battery;
			if (battery != null)
			{
				rgdBattery.RestoreBattery(battery);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDBattery", rgdBattery);
	}

	public RGD_Cookingstand_Smelter_Electric(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RGDBattery")
			{
				rgdBattery = (RGD_Battery)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdBattery = null;
	}
}
