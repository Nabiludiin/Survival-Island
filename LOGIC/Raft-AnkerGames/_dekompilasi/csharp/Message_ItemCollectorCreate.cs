using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_ItemCollectorCreate : Message_NetworkBehaviour
{
	public RGD_ItemCollector[] rgd_itemCollectors;

	protected bool Equals(Message_ItemCollectorCreate other)
	{
		if (rgd_itemCollectors == null && other.rgd_itemCollectors != null)
		{
			return false;
		}
		if (rgd_itemCollectors != null && other.rgd_itemCollectors == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgd_itemCollectors != null || other.rgd_itemCollectors != null)
			{
				return Enumerable.SequenceEqual(rgd_itemCollectors, other.rgd_itemCollectors);
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
		return Equals((Message_ItemCollectorCreate)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgd_itemCollectors);
	}

	public static bool operator ==(Message_ItemCollectorCreate left, Message_ItemCollectorCreate right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ItemCollectorCreate left, Message_ItemCollectorCreate right)
	{
		return !object.Equals(left, right);
	}

	public Message_ItemCollectorCreate(Messages type, MonoBehaviour_Network behaviour, ItemCollector[] itemCollectors)
		: base(type, behaviour)
	{
		List<RGD_ItemCollector> list = new List<RGD_ItemCollector>();
		foreach (ItemCollector itemCollector in itemCollectors)
		{
			if (!(itemCollector == null) && !itemCollector.playerControlled)
			{
				Block_Foundation_ItemNet componentInParent = itemCollector.GetComponentInParent<Block_Foundation_ItemNet>();
				if (componentInParent != null)
				{
					list.Add(new RGD_ItemCollector(RGDType.Block_ItemCollector, componentInParent, itemCollector));
				}
				else
				{
					Debug.Log("block on itemcollector parent is null");
				}
				rgd_itemCollectors = list.ToArray();
			}
		}
	}

	public void RestoreItemCollectors(ItemCollector[] itemCollectors)
	{
		if (!rgd_itemCollectors.ContainsItems())
		{
			return;
		}
		RGD_ItemCollector[] array = rgd_itemCollectors;
		foreach (RGD_ItemCollector rGD_ItemCollector in array)
		{
			if (!(rGD_ItemCollector == null))
			{
				ItemCollector itemCollectorFromObjectIndex = GetItemCollectorFromObjectIndex(itemCollectors, rGD_ItemCollector.BlockObjectIndex);
				if (itemCollectorFromObjectIndex != null)
				{
					rGD_ItemCollector.RestoreItemCollector(itemCollectorFromObjectIndex);
				}
			}
		}
	}

	private ItemCollector GetItemCollectorFromObjectIndex(ItemCollector[] itemCollectors, uint objectIndex)
	{
		for (int i = 0; i < itemCollectors.Length; i++)
		{
			if (itemCollectors[i].ObjectIndex == objectIndex)
			{
				return itemCollectors[i];
			}
		}
		return null;
	}

	public Message_ItemCollectorCreate()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgd_itemCollectors != null) ? rgd_itemCollectors.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgd_itemCollectors != null)
		{
			RGD_ItemCollector[] array = rgd_itemCollectors;
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
			rgd_itemCollectors = new RGD_ItemCollector[value];
			for (int i = 0; i < value; i++)
			{
				rgd_itemCollectors[i] = new RGD_ItemCollector();
				rgd_itemCollectors[i].DeserializeFast(reader);
			}
		}
	}
}
