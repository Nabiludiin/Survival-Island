using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_ThrowableComponent_Throw : Message_NetworkBehaviour_ID
{
	public uint throwableIndex;

	public int ammoItemIndex;

	public uint throwablePickupObjectIndex;

	public SerializableVector3 position;

	public SerializableVector3 force;

	protected bool Equals(Message_ThrowableComponent_Throw other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && throwableIndex == other.throwableIndex && ammoItemIndex == other.ammoItemIndex && throwablePickupObjectIndex == other.throwablePickupObjectIndex && object.Equals(position, other.position))
		{
			return object.Equals(force, other.force);
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
		return Equals((Message_ThrowableComponent_Throw)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), throwableIndex, ammoItemIndex, throwablePickupObjectIndex, position, force);
	}

	public static bool operator ==(Message_ThrowableComponent_Throw left, Message_ThrowableComponent_Throw right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ThrowableComponent_Throw left, Message_ThrowableComponent_Throw right)
	{
		return !object.Equals(left, right);
	}

	public Message_ThrowableComponent_Throw()
	{
	}

	public Message_ThrowableComponent_Throw(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Vector3 position, Vector3 force, int ammoItemIndex)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.position = new SerializableVector3(position);
		this.force = new SerializableVector3(force);
		this.ammoItemIndex = ammoItemIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(36))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in throwableIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in ammoItemIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in throwablePickupObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in force.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in force.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in force.z, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out throwableIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out ammoItemIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out throwablePickupObjectIndex, default(FastBufferWriter.ForPrimitives));
		position = new SerializableVector3();
		reader.ReadValue(out position.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.z, default(FastBufferWriter.ForPrimitives));
		force = new SerializableVector3();
		reader.ReadValue(out force.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out force.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out force.z, default(FastBufferWriter.ForPrimitives));
	}
}
