using System;
using Unity.Netcode;

[Serializable]
public class Message_Tank : Message_NetworkBehaviour_ID_SteamID
{
	public float tankIncrement;

	public int tankID;

	public int removedItemIndex;

	protected bool Equals(Message_Tank other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other) && tankIncrement.Equals(other.tankIncrement) && tankID == other.tankID)
		{
			return removedItemIndex == other.removedItemIndex;
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
		return Equals((Message_Tank)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), tankIncrement, tankID, removedItemIndex);
	}

	public static bool operator ==(Message_Tank left, Message_Tank right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Tank left, Message_Tank right)
	{
		return !object.Equals(left, right);
	}

	public Message_Tank()
	{
	}

	public Message_Tank(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, ulong steamID, float tankIncrement, int tankID = 0, int removedItemIndex = -1)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		this.tankIncrement = tankIncrement;
		this.tankID = tankID;
		this.removedItemIndex = removedItemIndex;
	}

	public Message_Tank(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Network_UserId steamID, float tankIncrement, int tankID = 0, int removedItemIndex = -1)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		this.tankIncrement = tankIncrement;
		this.tankID = tankID;
		this.removedItemIndex = removedItemIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(12))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in tankIncrement, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in tankID, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in removedItemIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out tankIncrement, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out tankID, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out removedItemIndex, default(FastBufferWriter.ForPrimitives));
	}
}
