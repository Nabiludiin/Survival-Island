using System;
using Unity.Netcode;

[Serializable]
public class Message_CookingStand_Collect : Message_CookingStand
{
	public int slotToCollectIndex;

	protected bool Equals(Message_CookingStand_Collect other)
	{
		if (Equals((Message_CookingStand)other))
		{
			return slotToCollectIndex == other.slotToCollectIndex;
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
		return Equals((Message_CookingStand_Collect)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), slotToCollectIndex);
	}

	public static bool operator ==(Message_CookingStand_Collect left, Message_CookingStand_Collect right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_CookingStand_Collect left, Message_CookingStand_Collect right)
	{
		return !object.Equals(left, right);
	}

	public Message_CookingStand_Collect()
	{
	}

	public Message_CookingStand_Collect(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, Block_CookingStand stand, CookingSlot slotToCollect)
		: base(type, behaviour, steamID, stand)
	{
		slotToCollectIndex = stand.GetIndexOfCookingSlot(slotToCollect);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in slotToCollectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out slotToCollectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
