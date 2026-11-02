using System;
using Unity.Netcode;

[Serializable]
public class Message_RotationComponentRotate : Message_NetworkBehaviour_ID
{
	public int r;

	public float Rotation
	{
		get
		{
			return (float)r / 10000f;
		}
		set
		{
			r = (int)(value * 10000f);
		}
	}

	protected bool Equals(Message_RotationComponentRotate other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return r == other.r;
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
		return Equals((Message_RotationComponentRotate)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), r);
	}

	public static bool operator ==(Message_RotationComponentRotate left, Message_RotationComponentRotate right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_RotationComponentRotate left, Message_RotationComponentRotate right)
	{
		return !object.Equals(left, right);
	}

	public Message_RotationComponentRotate()
	{
	}

	public Message_RotationComponentRotate(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, float degree)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		Rotation = degree;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in r, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out r, default(FastBufferWriter.ForPrimitives));
	}
}
