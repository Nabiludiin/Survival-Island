using System;
using Unity.Netcode;

[Serializable]
public class Message_NetworkBehaviour : Message
{
	public uint o;

	public uint b;

	public uint ObjectIndex
	{
		get
		{
			return o;
		}
		set
		{
			o = value;
		}
	}

	public uint BehaviourIndex
	{
		get
		{
			return b;
		}
		set
		{
			b = value;
		}
	}

	protected bool Equals(Message_NetworkBehaviour other)
	{
		if (Equals((Message)other) && o == other.o)
		{
			return b == other.b;
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
		return Equals((Message_NetworkBehaviour)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), o, b);
	}

	public static bool operator ==(Message_NetworkBehaviour left, Message_NetworkBehaviour right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NetworkBehaviour left, Message_NetworkBehaviour right)
	{
		return !object.Equals(left, right);
	}

	public Message_NetworkBehaviour(Messages type, MonoBehaviour_Network behaviour)
		: base(type)
	{
		ObjectIndex = behaviour.ObjectIndex;
		BehaviourIndex = behaviour.BehaviourIndex;
	}

	public Message_NetworkBehaviour()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in o, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in b, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out o, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out b, default(FastBufferWriter.ForPrimitives));
	}
}
