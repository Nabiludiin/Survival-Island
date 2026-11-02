using System;
using Unity.Netcode;

[Serializable]
public class Message_NetworkBehaviour_ID_Float : Message_NetworkBehaviour_ID
{
	public float floatValue;

	protected bool Equals(Message_NetworkBehaviour_ID_Float other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return floatValue.Equals(other.floatValue);
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
		return Equals((Message_NetworkBehaviour_ID_Float)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), floatValue);
	}

	public static bool operator ==(Message_NetworkBehaviour_ID_Float left, Message_NetworkBehaviour_ID_Float right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NetworkBehaviour_ID_Float left, Message_NetworkBehaviour_ID_Float right)
	{
		return !object.Equals(left, right);
	}

	public Message_NetworkBehaviour_ID_Float()
	{
	}

	public Message_NetworkBehaviour_ID_Float(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, float floatValue)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.floatValue = floatValue;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in floatValue, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out floatValue, default(FastBufferWriter.ForPrimitives));
	}
}
