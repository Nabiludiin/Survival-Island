using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_CookingPot_Slot : SerializableBehaviour
{
	public int slotIndex;

	public RGD_ItemInstance rgdItemInstance;

	protected bool Equals(RGD_CookingPot_Slot other)
	{
		if (slotIndex == other.slotIndex)
		{
			return object.Equals(rgdItemInstance, other.rgdItemInstance);
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
		return Equals((RGD_CookingPot_Slot)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(slotIndex, rgdItemInstance);
	}

	public static bool operator ==(RGD_CookingPot_Slot left, RGD_CookingPot_Slot right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_CookingPot_Slot left, RGD_CookingPot_Slot right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in slotIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue<bool>(rgdItemInstance != null, default(FastBufferWriter.ForPrimitives));
		if (rgdItemInstance != null)
		{
			rgdItemInstance.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out slotIndex, default(FastBufferWriter.ForPrimitives));
		bool value = false;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value)
		{
			rgdItemInstance = new RGD_ItemInstance();
			rgdItemInstance.DeserializeFast(reader);
		}
	}

	public RGD_CookingPot_Slot()
	{
	}

	public RGD_CookingPot_Slot(CookingTable_Slot slot)
	{
		slotIndex = slot.Index;
		rgdItemInstance = ((slot.CurrentItem == null) ? null : new RGD_ItemInstance(slot.CurrentItem));
	}

	public void RestoreSlot(CookingTable_Slot slot)
	{
		if (rgdItemInstance != null)
		{
			slot.InsertItem(null, rgdItemInstance.GetRealInstance());
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("SlotIndex", slotIndex);
		info.AddValue("RGDItemInstance", rgdItemInstance);
	}

	public RGD_CookingPot_Slot(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "SlotIndex":
				slotIndex = (int)item.Value;
				break;
			case "RGDItemInstance":
				rgdItemInstance = Helper.ObjToValue<RGD_ItemInstance>(item.Value);
				break;
			case "ItemIndex":
			{
				int uniqueIndex = (int)item.Value;
				if (rgdItemInstance == null)
				{
					Item_Base itemByIndex = ItemManager.GetItemByIndex(uniqueIndex);
					if (itemByIndex != null)
					{
						rgdItemInstance = new RGD_ItemInstance(new ItemInstance(itemByIndex, 1, itemByIndex.MaxUses));
					}
				}
				break;
			}
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		slotIndex = -1;
		rgdItemInstance = null;
	}
}
