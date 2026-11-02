using System;
using Unity.Netcode;

[Serializable]
public class Message_OnResourceChange : Message_NetworkBehaviour_ID
{
	public bool resourceReady;

	protected bool Equals(Message_OnResourceChange other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return resourceReady == other.resourceReady;
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
		return Equals((Message_OnResourceChange)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), resourceReady);
	}

	public static bool operator ==(Message_OnResourceChange left, Message_OnResourceChange right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_OnResourceChange left, Message_OnResourceChange right)
	{
		return !object.Equals(left, right);
	}

	public Message_OnResourceChange()
	{
	}

	public Message_OnResourceChange(Messages p_messageType, MonoBehaviour_Network p_behaviour, uint networkID_ObjectIndex, bool p_resourceReady)
		: base(p_messageType, p_behaviour, networkID_ObjectIndex)
	{
		resourceReady = p_resourceReady;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in resourceReady, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out resourceReady, default(FastBufferWriter.ForPrimitives));
	}
}
