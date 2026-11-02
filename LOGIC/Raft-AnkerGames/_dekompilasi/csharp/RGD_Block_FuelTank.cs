using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_FuelTank : RGD_Block
{
	public RGD_Tank rgdTank;

	protected bool Equals(RGD_Block_FuelTank other)
	{
		if (Equals((RGD_Block)other))
		{
			return object.Equals(rgdTank, other.rgdTank);
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
		return Equals((RGD_Block_FuelTank)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdTank);
	}

	public static bool operator ==(RGD_Block_FuelTank left, RGD_Block_FuelTank right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_FuelTank left, RGD_Block_FuelTank right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdTank.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdTank = new RGD_Tank();
		rgdTank.DeserializeFast(reader);
	}

	public RGD_Block_FuelTank()
	{
	}

	public RGD_Block_FuelTank(Placeable_Tank fuelTank)
		: base(RGDType.Block_FuelTank, fuelTank.GetComponent<Block>())
	{
		rgdTank = new RGD_Tank(fuelTank.tank);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Tank", rgdTank);
	}

	public override void RestoreBlock(Block block)
	{
		base.RestoreBlock(block);
		if (block != null)
		{
			Placeable_Tank component = block.GetComponent<Placeable_Tank>();
			if (component != null)
			{
				component.tank.SetTankAmount(rgdTank.tankValue);
			}
		}
	}

	public RGD_Block_FuelTank(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Tank")
			{
				rgdTank = Helper.ObjToValue<RGD_Tank>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdTank = null;
	}
}
