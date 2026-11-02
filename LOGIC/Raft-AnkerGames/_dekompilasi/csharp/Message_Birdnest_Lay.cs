using System;
using Unity.Netcode;

[Serializable]
public class Message_Birdnest_Lay : Message_NetworkBehaviour
{
	public int numberOfItems;

	protected bool Equals(Message_Birdnest_Lay other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return numberOfItems == other.numberOfItems;
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
		return Equals((Message_Birdnest_Lay)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), numberOfItems);
	}

	public static bool operator ==(Message_Birdnest_Lay left, Message_Birdnest_Lay right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Birdnest_Lay left, Message_Birdnest_Lay right)
	{
		return !object.Equals(left, right);
	}

	public Message_Birdnest_Lay()
	{
	}

	public Message_Birdnest_Lay(Messages type, MonoBehaviour_Network behaviour, int numberOfItems)
		: base(type, behaviour)
	{
		this.numberOfItems = numberOfItems;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in numberOfItems, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out numberOfItems, default(FastBufferWriter.ForPrimitives));
	}
}
