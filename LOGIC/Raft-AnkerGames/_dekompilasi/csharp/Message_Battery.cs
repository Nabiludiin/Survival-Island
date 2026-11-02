using System;
using Unity.Netcode;

[Serializable]
public class Message_Battery : Message_NetworkBehaviour_ID_SteamID
{
	public int batteryUsesLeft;

	public int batteryIndex;

	public int batteryUniqueIndex;

	protected bool Equals(Message_Battery other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other) && batteryUsesLeft == other.batteryUsesLeft && batteryIndex == other.batteryIndex)
		{
			return batteryUniqueIndex == other.batteryUniqueIndex;
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
		return Equals((Message_Battery)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), batteryUsesLeft, batteryIndex, batteryUniqueIndex);
	}

	public static bool operator ==(Message_Battery left, Message_Battery right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Battery left, Message_Battery right)
	{
		return !object.Equals(left, right);
	}

	public Message_Battery()
	{
	}

	public Message_Battery(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, uint networkID_ObjectIndex, int batteryUsesLeft, int batteryIndex = 0, int batteryUniqueIndex = 176)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		this.batteryUsesLeft = batteryUsesLeft;
		this.batteryIndex = batteryIndex;
		this.batteryUniqueIndex = batteryUniqueIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(12))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in batteryUsesLeft, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in batteryIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in batteryUniqueIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out batteryUsesLeft, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out batteryIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out batteryUniqueIndex, default(FastBufferWriter.ForPrimitives));
	}
}
