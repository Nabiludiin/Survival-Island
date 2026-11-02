using System;
using Unity.Netcode;

[Serializable]
public class Message_PickupObjectManager_RemoveItem : Message_NetworkBehaviour_SteamID
{
	public uint itemObjectIndex;

	protected bool Equals(Message_PickupObjectManager_RemoveItem other)
	{
		if (Equals((Message_NetworkBehaviour_SteamID)other))
		{
			return itemObjectIndex == other.itemObjectIndex;
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
		return Equals((Message_PickupObjectManager_RemoveItem)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), itemObjectIndex);
	}

	public static bool operator ==(Message_PickupObjectManager_RemoveItem left, Message_PickupObjectManager_RemoveItem right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_PickupObjectManager_RemoveItem left, Message_PickupObjectManager_RemoveItem right)
	{
		return !object.Equals(left, right);
	}

	public Message_PickupObjectManager_RemoveItem(Messages type, MonoBehaviour_Network behaviour, uint itemObjectIndex)
		: base(type, behaviour, 0uL)
	{
		this.itemObjectIndex = itemObjectIndex;
	}

	public Message_PickupObjectManager_RemoveItem(Messages type, MonoBehaviour_Network behaviour, Network_UserId pickupPlayerID, uint itemObjectIndex)
		: base(type, behaviour, pickupPlayerID)
	{
		this.itemObjectIndex = itemObjectIndex;
	}

	public Message_PickupObjectManager_RemoveItem()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in itemObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out itemObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
