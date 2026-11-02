using System;
using Unity.Netcode;

[Serializable]
public class Message_TradingPost : Message_NetworkBehaviour_ID_SteamID
{
	public int value = -1;

	protected bool Equals(Message_TradingPost other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
		{
			return value == other.value;
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
		return Equals((Message_TradingPost)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), value);
	}

	public static bool operator ==(Message_TradingPost left, Message_TradingPost right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_TradingPost left, Message_TradingPost right)
	{
		return !object.Equals(left, right);
	}

	public Message_TradingPost()
	{
	}

	public Message_TradingPost(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, ulong steamID, int value)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		this.value = value;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
	}
}
