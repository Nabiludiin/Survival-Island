using System;
using Unity.Netcode;

[Serializable]
public class Message_Fuel : Message_NetworkBehaviour_ID_SteamID
{
	public int incrementAmount;

	protected bool Equals(Message_Fuel other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
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
		return Equals((Message_Fuel)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), incrementAmount);
	}

	public static bool operator ==(Message_Fuel left, Message_Fuel right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Fuel left, Message_Fuel right)
	{
		return !object.Equals(left, right);
	}

	public Message_Fuel()
	{
	}

	public Message_Fuel(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Network_UserId steamID, int incrementFuel)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		incrementAmount = incrementFuel;
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
