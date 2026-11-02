using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_Radio : RGD_Block
{
	public RGD_RadioPlayer rgdRadio;

	protected bool Equals(RGD_Block_Radio other)
	{
		if (Equals((RGD_Block)other))
		{
			return object.Equals(rgdRadio, other.rgdRadio);
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
		return Equals((RGD_Block_Radio)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdRadio);
	}

	public static bool operator ==(RGD_Block_Radio left, RGD_Block_Radio right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_Radio left, RGD_Block_Radio right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdRadio.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdRadio = new RGD_RadioPlayer();
		rgdRadio.DeserializeFast(reader);
	}

	public RGD_Block_Radio()
	{
	}

	public RGD_Block_Radio(RGDType type, Block block, RadioPlayer radio)
		: base(type, block)
	{
		rgdRadio = radio.GetRGDRadioPlayer();
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDRadio", rgdRadio);
	}

	public RGD_Block_Radio(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RGDRadio")
			{
				rgdRadio = (RGD_RadioPlayer)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdRadio = null;
	}
}
