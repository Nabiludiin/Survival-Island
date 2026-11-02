using System;
using Unity.Netcode;

[Serializable]
public class Message_FillYield : Message_NetworkBehaviour_ID
{
	public int currentHoneyLevelIndex;

	protected bool Equals(Message_FillYield other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return currentHoneyLevelIndex == other.currentHoneyLevelIndex;
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
		return Equals((Message_FillYield)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), currentHoneyLevelIndex);
	}

	public static bool operator ==(Message_FillYield left, Message_FillYield right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_FillYield left, Message_FillYield right)
	{
		return !object.Equals(left, right);
	}

	public Message_FillYield()
	{
	}

	public Message_FillYield(Messages type, MonoBehaviour_Network behaviour, int currentHoneyLevelIndex, uint networkID_ObjectIndex)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.currentHoneyLevelIndex = currentHoneyLevelIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in currentHoneyLevelIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out currentHoneyLevelIndex, default(FastBufferWriter.ForPrimitives));
	}
}
