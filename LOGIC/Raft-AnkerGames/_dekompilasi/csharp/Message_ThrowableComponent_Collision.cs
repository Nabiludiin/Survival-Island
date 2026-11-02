using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_ThrowableComponent_Collision : Message_NetworkBehaviour_ID
{
	public uint throwableIndex;

	public int hitEntityIndex;

	public ParentType parentType;

	public SerializableVector3 position;

	public SerializableVector3 normal;

	protected bool Equals(Message_ThrowableComponent_Collision other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && throwableIndex == other.throwableIndex && hitEntityIndex == other.hitEntityIndex && parentType == other.parentType && object.Equals(position, other.position))
		{
			return object.Equals(normal, other.normal);
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
		return Equals((Message_ThrowableComponent_Collision)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), throwableIndex, hitEntityIndex, (int)parentType, position, normal);
	}

	public static bool operator ==(Message_ThrowableComponent_Collision left, Message_ThrowableComponent_Collision right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ThrowableComponent_Collision left, Message_ThrowableComponent_Collision right)
	{
		return !object.Equals(left, right);
	}

	public Message_ThrowableComponent_Collision()
	{
	}

	public Message_ThrowableComponent_Collision(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, uint throwableIndex, Vector3 position, Vector3 normal, ParentType parentType, int hitEntityIndex)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.throwableIndex = throwableIndex;
		this.position = new SerializableVector3(position);
		this.normal = new SerializableVector3(normal);
		this.parentType = parentType;
		this.hitEntityIndex = hitEntityIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(36))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in throwableIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in hitEntityIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in parentType, default(FastBufferWriter.ForEnums));
		writer.WriteValue(in position.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in normal.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in normal.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in normal.z, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out throwableIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out hitEntityIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out parentType, default(FastBufferWriter.ForEnums));
		position = new SerializableVector3();
		reader.ReadValue(out position.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.z, default(FastBufferWriter.ForPrimitives));
		normal = new SerializableVector3();
		reader.ReadValue(out normal.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out normal.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out normal.z, default(FastBufferWriter.ForPrimitives));
	}
}
