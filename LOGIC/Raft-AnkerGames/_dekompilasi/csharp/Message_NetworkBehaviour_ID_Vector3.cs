using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_NetworkBehaviour_ID_Vector3 : Message_NetworkBehaviour_ID
{
	public SerializableVector3 Vector;

	protected bool Equals(Message_NetworkBehaviour_ID_Vector3 other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return object.Equals(Vector, other.Vector);
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
		return Equals((Message_NetworkBehaviour_ID_Vector3)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Vector);
	}

	public static bool operator ==(Message_NetworkBehaviour_ID_Vector3 left, Message_NetworkBehaviour_ID_Vector3 right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NetworkBehaviour_ID_Vector3 left, Message_NetworkBehaviour_ID_Vector3 right)
	{
		return !object.Equals(left, right);
	}

	public Message_NetworkBehaviour_ID_Vector3()
	{
	}

	public Message_NetworkBehaviour_ID_Vector3(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Vector3 value)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		Vector = new SerializableVector3(value);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(12))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in Vector.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Vector.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Vector.z, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		Vector = new SerializableVector3();
		reader.ReadValue(out Vector.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Vector.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Vector.z, default(FastBufferWriter.ForPrimitives));
	}
}
