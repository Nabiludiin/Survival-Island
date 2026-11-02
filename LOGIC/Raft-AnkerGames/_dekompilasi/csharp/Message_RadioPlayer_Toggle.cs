using System;
using Unity.Netcode;

[Serializable]
public class Message_RadioPlayer_Toggle : Message_NetworkBehaviour_ID
{
	public int timelinePosition;

	protected bool Equals(Message_RadioPlayer_Toggle other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return timelinePosition == other.timelinePosition;
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
		return Equals((Message_RadioPlayer_Toggle)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), timelinePosition);
	}

	public static bool operator ==(Message_RadioPlayer_Toggle left, Message_RadioPlayer_Toggle right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_RadioPlayer_Toggle left, Message_RadioPlayer_Toggle right)
	{
		return !object.Equals(left, right);
	}

	public Message_RadioPlayer_Toggle()
	{
	}

	public Message_RadioPlayer_Toggle(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, int timelinePosition)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.timelinePosition = timelinePosition;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in timelinePosition, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out timelinePosition, default(FastBufferWriter.ForPrimitives));
	}
}
