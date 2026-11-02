using System;
using Unity.Netcode;

[Serializable]
public class Message_AnimalCaptured : Message_NetworkBehaviour_ID
{
	public bool isAnimalCaptured;

	protected bool Equals(Message_AnimalCaptured other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return isAnimalCaptured == other.isAnimalCaptured;
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
		return Equals((Message_AnimalCaptured)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), isAnimalCaptured);
	}

	public static bool operator ==(Message_AnimalCaptured left, Message_AnimalCaptured right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_AnimalCaptured left, Message_AnimalCaptured right)
	{
		return !object.Equals(left, right);
	}

	public Message_AnimalCaptured()
	{
	}

	public Message_AnimalCaptured(Messages p_messageType, MonoBehaviour_Network p_behaviour, uint networkID_ObjectIndex, bool p_isAnimalCaptured)
		: base(p_messageType, p_behaviour, networkID_ObjectIndex)
	{
		isAnimalCaptured = p_isAnimalCaptured;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in isAnimalCaptured, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out isAnimalCaptured, default(FastBufferWriter.ForPrimitives));
	}
}
