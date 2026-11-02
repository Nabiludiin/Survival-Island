using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_BeeHive : RGD_Block
{
	public RGD_BeeHive rgdBeeHive;

	public int productionTimer;

	protected bool Equals(RGD_Block_BeeHive other)
	{
		if (Equals((RGD_Block)other) && object.Equals(rgdBeeHive, other.rgdBeeHive))
		{
			return productionTimer == other.productionTimer;
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
		return Equals((RGD_Block_BeeHive)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdBeeHive, productionTimer);
	}

	public static bool operator ==(RGD_Block_BeeHive left, RGD_Block_BeeHive right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_BeeHive left, RGD_Block_BeeHive right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdBeeHive.SerializeFast(writer);
		writer.WriteValueSafe(in productionTimer, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdBeeHive = new RGD_BeeHive();
		rgdBeeHive.DeserializeFast(reader);
		reader.ReadValue(out productionTimer, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Block_BeeHive()
	{
	}

	public RGD_Block_BeeHive(RGDType type, Block block, BeeHive beeHive)
		: base(type, block)
	{
		rgdBeeHive = new RGD_BeeHive(beeHive);
		productionTimer = (int)beeHive.productionTimer.GetProgress();
	}

	public void RestoreBeehive(BeeHive beehive)
	{
		beehive.productionTimer.SetProgress(productionTimer);
		rgdBeeHive.RestoreBeeHive(beehive);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDBeeHive", rgdBeeHive);
		info.AddValue("ProductionTimer", productionTimer);
	}

	public RGD_Block_BeeHive(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "RGDBeeHive"))
			{
				if (name == "ProductionTimer")
				{
					productionTimer = Helper.ObjToValue<int>(item.Value);
				}
			}
			else
			{
				rgdBeeHive = (RGD_BeeHive)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdBeeHive = null;
		productionTimer = 0;
	}
}
