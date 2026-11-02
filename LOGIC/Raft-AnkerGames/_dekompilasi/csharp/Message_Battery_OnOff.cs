using System;
using Unity.Netcode;

[Serializable]
public class Message_Battery_OnOff : Message_Battery
{
	public bool on;

	protected bool Equals(Message_Battery_OnOff other)
	{
		if (Equals((Message_Battery)other))
		{
			return on == other.on;
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
		return Equals((Message_Battery_OnOff)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), on);
	}

	public static bool operator ==(Message_Battery_OnOff left, Message_Battery_OnOff right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Battery_OnOff left, Message_Battery_OnOff right)
	{
		return !object.Equals(left, right);
	}

	public Message_Battery_OnOff()
	{
	}

	public Message_Battery_OnOff(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, uint networkID_ObjectIndex, int batteryUsesLeft, bool on)
		: base(type, behaviour, steamID, networkID_ObjectIndex, batteryUsesLeft, 176)
	{
		this.on = on;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in on, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out on, default(FastBufferWriter.ForPrimitives));
	}
}
