using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_BiofuelExtractor : RGD_Block
{
	public RGD_BiofuelExtactor rgdBioFuelCreator;

	protected bool Equals(RGD_Block_BiofuelExtractor other)
	{
		if (Equals((RGD_Block)other))
		{
			return object.Equals(rgdBioFuelCreator, other.rgdBioFuelCreator);
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
		return Equals((RGD_Block_BiofuelExtractor)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdBioFuelCreator);
	}

	public static bool operator ==(RGD_Block_BiofuelExtractor left, RGD_Block_BiofuelExtractor right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_BiofuelExtractor left, RGD_Block_BiofuelExtractor right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdBioFuelCreator.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdBioFuelCreator = new RGD_BiofuelExtactor();
		rgdBioFuelCreator.DeserializeFast(reader);
	}

	public RGD_Block_BiofuelExtractor()
	{
	}

	public RGD_Block_BiofuelExtractor(Placeable_BiofuelExtractor bioFuelCreator)
		: base(RGDType.Block_BioFuelCreator, bioFuelCreator.GetComponent<Block>())
	{
		rgdBioFuelCreator = new RGD_BiofuelExtactor(bioFuelCreator);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("BioFuelCreator", rgdBioFuelCreator);
	}

	public RGD_Block_BiofuelExtractor(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "BioFuelCreator")
			{
				rgdBioFuelCreator = Helper.ObjToValue<RGD_BiofuelExtactor>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdBioFuelCreator = null;
	}
}
