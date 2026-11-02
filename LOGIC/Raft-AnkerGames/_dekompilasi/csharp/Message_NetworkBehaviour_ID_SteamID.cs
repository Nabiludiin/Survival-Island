using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_NetworkBehaviour_ID_SteamID : Message_NetworkBehaviour_ID
{
	[SerializeField]
	private ulong steamID;

	public Network_UserId SteamID => steamID;

	protected bool Equals(Message_NetworkBehaviour_ID_SteamID other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return steamID == other.steamID;
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
		return Equals((Message_NetworkBehaviour_ID_SteamID)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), steamID);
	}

	public static bool operator ==(Message_NetworkBehaviour_ID_SteamID left, Message_NetworkBehaviour_ID_SteamID right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NetworkBehaviour_ID_SteamID left, Message_NetworkBehaviour_ID_SteamID right)
	{
		return !object.Equals(left, right);
	}

	public Message_NetworkBehaviour_ID_SteamID(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Network_UserId steamID)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.steamID = steamID;
	}

	public Message_NetworkBehaviour_ID_SteamID()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in steamID, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out steamID, default(FastBufferWriter.ForPrimitives));
	}
}
