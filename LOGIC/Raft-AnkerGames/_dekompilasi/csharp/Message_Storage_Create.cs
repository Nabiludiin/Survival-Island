using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Storage_Create : Message_NetworkBehaviour
{
	public List<RGD_Storage> rgdInventories = new List<RGD_Storage>();

	protected bool Equals(Message_Storage_Create other)
	{
		if (rgdInventories == null && other.rgdInventories != null)
		{
			return false;
		}
		if (rgdInventories != null && other.rgdInventories == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdInventories != null || other.rgdInventories != null)
			{
				return rgdInventories.SequenceEqual(other.rgdInventories);
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
		return Equals((Message_Storage_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdInventories);
	}

	public static bool operator ==(Message_Storage_Create left, Message_Storage_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Storage_Create left, Message_Storage_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_Storage_Create()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(rgdInventories.Count, default(FastBufferWriter.ForPrimitives));
		foreach (RGD_Storage rgdInventory in rgdInventories)
		{
			rgdInventory.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			rgdInventories = new List<RGD_Storage>(value);
			for (int i = 0; i < value; i++)
			{
				RGD_Storage rGD_Storage = new RGD_Storage();
				rGD_Storage.DeserializeFast(reader);
				rgdInventories.Add(rGD_Storage);
			}
		}
	}

	public Message_Storage_Create(Messages type, MonoBehaviour_Network behaviour)
		: base(type, behaviour)
	{
		for (int i = 0; i < StorageManager.allStorages.Count; i++)
		{
			Storage_Small storage_Small = StorageManager.allStorages[i];
			if (storage_Small != null && storage_Small.IsValidSerialization())
			{
				Inventory inventoryReference = storage_Small.GetInventoryReference();
				RGD_Storage item = new RGD_Storage(RGDType.Block_Storage, storage_Small, storage_Small.ObjectIndex, inventoryReference, storage_Small.IsOpen);
				rgdInventories.Add(item);
			}
		}
	}

	public void RestoreStorages(Message_Storage_Create msgCreate, StorageManager storageManager)
	{
		if (rgdInventories == null)
		{
			return;
		}
		for (int i = 0; i < msgCreate.rgdInventories.Count; i++)
		{
			RGD_Storage rGD_Storage = msgCreate.rgdInventories[i];
			if (rGD_Storage == null)
			{
				continue;
			}
			Storage_Small storageByObjectIndex = storageManager.GetStorageByObjectIndex(rGD_Storage.storageObjectIndex);
			if (!(storageByObjectIndex == null))
			{
				if (rGD_Storage.isOpen)
				{
					storageByObjectIndex.Open(null);
				}
				rGD_Storage.RestoreInventory(storageByObjectIndex.GetInventoryReference());
			}
		}
	}
}
