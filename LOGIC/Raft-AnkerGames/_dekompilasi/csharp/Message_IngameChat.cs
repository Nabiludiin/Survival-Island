using System;
using Unity.Netcode;

[Serializable]
public class Message_IngameChat : Message_NetworkBehaviour_SteamID
{
	public string messageText;

	protected bool Equals(Message_IngameChat other)
	{
		if (Equals((Message_NetworkBehaviour_SteamID)other))
		{
			return messageText == other.messageText;
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
		return Equals((Message_IngameChat)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), messageText);
	}

	public static bool operator ==(Message_IngameChat left, Message_IngameChat right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_IngameChat left, Message_IngameChat right)
	{
		return !object.Equals(left, right);
	}

	public Message_IngameChat()
	{
	}

	public Message_IngameChat(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, string p_messageText)
		: base(type, behaviour, steamID)
	{
		messageText = p_messageText;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(messageText);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out messageText, false);
	}
}
