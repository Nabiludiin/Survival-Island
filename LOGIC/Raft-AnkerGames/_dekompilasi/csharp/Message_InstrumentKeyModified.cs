using System;
using Unity.Netcode;

[Serializable]
public class Message_InstrumentKeyModified : Message_NetworkBehaviour_ID
{
	public int pitchIndex = -1;

	public int keyIndex = -1;

	public bool pressed;

	protected bool Equals(Message_InstrumentKeyModified other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && pitchIndex == other.pitchIndex && keyIndex == other.keyIndex)
		{
			return pressed == other.pressed;
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
		return Equals((Message_InstrumentKeyModified)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), pitchIndex, keyIndex, pressed);
	}

	public static bool operator ==(Message_InstrumentKeyModified left, Message_InstrumentKeyModified right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_InstrumentKeyModified left, Message_InstrumentKeyModified right)
	{
		return !object.Equals(left, right);
	}

	public Message_InstrumentKeyModified()
	{
	}

	public Message_InstrumentKeyModified(Messages type, MonoBehaviour_Network behaviour, uint objectIndex, int keyIndex, bool pressed, int pitchIndex)
		: base(type, behaviour, objectIndex)
	{
		this.keyIndex = keyIndex;
		this.pressed = pressed;
		this.pitchIndex = pitchIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in pitchIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in keyIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in pressed, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out pitchIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out keyIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out pressed, default(FastBufferWriter.ForPrimitives));
	}
}
