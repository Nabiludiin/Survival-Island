using System;
using Unity.Netcode;

[Serializable]
public class Message_WorldEventToggle_Off : Message_NetworkBehaviour_ID
{
	public int worldEventTypeIndex;

	protected bool Equals(Message_WorldEventToggle_Off other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return worldEventTypeIndex == other.worldEventTypeIndex;
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
		return Equals((Message_WorldEventToggle_Off)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), worldEventTypeIndex);
	}

	public static bool operator ==(Message_WorldEventToggle_Off left, Message_WorldEventToggle_Off right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_WorldEventToggle_Off left, Message_WorldEventToggle_Off right)
	{
		return !object.Equals(left, right);
	}

	public Message_WorldEventToggle_Off()
	{
	}

	public Message_WorldEventToggle_Off(Messages type, MonoBehaviour_Network behaviour, uint networkID, SO_WorldEvent.WorldEventType weType)
		: base(type, behaviour, networkID)
	{
		worldEventTypeIndex = (int)weType;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in worldEventTypeIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out worldEventTypeIndex, default(FastBufferWriter.ForPrimitives));
	}
}
