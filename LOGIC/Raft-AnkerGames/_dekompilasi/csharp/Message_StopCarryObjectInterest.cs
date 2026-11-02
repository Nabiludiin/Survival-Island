using System;
using Unity.Netcode;

[Serializable]
public class Message_StopCarryObjectInterest : Message_NetworkBehaviour_ID_SteamID
{
	public ObjectOfInterestType interestType;

	protected bool Equals(Message_StopCarryObjectInterest other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
		{
			return interestType == other.interestType;
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
		return Equals((Message_StopCarryObjectInterest)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), (int)interestType);
	}

	public static bool operator ==(Message_StopCarryObjectInterest left, Message_StopCarryObjectInterest right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_StopCarryObjectInterest left, Message_StopCarryObjectInterest right)
	{
		return !object.Equals(left, right);
	}

	public Message_StopCarryObjectInterest()
	{
	}

	public Message_StopCarryObjectInterest(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Network_UserId steamID, ObjectOfInterestType interestType)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		this.interestType = interestType;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in interestType, default(FastBufferWriter.ForEnums));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out interestType, default(FastBufferWriter.ForEnums));
	}
}
