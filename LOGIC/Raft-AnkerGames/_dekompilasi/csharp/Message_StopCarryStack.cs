using System;
using Unity.Netcode;

[Serializable]
public class Message_StopCarryStack : Message_NetworkBehaviour_ID_SteamID
{
	public uint otherCarryObjectIndex;

	protected bool Equals(Message_StopCarryStack other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
		{
			return otherCarryObjectIndex == other.otherCarryObjectIndex;
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
		return Equals((Message_StopCarryStack)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), otherCarryObjectIndex);
	}

	public static bool operator ==(Message_StopCarryStack left, Message_StopCarryStack right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_StopCarryStack left, Message_StopCarryStack right)
	{
		return !object.Equals(left, right);
	}

	public Message_StopCarryStack()
	{
	}

	public Message_StopCarryStack(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Network_UserId steamID, CarryNetworked otherCarry)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		otherCarryObjectIndex = otherCarry.ObjectIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in otherCarryObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out otherCarryObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
