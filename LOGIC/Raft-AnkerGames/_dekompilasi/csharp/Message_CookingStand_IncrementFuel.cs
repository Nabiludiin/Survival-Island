using System;
using Unity.Netcode;

[Serializable]
public class Message_CookingStand_IncrementFuel : Message_CookingStand
{
	public int incrementAmount;

	protected bool Equals(Message_CookingStand_IncrementFuel other)
	{
		if (Equals((Message_CookingStand)other))
		{
			return incrementAmount == other.incrementAmount;
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
		return Equals((Message_CookingStand_IncrementFuel)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), incrementAmount);
	}

	public static bool operator ==(Message_CookingStand_IncrementFuel left, Message_CookingStand_IncrementFuel right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_CookingStand_IncrementFuel left, Message_CookingStand_IncrementFuel right)
	{
		return !object.Equals(left, right);
	}

	public Message_CookingStand_IncrementFuel()
	{
	}

	public Message_CookingStand_IncrementFuel(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, Block_CookingStand stand, int incrementAmount)
		: base(type, behaviour, steamID, stand)
	{
		this.incrementAmount = incrementAmount;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in incrementAmount, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out incrementAmount, default(FastBufferWriter.ForPrimitives));
	}
}
