using System;
using Unity.Netcode;

[Serializable]
public class Message_InitiateConnectionResult : Message
{
	public InitiateResult result;

	public GameMode gameMode;

	protected bool Equals(Message_InitiateConnectionResult other)
	{
		if (Equals((Message)other) && result == other.result)
		{
			return gameMode == other.gameMode;
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
		return Equals((Message_InitiateConnectionResult)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), (int)result, (int)gameMode);
	}

	public static bool operator ==(Message_InitiateConnectionResult left, Message_InitiateConnectionResult right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_InitiateConnectionResult left, Message_InitiateConnectionResult right)
	{
		return !object.Equals(left, right);
	}

	public Message_InitiateConnectionResult(Messages type, InitiateResult result, GameMode gameMode)
	{
		Type = type;
		this.result = result;
		this.gameMode = gameMode;
	}

	public Message_InitiateConnectionResult()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in result, default(FastBufferWriter.ForEnums));
		writer.WriteValueSafe(in gameMode, default(FastBufferWriter.ForEnums));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out result, default(FastBufferWriter.ForEnums));
		reader.ReadValue(out gameMode, default(FastBufferWriter.ForEnums));
	}
}
