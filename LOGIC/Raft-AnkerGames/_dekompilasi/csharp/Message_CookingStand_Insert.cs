using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_CookingStand_Insert : Message_CookingStand
{
	public int itemToInsertIndex;

	public int[] emptySlotIndexes;

	protected bool Equals(Message_CookingStand_Insert other)
	{
		if (emptySlotIndexes == null && other.emptySlotIndexes != null)
		{
			return false;
		}
		if (emptySlotIndexes != null && other.emptySlotIndexes == null)
		{
			return false;
		}
		if (Equals((Message_CookingStand)other) && itemToInsertIndex == other.itemToInsertIndex)
		{
			if (emptySlotIndexes != null || other.emptySlotIndexes != null)
			{
				return Enumerable.SequenceEqual(emptySlotIndexes, other.emptySlotIndexes);
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
		return Equals((Message_CookingStand_Insert)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), itemToInsertIndex, emptySlotIndexes);
	}

	public static bool operator ==(Message_CookingStand_Insert left, Message_CookingStand_Insert right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_CookingStand_Insert left, Message_CookingStand_Insert right)
	{
		return !object.Equals(left, right);
	}

	public Message_CookingStand_Insert()
	{
	}

	public Message_CookingStand_Insert(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, Block_CookingStand stand, Item_Base itemToInsert, CookingSlot[] emptySlots)
		: base(type, behaviour, steamID, stand)
	{
		itemToInsertIndex = itemToInsert.UniqueIndex;
		emptySlotIndexes = new int[emptySlots.Length];
		for (int i = 0; i < emptySlots.Length; i++)
		{
			emptySlotIndexes[i] = stand.GetIndexOfCookingSlot(emptySlots[i]);
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in itemToInsertIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<int>((emptySlotIndexes != null) ? emptySlotIndexes.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (emptySlotIndexes != null)
		{
			int[] array = emptySlotIndexes;
			for (int i = 0; i < array.Length; i++)
			{
				int value = array[i];
				writer.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out itemToInsertIndex, default(FastBufferWriter.ForPrimitives));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			emptySlotIndexes = new int[value];
			for (int i = 0; i < value; i++)
			{
				reader.ReadValue(out emptySlotIndexes[i], default(FastBufferWriter.ForPrimitives));
			}
		}
	}
}
