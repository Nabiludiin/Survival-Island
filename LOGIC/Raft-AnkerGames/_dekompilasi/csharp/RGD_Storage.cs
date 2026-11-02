using System;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Storage : RGD_Block
{
	public RGD_Slot[] slots;

	public bool isOpen;

	public uint storageObjectIndex;

	protected bool Equals(RGD_Storage other)
	{
		if (slots == null && other.slots != null)
		{
			return false;
		}
		if (slots != null && other.slots == null)
		{
			return false;
		}
		if (Equals((RGD_Block)other) && ((slots == null && other.slots == null) || Enumerable.SequenceEqual(slots, other.slots)) && isOpen == other.isOpen)
		{
			return storageObjectIndex == other.storageObjectIndex;
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
		return Equals((RGD_Storage)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), slots, isOpen, storageObjectIndex);
	}

	public static bool operator ==(RGD_Storage left, RGD_Storage right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Storage left, RGD_Storage right)
	{
		return !object.Equals(left, right);
	}

	public RGD_Storage()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((slots != null) ? slots.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (slots != null)
		{
			RGD_Slot[] array = slots;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SerializeFast(writer);
			}
		}
		writer.WriteValueSafe(in isOpen, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in storageObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			slots = new RGD_Slot[value];
			for (int i = 0; i < value; i++)
			{
				slots[i] = new RGD_Slot();
				slots[i].DeserializeFast(reader);
			}
		}
		reader.ReadValue(out isOpen, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out storageObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Storage(RGDType type, Block block, uint storageObjectIndex, Inventory inventory, bool isOpen)
		: base(type, block)
	{
		this.storageObjectIndex = storageObjectIndex;
		this.isOpen = isOpen;
		slots = inventory.GetRGDSlots();
	}

	public void RestoreInventory(Inventory inventory)
	{
		if (!(inventory == null) && slots != null)
		{
			inventory.SetSlotsFromRGD(slots);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Slots", slots);
		info.AddValue("IsOpen", isOpen);
		info.AddValue("ObjectIndex", storageObjectIndex);
	}

	public RGD_Storage(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Slots":
				slots = item.Value as RGD_Slot[];
				break;
			case "IsOpen":
				isOpen = Convert.ToBoolean(item.Value);
				break;
			case "ObjectIndex":
				storageObjectIndex = Convert.ToUInt32(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		slots = null;
		isOpen = false;
		storageObjectIndex = 0u;
	}
}
