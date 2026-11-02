using System;
using Unity.Netcode;

[Serializable]
public class Message_WorldInfo : Message
{
	public string worldGuid;

	public bool friendlyFire;

	public int traderReputation;

	protected bool Equals(Message_WorldInfo other)
	{
		if (Equals((Message)other) && worldGuid == other.worldGuid && friendlyFire == other.friendlyFire)
		{
			return traderReputation == other.traderReputation;
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
		return Equals((Message_WorldInfo)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), worldGuid, friendlyFire, traderReputation);
	}

	public static bool operator ==(Message_WorldInfo left, Message_WorldInfo right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_WorldInfo left, Message_WorldInfo right)
	{
		return !object.Equals(left, right);
	}

	public Message_WorldInfo(Messages type, Guid guid)
		: base(type)
	{
		worldGuid = guid.ToString();
		friendlyFire = GameManager.FriendlyFire;
		TradingPostUI value = ComponentManager<TradingPostUI>.Value;
		if (value != null)
		{
			traderReputation = value.CurrentReputation;
		}
	}

	public Message_WorldInfo()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(worldGuid);
		writer.WriteValueSafe(in friendlyFire, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in traderReputation, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out worldGuid, false);
		reader.ReadValue(out friendlyFire, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out traderReputation, default(FastBufferWriter.ForPrimitives));
	}
}
