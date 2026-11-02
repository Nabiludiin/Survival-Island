using System;
using Unity.Netcode;

[Serializable]
public class Message_Domestic_Create : Message_NetworkBehaviour_ID_SteamID
{
	public bool isTamed;

	protected bool Equals(Message_Domestic_Create other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
		{
			return isTamed == other.isTamed;
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
		return Equals((Message_Domestic_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), isTamed);
	}

	public static bool operator ==(Message_Domestic_Create left, Message_Domestic_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Domestic_Create left, Message_Domestic_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_Domestic_Create()
	{
	}

	public Message_Domestic_Create(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, AI_NetworkBehaviour_Domestic domesticAnimal, Network_UserId steamID)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		isTamed = domesticAnimal.IsTamed;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in isTamed, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out isTamed, default(FastBufferWriter.ForPrimitives));
	}
}
