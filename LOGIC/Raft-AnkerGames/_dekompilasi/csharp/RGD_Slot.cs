using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Slot : SerializableBehaviour
{
	public short slotIndex;

	public short itemIndex;

	public short itemAmount;

	public short itemUses;

	public string exclusiveString;

	public bool HasItem => itemIndex != short.MaxValue;

	public ItemInstance ItemInstance
	{
		get
		{
			if (HasItem)
			{
				Item_Base itemByIndex = ItemManager.GetItemByIndex(itemIndex);
				if (itemByIndex == null)
				{
					return null;
				}
				return new ItemInstance(itemByIndex, itemAmount, itemUses)
				{
					exclusiveString = exclusiveString
				};
			}
			return null;
		}
	}

	protected bool Equals(RGD_Slot other)
	{
		if (slotIndex == other.slotIndex && itemIndex == other.itemIndex && itemAmount == other.itemAmount && itemUses == other.itemUses)
		{
			return exclusiveString == other.exclusiveString;
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
		return Equals((RGD_Slot)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(slotIndex, itemIndex, itemAmount, itemUses, exclusiveString);
	}

	public static bool operator ==(RGD_Slot left, RGD_Slot right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Slot left, RGD_Slot right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(8))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in slotIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in itemIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in itemAmount, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in itemUses, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<bool>(exclusiveString != null, default(FastBufferWriter.ForPrimitives));
		if (exclusiveString != null)
		{
			writer.WriteValueSafe(exclusiveString);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out slotIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out itemIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out itemAmount, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out itemUses, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out bool value, default(FastBufferWriter.ForPrimitives));
		if (value)
		{
			reader.ReadValue(out exclusiveString, false);
		}
	}

	public RGD_Slot()
	{
	}

	public RGD_Slot(Slot slot, int slotIndex)
	{
		this.slotIndex = (short)slotIndex;
		if (slot != null && !slot.IsEmpty)
		{
			ItemInstance itemInstance = slot.itemInstance;
			if (itemInstance != null)
			{
				itemIndex = (short)itemInstance.UniqueIndex;
				itemAmount = (short)itemInstance.Amount;
				itemUses = (short)itemInstance.Uses;
				exclusiveString = itemInstance.exclusiveString;
			}
		}
	}

	public void RestoreSlot(Slot slot)
	{
		if (slot == null)
		{
			Debug.Log("Can't restore slot, slot is null");
		}
		else if (HasItem)
		{
			slot.SetItem(ItemInstance);
		}
		else
		{
			slot.SetItem(null);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("SlotIndex", slotIndex);
		info.AddValue("ItemIndex", itemIndex);
		info.AddValue("ItemAmount", itemAmount);
		info.AddValue("ItemUses", itemUses);
		info.AddValue("ExclusiveString", exclusiveString);
	}

	public RGD_Slot(SerializationInfo info, StreamingContext sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "SlotIndex":
				slotIndex = Helper.ObjToValue<short>(item.Value);
				break;
			case "ItemIndex":
				itemIndex = Helper.ObjToValue<short>(item.Value);
				break;
			case "ItemAmount":
				itemAmount = Helper.ObjToValue<short>(item.Value);
				break;
			case "ItemUses":
				itemUses = Helper.ObjToValue<short>(item.Value);
				break;
			case "ExclusiveString":
				exclusiveString = Helper.ObjToValue<string>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		slotIndex = short.MaxValue;
		itemIndex = short.MaxValue;
		itemAmount = 0;
		itemUses = 0;
		exclusiveString = "NONE";
	}
}
