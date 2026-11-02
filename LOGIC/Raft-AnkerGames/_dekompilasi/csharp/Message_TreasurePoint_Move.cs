using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_TreasurePoint_Move : Message_NetworkBehaviour_ID
{
	public int x;

	public int y;

	public int z;

	public int q;

	public int w;

	public int e;

	public Vector3 Position
	{
		get
		{
			return new Vector3((float)x / 100f, (float)y / 100f, (float)z / 100f);
		}
		set
		{
			x = (int)(value.x * 100f);
			y = (int)(value.y * 100f);
			z = (int)(value.z * 100f);
		}
	}

	public Vector3 UpDirection
	{
		get
		{
			return new Vector3((float)q / 100f, (float)w / 100f, (float)e / 100f);
		}
		set
		{
			q = (int)(value.x * 100f);
			w = (int)(value.y * 100f);
			e = (int)(value.z * 100f);
		}
	}

	protected bool Equals(Message_TreasurePoint_Move other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && x == other.x && y == other.y && z == other.z && q == other.q && w == other.w)
		{
			return e == other.e;
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
		return Equals((Message_TreasurePoint_Move)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), x, y, z, q, w, e);
	}

	public static bool operator ==(Message_TreasurePoint_Move left, Message_TreasurePoint_Move right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_TreasurePoint_Move left, Message_TreasurePoint_Move right)
	{
		return !object.Equals(left, right);
	}

	public Message_TreasurePoint_Move()
	{
	}

	public Message_TreasurePoint_Move(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Vector3 position, Vector3 upDirection)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		Position = position;
		UpDirection = upDirection;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(24))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in q, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in w, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in e, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out q, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out w, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out e, default(FastBufferWriter.ForPrimitives));
	}
}
