using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_SpawnerManager_Create : Message_NetworkBehaviour
{
	public RGD_FloatingItem[] floatingItems;

	protected bool Equals(Message_SpawnerManager_Create other)
	{
		if (floatingItems == null && other.floatingItems != null)
		{
			return false;
		}
		if (floatingItems != null && other.floatingItems == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (floatingItems != null || other.floatingItems != null)
			{
				return Enumerable.SequenceEqual(floatingItems, other.floatingItems);
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
		return Equals((Message_SpawnerManager_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), floatingItems);
	}

	public static bool operator ==(Message_SpawnerManager_Create left, Message_SpawnerManager_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_SpawnerManager_Create left, Message_SpawnerManager_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_SpawnerManager_Create(Messages type, ObjectSpawnerManager manager)
		: base(type, manager)
	{
		List<RGD_FloatingItem> list = new List<RGD_FloatingItem>();
		foreach (ObjectSpawner spawner in manager.spawners)
		{
			foreach (PickupItem_Networked spawnedObject in spawner.spawnedObjects)
			{
				if (!(spawnedObject == null))
				{
					RGD_FloatingItem item = new RGD_FloatingItem(spawnedObject);
					list.Add(item);
				}
			}
		}
		floatingItems = list.ToArray();
	}

	public Message_SpawnerManager_Create()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((floatingItems != null) ? floatingItems.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (floatingItems != null)
		{
			RGD_FloatingItem[] array = floatingItems;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SerializeFast(writer);
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			floatingItems = new RGD_FloatingItem[value];
			for (int i = 0; i < value; i++)
			{
				floatingItems[i] = new RGD_FloatingItem();
				floatingItems[i].DeserializeFast(reader);
			}
		}
	}
}
