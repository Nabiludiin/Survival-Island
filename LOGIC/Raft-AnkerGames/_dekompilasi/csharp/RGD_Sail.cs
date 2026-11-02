using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Sail : RGD_Block
{
	public bool isOpen;

	public float rotation;

	protected bool Equals(RGD_Sail other)
	{
		if (Equals((RGD_Block)other) && isOpen == other.isOpen)
		{
			return rotation.Equals(other.rotation);
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
		return Equals((RGD_Sail)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), isOpen, rotation);
	}

	public static bool operator ==(RGD_Sail left, RGD_Sail right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Sail left, RGD_Sail right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in isOpen, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in rotation, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out isOpen, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out rotation, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Sail()
	{
	}

	public RGD_Sail(RGDType type, Sail sail)
		: base(type, sail.GetComponent<Block>())
	{
		isOpen = sail.open;
		rotation = sail.LocalRotation;
	}

	public void RestoreSail(Sail sail)
	{
		if (isOpen)
		{
			sail.Open();
		}
		sail.SetRotation(rotation);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Open", isOpen);
		info.AddValue("Rotation", rotation);
	}

	public RGD_Sail(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "Open"))
			{
				if (name == "Rotation")
				{
					rotation = (float)item.Value;
				}
			}
			else
			{
				isOpen = (bool)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		isOpen = false;
		rotation = 0f;
	}
}
