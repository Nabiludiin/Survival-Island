using System;
using Unity.Netcode;

[Serializable]
public class Message
{
	public short t;

	public Messages Type
	{
		get
		{
			return (Messages)t;
		}
		set
		{
			t = (short)value;
		}
	}

	protected bool Equals(Message other)
	{
		return t == other.t;
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
		return Equals((Message)obj);
	}

	public override int GetHashCode()
	{
		return t.GetHashCode();
	}

	public static bool operator ==(Message left, Message right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message left, Message right)
	{
		return !object.Equals(left, right);
	}

	public Message()
	{
	}

	public Message(Messages type)
	{
		Type = type;
	}

	public virtual void SerializeFast(FastBufferWriter writer)
	{
		writer.WriteValueSafe(in t, default(FastBufferWriter.ForPrimitives));
	}

	public virtual void DeserializeFast(FastBufferReader reader)
	{
	}
}
