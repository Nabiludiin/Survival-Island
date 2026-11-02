using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_ItemCollector : RGD_Block_Foundation
{
	public RGD_FloatingItem[] rgdcollectedItems;

	protected bool Equals(RGD_ItemCollector other)
	{
		if (rgdcollectedItems == null && other.rgdcollectedItems != null)
		{
			return false;
		}
		if (rgdcollectedItems != null && other.rgdcollectedItems == null)
		{
			return false;
		}
		if (Equals((RGD_Block_Foundation)other))
		{
			if (rgdcollectedItems != null || other.rgdcollectedItems != null)
			{
				return Enumerable.SequenceEqual(rgdcollectedItems, other.rgdcollectedItems);
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
		return Equals((RGD_ItemCollector)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdcollectedItems);
	}

	public static bool operator ==(RGD_ItemCollector left, RGD_ItemCollector right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_ItemCollector left, RGD_ItemCollector right)
	{
		return !object.Equals(left, right);
	}

	public RGD_ItemCollector(RGDType type, Block_Foundation block, ItemCollector itemCollector)
		: base(type, block)
	{
		if (itemCollector == null)
		{
			Debug.Log("Item collector is null in rgd itemcollector");
			return;
		}
		if (itemCollector.collectedItems == null || itemCollector.collectedItems.Count == 0)
		{
			rgdcollectedItems = null;
			return;
		}
		List<RGD_FloatingItem> list = new List<RGD_FloatingItem>();
		for (int i = 0; i < itemCollector.collectedItems.Count; i++)
		{
			PickupItem_Networked pickupItem_Networked = itemCollector.collectedItems[i];
			if (!(pickupItem_Networked == null))
			{
				list.Add(new RGD_FloatingItem(pickupItem_Networked));
			}
		}
		rgdcollectedItems = list.ToArray();
	}

	public void RestoreItemCollector(ItemCollector itemCollector)
	{
		if (rgdcollectedItems == null)
		{
			return;
		}
		ObjectSpawnerManager value = ComponentManager<ObjectSpawnerManager>.Value;
		RGD_FloatingItem[] array = rgdcollectedItems;
		foreach (RGD_FloatingItem rgdItem in array)
		{
			ObjectSpawner objectSpawner = value.spawners.Find((ObjectSpawner s) => s.UniqueIndex == rgdItem.spawnerIndex);
			if (objectSpawner == null)
			{
				continue;
			}
			SpawnableFloatingObject spawnableFromIndex = objectSpawner.GetSpawnableFromIndex(rgdItem.uniqueSpawnableIndex);
			if (spawnableFromIndex != null)
			{
				PickupItem_Networked pickupItem_Networked = objectSpawner.SpawnItem(rgdItem.Position.Value, rgdItem.Rotation.Value, spawnableFromIndex, SaveAndLoad.GetUniqueObjectIndex(), NetworkUpdateManager.GetUniqueBehaviourIndex(), rgdItem.poolDuplicateIndex);
				if (pickupItem_Networked != null)
				{
					itemCollector.CollectItem(pickupItem_Networked, playSound: false);
				}
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CollectedFloatingItems", rgdcollectedItems);
	}

	public RGD_ItemCollector(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "CollectedFloatingItems")
			{
				rgdcollectedItems = (RGD_FloatingItem[])item.Value;
			}
		}
	}

	public RGD_ItemCollector()
	{
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdcollectedItems = null;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdcollectedItems != null) ? rgdcollectedItems.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdcollectedItems != null)
		{
			RGD_FloatingItem[] array = rgdcollectedItems;
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
			rgdcollectedItems = new RGD_FloatingItem[value];
			for (int i = 0; i < value; i++)
			{
				rgdcollectedItems[i] = new RGD_FloatingItem();
				rgdcollectedItems[i].DeserializeFast(reader);
			}
		}
	}
}
