using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Scarecrow : RGD_Block
{
	public int scarecrowHealth;

	protected bool Equals(RGD_Scarecrow other)
	{
		if (Equals((RGD_Block)other))
		{
			return scarecrowHealth == other.scarecrowHealth;
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
		return Equals((RGD_Scarecrow)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), scarecrowHealth);
	}

	public static bool operator ==(RGD_Scarecrow left, RGD_Scarecrow right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Scarecrow left, RGD_Scarecrow right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in scarecrowHealth, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out scarecrowHealth, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Scarecrow()
	{
	}

	public RGD_Scarecrow(RGDType type, Block block, Scarecrow scarecrow)
		: base(type, block)
	{
		scarecrowHealth = scarecrow.Health;
	}

	public void RestoreScarecrow(Scarecrow scarecrow)
	{
		if (scarecrowHealth != -1 && !scarecrow.invurnerable)
		{
			while (scarecrow.Health > scarecrowHealth)
			{
				scarecrow.DestroyPartInstantly();
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("ScarecrowHealth", scarecrowHealth);
	}

	public RGD_Scarecrow(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "ScarecrowHealth")
			{
				scarecrowHealth = (int)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		scarecrowHealth = -1;
	}
}
