using System;
using Unity.Netcode;

[Serializable]
public class Message_ItemCollectorCollect : Message_NetworkBehaviour_ID
{
	public uint pickupNetworkObjectIndex;

	protected bool Equals(Message_ItemCollectorCollect other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return pickupNetworkObjectIndex == other.pickupNetworkObjectIndex;
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
		return Equals((Message_ItemCollectorCollect)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), pickupNetworkObjectIndex);
	}

	public static bool operator ==(Message_ItemCollectorCollect left, Message_ItemCollectorCollect right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ItemCollectorCollect left, Message_ItemCollectorCollect right)
	{
		return !object.Equals(left, right);
	}

	public Message_ItemCollectorCollect(Messages type, MonoBehaviour_Network behaviour, PickupItem_Networked pickupNetworked, uint objectIndex)
		: base(type, behaviour, objectIndex)
	{
		pickupNetworkObjectIndex = pickupNetworked.ObjectIndex;
	}

	public Message_ItemCollectorCollect()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in pickupNetworkObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out pickupNetworkObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
