using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_Sprinkler : RGD_Block
{
	public RGD_Sprinkler rgdSprinkler;

	protected bool Equals(RGD_Block_Sprinkler other)
	{
		if (Equals((RGD_Block)other))
		{
			return object.Equals(rgdSprinkler, other.rgdSprinkler);
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
		return Equals((RGD_Block_Sprinkler)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdSprinkler);
	}

	public static bool operator ==(RGD_Block_Sprinkler left, RGD_Block_Sprinkler right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_Sprinkler left, RGD_Block_Sprinkler right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdSprinkler.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdSprinkler = new RGD_Sprinkler();
		rgdSprinkler.DeserializeFast(reader);
	}

	public RGD_Block_Sprinkler()
	{
	}

	public RGD_Block_Sprinkler(RGDType type, Block block, Sprinkler sprinkler)
		: base(type, block)
	{
		rgdSprinkler = new RGD_Sprinkler(sprinkler);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDSprinkler", rgdSprinkler);
	}

	public RGD_Block_Sprinkler(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RGDSprinkler")
			{
				rgdSprinkler = (RGD_Sprinkler)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdSprinkler = null;
	}
}
