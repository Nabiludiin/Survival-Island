using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Throw : Message_NetworkBehaviour
{
	[SerializeField]
	private float xForce;

	[SerializeField]
	private float yForce;

	[SerializeField]
	private float zForce;

	public Vector3 Force
	{
		get
		{
			return new Vector3(xForce, yForce, zForce);
		}
		private set
		{
			xForce = value.x;
			yForce = value.y;
			zForce = value.z;
		}
	}

	protected bool Equals(Message_Throw other)
	{
		if (Equals((Message_NetworkBehaviour)other) && xForce.Equals(other.xForce) && yForce.Equals(other.yForce))
		{
			return zForce.Equals(other.zForce);
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
		return Equals((Message_Throw)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), xForce, yForce, zForce);
	}

	public static bool operator ==(Message_Throw left, Message_Throw right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Throw left, Message_Throw right)
	{
		return !object.Equals(left, right);
	}

	public Message_Throw()
	{
	}

	public Message_Throw(Messages type, MonoBehaviour_Network behaviour, Vector3 force)
		: base(type, behaviour)
	{
		Force = force;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(12))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in xForce, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yForce, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zForce, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out xForce, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yForce, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zForce, default(FastBufferWriter.ForPrimitives));
	}
}
