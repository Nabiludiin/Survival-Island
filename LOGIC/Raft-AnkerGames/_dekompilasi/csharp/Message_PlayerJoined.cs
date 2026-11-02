using System;
using Unity.Netcode;

[Serializable]
public class Message_PlayerJoined : Message
{
	public RGD_Settings_Character characterSettings;

	protected bool Equals(Message_PlayerJoined other)
	{
		if (Equals((Message)other))
		{
			return object.Equals(characterSettings, other.characterSettings);
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
		return Equals((Message_PlayerJoined)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), characterSettings);
	}

	public static bool operator ==(Message_PlayerJoined left, Message_PlayerJoined right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_PlayerJoined left, Message_PlayerJoined right)
	{
		return !object.Equals(left, right);
	}

	public Message_PlayerJoined()
	{
	}

	public Message_PlayerJoined(Messages type, RGD_Settings_Character characterSettings)
		: base(type)
	{
		this.characterSettings = characterSettings;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		characterSettings.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		characterSettings = new RGD_Settings_Character();
		characterSettings.DeserializeFast(reader);
	}
}
