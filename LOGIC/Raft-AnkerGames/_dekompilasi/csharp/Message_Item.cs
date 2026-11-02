using System;
using Unity.Netcode;

[Serializable]
public class Message_Item : Message_NetworkBehaviour
{
	public int itemIndex;

	protected bool Equals(Message_Item other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return itemIndex == other.itemIndex;
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
		return Equals((Message_Item)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), itemIndex);
	}

	public static bool operator ==(Message_Item left, Message_Item right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Item left, Message_Item right)
	{
		return !object.Equals(left, right);
	}

	public Message_Item()
	{
	}

	public Message_Item(Messages type, MonoBehaviour_Network behaviour, Item_Base item)
		: base(type, behaviour)
	{
		itemIndex = item.UniqueIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in itemIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out itemIndex, default(FastBufferWriter.ForPrimitives));
	}
}
