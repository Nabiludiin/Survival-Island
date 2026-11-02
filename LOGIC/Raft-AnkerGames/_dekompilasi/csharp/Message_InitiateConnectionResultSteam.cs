using System;
using Unity.Netcode;

[Serializable]
public class Message_InitiateConnectionResultSteam : Message
{
	public InitiateResult result;

	public string conString;

	protected bool Equals(Message_InitiateConnectionResultSteam other)
	{
		if (Equals((Message)other) && result == other.result)
		{
			return conString == other.conString;
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
		return Equals((Message_InitiateConnectionResultSteam)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), (int)result, conString);
	}

	public static bool operator ==(Message_InitiateConnectionResultSteam left, Message_InitiateConnectionResultSteam right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_InitiateConnectionResultSteam left, Message_InitiateConnectionResultSteam right)
	{
		return !object.Equals(left, right);
	}

	public Message_InitiateConnectionResultSteam(Messages type, InitiateResult result, string conString)
	{
		Type = type;
		this.result = result;
		this.conString = conString;
	}

	public Message_InitiateConnectionResultSteam()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in result, default(FastBufferWriter.ForEnums));
		writer.WriteValueSafe(conString);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out result, default(FastBufferWriter.ForEnums));
		reader.ReadValue(out conString, false);
	}
}
