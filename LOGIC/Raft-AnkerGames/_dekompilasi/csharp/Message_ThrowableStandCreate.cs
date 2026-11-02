using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_ThrowableStandCreate : Message_NetworkBehaviour
{
	[SerializeField]
	private float xPos;

	[SerializeField]
	private float yPos;

	[SerializeField]
	private float zPos;

	[SerializeField]
	private float xRot;

	[SerializeField]
	private float yRot;

	[SerializeField]
	private float zRot;

	public Vector3 Position
	{
		get
		{
			return new Vector3(xPos, yPos, zPos);
		}
		set
		{
			xPos = value.x;
			yPos = value.y;
			zPos = value.z;
		}
	}

	public Vector3 Rotation
	{
		get
		{
			return new Vector3(xRot, yRot, zRot);
		}
		set
		{
			xRot = value.x;
			yRot = value.y;
			zRot = value.z;
		}
	}

	protected bool Equals(Message_ThrowableStandCreate other)
	{
		if (Equals((Message_NetworkBehaviour)other) && xPos.Equals(other.xPos) && yPos.Equals(other.yPos) && zPos.Equals(other.zPos) && xRot.Equals(other.xRot) && yRot.Equals(other.yRot))
		{
			return zRot.Equals(other.zRot);
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
		return Equals((Message_ThrowableStandCreate)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), xPos, yPos, zPos, xRot, yRot, zRot);
	}

	public static bool operator ==(Message_ThrowableStandCreate left, Message_ThrowableStandCreate right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ThrowableStandCreate left, Message_ThrowableStandCreate right)
	{
		return !object.Equals(left, right);
	}

	public Message_ThrowableStandCreate()
	{
	}

	public Message_ThrowableStandCreate(Messages type, MonoBehaviour_Network behaviour, Transform anchor)
		: base(type, behaviour)
	{
		Position = anchor.position;
		Rotation = anchor.eulerAngles;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(24))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in xPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in xRot, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yRot, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zRot, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out xPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out xRot, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yRot, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zRot, default(FastBufferWriter.ForPrimitives));
	}
}
