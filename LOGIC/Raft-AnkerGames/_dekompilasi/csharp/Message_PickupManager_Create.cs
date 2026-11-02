using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_PickupManager_Create : Message_NetworkBehaviour
{
	public List<RGD_PickupItem> pickupItems = new List<RGD_PickupItem>();

	protected bool Equals(Message_PickupManager_Create other)
	{
		if (pickupItems == null && other.pickupItems != null)
		{
			return false;
		}
		if (pickupItems != null && other.pickupItems == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (pickupItems != null || other.pickupItems != null)
			{
				return pickupItems.SequenceEqual(other.pickupItems);
			}
			return true;
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
		return Equals((Message_PickupManager_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), pickupItems);
	}

	public static bool operator ==(Message_PickupManager_Create left, Message_PickupManager_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_PickupManager_Create left, Message_PickupManager_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_PickupManager_Create()
	{
	}

	public Message_PickupManager_Create(Messages type, PickupObjectManager pickupObjectManager)
		: base(type, pickupObjectManager)
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(pickupItems.Count, default(FastBufferWriter.ForPrimitives));
		foreach (RGD_PickupItem pickupItem in pickupItems)
		{
			pickupItem.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		pickupItems = new List<RGD_PickupItem>(value);
		for (int i = 0; i < value; i++)
		{
			RGD_PickupItem rGD_PickupItem = new RGD_PickupItem();
			rGD_PickupItem.DeserializeFast(reader);
			pickupItems.Add(rGD_PickupItem);
		}
	}
}
