using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_NetworkBehaviour_SteamID : Message_NetworkBehaviour
{
	[SerializeField]
	private Network_UserId steamID;

	public Network_UserId SteamID => steamID;

	protected bool Equals(Message_NetworkBehaviour_SteamID other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return steamID.Equals(other.steamID);
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
		return Equals((Message_NetworkBehaviour_SteamID)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), steamID);
	}

	public static bool operator ==(Message_NetworkBehaviour_SteamID left, Message_NetworkBehaviour_SteamID right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NetworkBehaviour_SteamID left, Message_NetworkBehaviour_SteamID right)
	{
		return !object.Equals(left, right);
	}

	public Message_NetworkBehaviour_SteamID(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID)
		: base(type, behaviour)
	{
		this.steamID = steamID;
	}

	public Message_NetworkBehaviour_SteamID()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<ulong>(steamID.Id, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out ulong value, default(FastBufferWriter.ForPrimitives));
		steamID = new Network_UserId(value);
	}
}
