using System;
using Unity.Netcode;

[Serializable]
public class Message_CookingStand_SlotComplete : Message_CookingStand
{
	public int cookingSlotIndex;

	protected bool Equals(Message_CookingStand_SlotComplete other)
	{
		if (Equals((Message_CookingStand)other))
		{
			return cookingSlotIndex == other.cookingSlotIndex;
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
		return Equals((Message_CookingStand_SlotComplete)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), cookingSlotIndex);
	}

	public static bool operator ==(Message_CookingStand_SlotComplete left, Message_CookingStand_SlotComplete right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_CookingStand_SlotComplete left, Message_CookingStand_SlotComplete right)
	{
		return !object.Equals(left, right);
	}

	public Message_CookingStand_SlotComplete()
	{
	}

	public Message_CookingStand_SlotComplete(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, Block_CookingStand stand, int cookingSlotIndex)
		: base(type, behaviour, steamID, stand)
	{
		this.cookingSlotIndex = cookingSlotIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in cookingSlotIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out cookingSlotIndex, default(FastBufferWriter.ForPrimitives));
	}
}
