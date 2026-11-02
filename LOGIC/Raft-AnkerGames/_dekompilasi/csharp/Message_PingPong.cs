using System;
using Unity.Netcode;

[Serializable]
public class Message_PingPong : Message
{
	public float timeSent;

	protected bool Equals(Message_PingPong other)
	{
		if (Equals((Message)other))
		{
			return timeSent.Equals(other.timeSent);
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
		return Equals((Message_PingPong)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), timeSent);
	}

	public static bool operator ==(Message_PingPong left, Message_PingPong right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_PingPong left, Message_PingPong right)
	{
		return !object.Equals(left, right);
	}

	public Message_PingPong(Messages type, float timeSent = 0f)
	{
		Type = type;
		this.timeSent = timeSent;
	}

	public Message_PingPong()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in timeSent, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out timeSent, default(FastBufferWriter.ForPrimitives));
	}
}
