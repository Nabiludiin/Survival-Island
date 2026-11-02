using System;
using Unity.Netcode;

[Serializable]
public class Message_NetworkBehaviour_ID : Message_NetworkBehaviour
{
	public uint n;

	public uint NetworkID_ObjectIndex
	{
		get
		{
			return n;
		}
		set
		{
			n = value;
		}
	}

	protected bool Equals(Message_NetworkBehaviour_ID other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return n == other.n;
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
		return Equals((Message_NetworkBehaviour_ID)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), n);
	}

	public static bool operator ==(Message_NetworkBehaviour_ID left, Message_NetworkBehaviour_ID right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NetworkBehaviour_ID left, Message_NetworkBehaviour_ID right)
	{
		return !object.Equals(left, right);
	}

	public Message_NetworkBehaviour_ID(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex)
		: base(type, behaviour)
	{
		NetworkID_ObjectIndex = networkID_ObjectIndex;
	}

	public Message_NetworkBehaviour_ID()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in n, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out n, default(FastBufferWriter.ForPrimitives));
	}
}
