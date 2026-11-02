using System;
using Unity.Netcode;

[Serializable]
public class Message_SelectItem : Message_NetworkBehaviour
{
	public int itemIndex;

	protected bool Equals(Message_SelectItem other)
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
		return Equals((Message_SelectItem)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), itemIndex);
	}

	public static bool operator ==(Message_SelectItem left, Message_SelectItem right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_SelectItem left, Message_SelectItem right)
	{
		return !object.Equals(left, right);
	}

	public Message_SelectItem(Messages type, MonoBehaviour_Network behaviour, int itemIndex)
		: base(type, behaviour)
	{
		this.itemIndex = itemIndex;
	}

	public Message_SelectItem()
	{
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
