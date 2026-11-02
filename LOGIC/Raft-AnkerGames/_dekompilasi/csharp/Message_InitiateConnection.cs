using System;
using Unity.Netcode;

[Serializable]
public class Message_InitiateConnection : Message
{
	public int appBuildID;

	public string password;

	protected bool Equals(Message_InitiateConnection other)
	{
		if (Equals((Message)other) && appBuildID == other.appBuildID)
		{
			return password == other.password;
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
		return Equals((Message_InitiateConnection)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), appBuildID, password);
	}

	public static bool operator ==(Message_InitiateConnection left, Message_InitiateConnection right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_InitiateConnection left, Message_InitiateConnection right)
	{
		return !object.Equals(left, right);
	}

	public Message_InitiateConnection(Messages type, int appBuildID, string password)
		: base(type)
	{
		this.appBuildID = appBuildID;
		this.password = password;
	}

	public Message_InitiateConnection()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in appBuildID, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(password);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out appBuildID, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out password, false);
	}
}
