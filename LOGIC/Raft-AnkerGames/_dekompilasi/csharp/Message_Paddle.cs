using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Paddle : Message_NetworkBehaviour
{
	[SerializeField]
	private float xPos;

	[SerializeField]
	private float yPos;

	[SerializeField]
	private float zPos;

	[SerializeField]
	private float xDir;

	[SerializeField]
	private float yDir;

	[SerializeField]
	private float zDir;

	public float paddleForce;

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

	public Vector3 Direction
	{
		get
		{
			return new Vector3(xDir, yDir, zDir);
		}
		set
		{
			xDir = value.x;
			yDir = value.y;
			zDir = value.z;
		}
	}

	protected bool Equals(Message_Paddle other)
	{
		if (Equals((Message_NetworkBehaviour)other) && xPos.Equals(other.xPos) && yPos.Equals(other.yPos) && zPos.Equals(other.zPos) && xDir.Equals(other.xDir) && yDir.Equals(other.yDir) && zDir.Equals(other.zDir))
		{
			return paddleForce.Equals(other.paddleForce);
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
		return Equals((Message_Paddle)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), xPos, yPos, zPos, xDir, yDir, zDir, paddleForce);
	}

	public static bool operator ==(Message_Paddle left, Message_Paddle right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Paddle left, Message_Paddle right)
	{
		return !object.Equals(left, right);
	}

	public Message_Paddle()
	{
	}

	public Message_Paddle(Messages type, MonoBehaviour_Network behaviour, Vector3 position, Vector3 direction, float paddleForce)
		: base(type, behaviour)
	{
		Position = position;
		Direction = direction;
		this.paddleForce = paddleForce;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(28))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in xPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in xDir, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yDir, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zDir, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in paddleForce, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out xPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out xDir, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yDir, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zDir, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out paddleForce, default(FastBufferWriter.ForPrimitives));
	}
}
