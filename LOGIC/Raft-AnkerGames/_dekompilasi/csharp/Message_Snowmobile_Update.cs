using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Snowmobile_Update : Message_NetworkBehaviour_ID
{
	public int x;

	public int y;

	public int z;

	public int q;

	public int w;

	public int v;

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

	protected bool Equals(Message_Snowmobile_Update other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && x == other.x && y == other.y && z == other.z && q == other.q && w == other.w)
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
		return Equals((Message_Snowmobile_Update)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), x, y, z, q, w, v);
	}

	public static bool operator ==(Message_Snowmobile_Update left, Message_Snowmobile_Update right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Snowmobile_Update left, Message_Snowmobile_Update right)
	{
		return !object.Equals(left, right);
	}

	public Message_Snowmobile_Update()
	{
	}

	public Message_Snowmobile_Update(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Vector3 position, Vector3 euler)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		Position = position;
		Euler = euler;
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
		writer.WriteValue(in v, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out q, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out w, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out v, default(FastBufferWriter.ForPrimitives));
	}
}
