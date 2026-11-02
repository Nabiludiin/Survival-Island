using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_ElectricPurifier : RGD_Block
{
	public RGD_Battery rgdBattery;

	public float tankAmount;

	public uint objectIndex;

	protected bool Equals(RGD_Block_ElectricPurifier other)
	{
		if (Equals((RGD_Block)other) && object.Equals(rgdBattery, other.rgdBattery) && tankAmount.Equals(other.tankAmount))
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
		return Equals((RGD_Block_ElectricPurifier)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdBattery, tankAmount, objectIndex);
	}

	public static bool operator ==(RGD_Block_ElectricPurifier left, RGD_Block_ElectricPurifier right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_ElectricPurifier left, RGD_Block_ElectricPurifier right)
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
			rgdBattery = new RGD_Battery();
			rgdBattery.DeserializeFast(reader);
		}
		reader.ReadValue(out tankAmount, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out objectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Block_ElectricPurifier()
	{
	}

	public RGD_Block_ElectricPurifier(Electric_Purifier purifier)
		: base(RGDType.Block_Electric_Purifier, purifier.GetComponent<Block_ElectricPurifier>())
	{
		tankAmount = ((purifier.tank.CurrentTankAmount > 0f) ? purifier.tank.CurrentTankAmount : (-1f));
		rgdBattery = (purifier.battery.BatterySlotIsEmpty ? null : new RGD_Battery(purifier.battery));
		objectIndex = purifier.ObjectIndex;
	}

	public override void RestoreBlock(Block block)
	{
		base.RestoreBlock(block);
		Electric_Purifier component = block.GetComponent<Electric_Purifier>();
		if (!(component == null))
		{
			if (rgdBattery != null)
			{
				rgdBattery.RestoreBattery(component.battery);
			}
			if (tankAmount > 0f)
			{
				component.tank.SetTankAmount(tankAmount);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDBattery", rgdBattery);
		info.AddValue("TankAmount", tankAmount);
		info.AddValue("ObjectIndex", objectIndex);
	}

	public RGD_Block_ElectricPurifier(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "BatteryUses":
				rgdBattery = new RGD_Battery(Helper.ObjToValue<int>(item.Value));
				break;
			case "TankAmount":
				tankAmount = Helper.ObjToValue<float>(item.Value);
				break;
			case "ObjectIndex":
				objectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			case "RGDBattery":
				rgdBattery = (RGD_Battery)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdBattery = null;
		tankAmount = -1f;
		objectIndex = 0u;
	}
}
