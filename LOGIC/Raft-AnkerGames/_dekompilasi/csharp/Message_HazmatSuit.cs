using System;
using Unity.Netcode;

[Serializable]
public class Message_HazmatSuit : Message_NetworkBehaviour
{
	public bool equiped;

	public float duration;

	protected bool Equals(Message_HazmatSuit other)
	{
		if (Equals((Message_NetworkBehaviour)other) && equiped == other.equiped)
		{
			return duration.Equals(other.duration);
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
		return Equals((Message_HazmatSuit)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), equiped, duration);
	}

	public static bool operator ==(Message_HazmatSuit left, Message_HazmatSuit right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_HazmatSuit left, Message_HazmatSuit right)
	{
		return !object.Equals(left, right);
	}

	public Message_HazmatSuit()
	{
	}

	public Message_HazmatSuit(Messages type, MonoBehaviour_Network behaviour, bool equiped, float duration = -1f)
		: base(type, behaviour)
	{
		this.equiped = equiped;
		this.duration = duration;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in equiped, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in duration, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out equiped, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out duration, default(FastBufferWriter.ForPrimitives));
	}
}
