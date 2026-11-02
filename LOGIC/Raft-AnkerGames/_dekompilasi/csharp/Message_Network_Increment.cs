using System;
using Unity.Netcode;

[Serializable]
public class Message_Network_Increment : Message_NetworkBehaviour_ID
{
	public float amount;

	protected bool Equals(Message_Network_Increment other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return amount.Equals(other.amount);
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
		return Equals((Message_Network_Increment)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), amount);
	}

	public static bool operator ==(Message_Network_Increment left, Message_Network_Increment right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Network_Increment left, Message_Network_Increment right)
	{
		return !object.Equals(left, right);
	}

	public Message_Network_Increment()
	{
	}

	public Message_Network_Increment(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, float amount)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.amount = amount;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in amount, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out amount, default(FastBufferWriter.ForPrimitives));
	}
}
