using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_RotationComponentSetRotation : Message_NetworkBehaviour_ID
{
	public int q;

	public int w;

	public int v;

	public Vector3 Euler
	{
		get
		{
			return new Vector3((float)q / 100f, (float)w / 100f, (float)v / 100f);
		}
		set
		{
			q = (int)(value.x * 100f);
			w = (int)(value.y * 100f);
			v = (int)(value.z * 100f);
		}
	}

	protected bool Equals(Message_RotationComponentSetRotation other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && q == other.q && w == other.w)
		{
			return v == other.v;
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
		return Equals((Message_RotationComponentSetRotation)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), q, w, v);
	}

	public static bool operator ==(Message_RotationComponentSetRotation left, Message_RotationComponentSetRotation right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_RotationComponentSetRotation left, Message_RotationComponentSetRotation right)
	{
		return !object.Equals(left, right);
	}

	public Message_RotationComponentSetRotation()
	{
	}

	public Message_RotationComponentSetRotation(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Vector3 rotation)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		Euler = rotation;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(12))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in q, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in w, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in v, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out q, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out w, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out v, default(FastBufferWriter.ForPrimitives));
	}
}
