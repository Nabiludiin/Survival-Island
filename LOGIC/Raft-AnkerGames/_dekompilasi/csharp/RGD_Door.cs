using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Door : RGD_Block
{
	public bool isOpen;

	protected bool Equals(RGD_Door other)
	{
		if (Equals((RGD_Block)other))
		{
			return isOpen == other.isOpen;
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
		return Equals((RGD_Door)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), isOpen);
	}

	public static bool operator ==(RGD_Door left, RGD_Door right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Door left, RGD_Door right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in isOpen, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out isOpen, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Door()
	{
	}

	public RGD_Door(RGDType type, Block block, bool isOpen)
		: base(type, block)
	{
		this.isOpen = isOpen;
	}

	public void Restore(Door door)
	{
		if (isOpen)
		{
			door.Open();
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Open", isOpen);
	}

	public RGD_Door(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Open")
			{
				isOpen = (bool)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		isOpen = false;
	}
}
