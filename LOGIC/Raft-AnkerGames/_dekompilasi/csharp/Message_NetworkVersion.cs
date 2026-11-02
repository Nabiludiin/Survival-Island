using System;
using Unity.Netcode;

[Serializable]
public class Message_NetworkVersion : Message
{
	public int version;

	protected bool Equals(Message_NetworkVersion other)
	{
		if (Equals((Message)other))
		{
			return version == other.version;
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
		return Equals((Message_NetworkVersion)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), version);
	}

	public static bool operator ==(Message_NetworkVersion left, Message_NetworkVersion right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NetworkVersion left, Message_NetworkVersion right)
	{
		return !object.Equals(left, right);
	}

	public Message_NetworkVersion(Messages type, int version)
	{
		Type = type;
		this.version = version;
	}

	public Message_NetworkVersion()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in version, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out version, default(FastBufferWriter.ForPrimitives));
	}
}
