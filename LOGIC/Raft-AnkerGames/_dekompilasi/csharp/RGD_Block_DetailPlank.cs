using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_DetailPlank : RGD_Block
{
	public float length;

	protected bool Equals(RGD_Block_DetailPlank other)
	{
		if (Equals((RGD_Block)other))
		{
			return length.Equals(other.length);
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
		return Equals((RGD_Block_DetailPlank)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), length);
	}

	public static bool operator ==(RGD_Block_DetailPlank left, RGD_Block_DetailPlank right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_DetailPlank left, RGD_Block_DetailPlank right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in length, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValueSafe(out length, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Block_DetailPlank()
	{
	}

	public RGD_Block_DetailPlank(RGDType type, Block_DetailPlank detailPlank)
		: base(type, detailPlank)
	{
		length = detailPlank.lengthSetter.GetLength();
	}

	public override void RestoreBlock(Block block)
	{
		base.RestoreBlock(block);
		(block as Block_DetailPlank).lengthSetter.SetLength(length);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Length", length);
	}

	public RGD_Block_DetailPlank(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Length")
			{
				length = Helper.ObjToValue<float>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		length = 1f;
	}
}
