using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Firework_Launch : Message_NetworkBehaviour
{
	public int x;

	public int y;

	public int z;

	public int a;

	public int bb;

	public int c;

	[SerializeField]
	private int l;

	[SerializeField]
	private bool r;

	[SerializeField]
	private uint f;

	[SerializeField]
	private int v;

	public Vector3 StartPosition
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

	public Vector3 StartDirection
	{
		get
		{
			return new Vector3((float)a / 100f, (float)bb / 100f, (float)c / 100f);
		}
		set
		{
			a = (int)(value.x * 100f);
			bb = (int)(value.y * 100f);
			c = (int)(value.z * 100f);
		}
	}

	public float LifeTime
	{
		get
		{
			return (float)l / 100f;
		}
		set
		{
			l = (int)(value * 100f);
		}
	}

	public bool RemoveItemFromInventory
	{
		get
		{
			return r;
		}
		set
		{
			r = value;
		}
	}

	public uint FireworkObjectIndex
	{
		get
		{
			return f;
		}
		set
		{
			f = value;
		}
	}

	public int ColorValueIndex
	{
		get
		{
			return v;
		}
		set
		{
			v = value;
		}
	}

	protected bool Equals(Message_Firework_Launch other)
	{
		if (Equals((Message_NetworkBehaviour)other) && x == other.x && y == other.y && z == other.z && a == other.a && bb == other.bb && c == other.c && l == other.l && r == other.r && f == other.f)
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
		return Equals((Message_Firework_Launch)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add(x);
		hashCode.Add(y);
		hashCode.Add(z);
		hashCode.Add(a);
		hashCode.Add(bb);
		hashCode.Add(c);
		hashCode.Add(l);
		hashCode.Add(r);
		hashCode.Add(f);
		hashCode.Add(v);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_Firework_Launch left, Message_Firework_Launch right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Firework_Launch left, Message_Firework_Launch right)
	{
		return !object.Equals(left, right);
	}

	public Message_Firework_Launch()
	{
	}

	public Message_Firework_Launch(Messages type, MonoBehaviour_Network behaviour, Vector3 startPosition, Vector3 startDirection, float lifeTime, bool removeItemFromInventory, int colorValueIndex, uint fireworkObjectIndex)
		: base(type, behaviour)
	{
		StartPosition = startPosition;
		StartDirection = startDirection;
		LifeTime = lifeTime;
		RemoveItemFromInventory = removeItemFromInventory;
		FireworkObjectIndex = fireworkObjectIndex;
		ColorValueIndex = colorValueIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(37))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in a, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in bb, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in c, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in l, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in r, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in f, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in v, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out a, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out bb, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out c, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out l, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out r, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out f, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out v, default(FastBufferWriter.ForPrimitives));
	}
}
