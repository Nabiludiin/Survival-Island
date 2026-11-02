using System;
using Unity.Netcode;

[Serializable]
public class Message_Consume : Message_NetworkBehaviour
{
	public int edibleItemIndex;

	protected bool Equals(Message_Consume other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return edibleItemIndex == other.edibleItemIndex;
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
		return Equals((Message_Consume)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), edibleItemIndex);
	}

	public static bool operator ==(Message_Consume left, Message_Consume right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Consume left, Message_Consume right)
	{
		return !object.Equals(left, right);
	}

	public Message_Consume()
	{
	}

	public Message_Consume(Messages type, MonoBehaviour_Network behaviour, Item_Base edible)
		: base(type, behaviour)
	{
		edibleItemIndex = edible.UniqueIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in edibleItemIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out edibleItemIndex, default(FastBufferWriter.ForPrimitives));
	}
}
