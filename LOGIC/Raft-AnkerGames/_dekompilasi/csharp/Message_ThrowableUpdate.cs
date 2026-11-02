using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_ThrowableUpdate : Message_NetworkBehaviour
{
	public bool inHand;

	public bool inWater;

	public bool pulling;

	public float xPos;

	public float yPos;

	public float zPos;

	protected bool Equals(Message_ThrowableUpdate other)
	{
		if (Equals((Message_NetworkBehaviour)other) && inHand == other.inHand && inWater == other.inWater && pulling == other.pulling && xPos.Equals(other.xPos) && yPos.Equals(other.yPos))
		{
			return zPos.Equals(other.zPos);
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
		return Equals((Message_ThrowableUpdate)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), inHand, inWater, pulling, xPos, yPos, zPos);
	}

	public static bool operator ==(Message_ThrowableUpdate left, Message_ThrowableUpdate right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ThrowableUpdate left, Message_ThrowableUpdate right)
	{
		return !object.Equals(left, right);
	}

	public Message_ThrowableUpdate(Messages type, MonoBehaviour_Network behaviour, Throwable throwable)
		: base(type, behaviour)
	{
		xPos = throwable.throwableObject.position.x;
		yPos = throwable.throwableObject.position.y;
		zPos = throwable.throwableObject.position.z;
		inHand = throwable.InHand;
		inWater = throwable.InWater;
		pulling = inWater && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(throwable.ThrowableAction, "LMB");
	}

	public Message_ThrowableUpdate()
	{
	}

	public Vector3 GetPosition()
	{
		return new Vector3(xPos, yPos, zPos);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(15))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in inHand, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in inWater, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in pulling, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in xPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zPos, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out inHand, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out inWater, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out pulling, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out xPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zPos, default(FastBufferWriter.ForPrimitives));
	}
}
