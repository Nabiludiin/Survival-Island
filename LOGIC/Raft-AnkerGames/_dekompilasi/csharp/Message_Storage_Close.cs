using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Storage_Close : Message_Storage
{
	public RGD_Slot[] slots;

	protected bool Equals(Message_Storage_Close other)
	{
		if (slots == null && other.slots != null)
		{
			return false;
		}
		if (slots != null && other.slots == null)
		{
			return false;
		}
		if (Equals((Message_Storage)other))
		{
			if (slots != null || other.slots != null)
			{
				return Enumerable.SequenceEqual(slots, other.slots);
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
		return Equals((Message_Storage_Close)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), slots);
	}

	public static bool operator ==(Message_Storage_Close left, Message_Storage_Close right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Storage_Close left, Message_Storage_Close right)
	{
		return !object.Equals(left, right);
	}

	public Message_Storage_Close()
	{
	}

	public Message_Storage_Close(Messages type, MonoBehaviour_Network behaviour, Storage_Small storage)
		: base(type, behaviour, storage)
	{
		slots = storage.GetInventoryReference().GetRGDSlots();
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
	}
}
