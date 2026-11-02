using System;
using Unity.Netcode;

[Serializable]
public class Message_Equipment : Message_NetworkBehaviour_SteamID
{
	public int equipmentIndex;

	protected bool Equals(Message_Equipment other)
	{
		if (Equals((Message_NetworkBehaviour_SteamID)other))
		{
			return equipmentIndex == other.equipmentIndex;
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
		return Equals((Message_Equipment)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), equipmentIndex);
	}

	public static bool operator ==(Message_Equipment left, Message_Equipment right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Equipment left, Message_Equipment right)
	{
		return !object.Equals(left, right);
	}

	public Message_Equipment()
	{
	}

	public Message_Equipment(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, int equipmentIndex)
		: base(type, behaviour, steamID)
	{
		this.equipmentIndex = equipmentIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in equipmentIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out equipmentIndex, default(FastBufferWriter.ForPrimitives));
	}
}
