using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Seagull_Update : Message_NetworkBehaviour_ID
{
	public int x;

	public int y;

	public int z;

	public int q;

	public int w;

	public int v;

	public byte s;

	public byte p;

	public bool[] a = new bool[6];

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

	public Vector3 ForwardDirection
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

	public SeagullState SeagullState
	{
		get
		{
			return (SeagullState)s;
		}
		set
		{
			s = (byte)value;
		}
	}

	public SeagullParentEnum SeagullParentEnum
	{
		get
		{
			return (SeagullParentEnum)p;
		}
		set
		{
			p = (byte)value;
		}
	}

	public bool Sitting
	{
		get
		{
			return a[0];
		}
		set
		{
			a[0] = value;
		}
	}

	public bool Anim_Sitting
	{
		get
		{
			return a[1];
		}
		set
		{
			a[1] = value;
		}
	}

	public bool Anim_FlyingAltitude
	{
		get
		{
			return a[2];
		}
		set
		{
			a[2] = value;
		}
	}

	public bool Anim_Nest
	{
		get
		{
			return a[3];
		}
		set
		{
			a[3] = value;
		}
	}

	public bool Anim_Cropplot
	{
		get
		{
			return a[4];
		}
		set
		{
			a[4] = value;
		}
	}

	public bool Anim_Scarecrow
	{
		get
		{
			return a[5];
		}
		set
		{
			a[5] = value;
		}
	}

	protected bool Equals(Message_Seagull_Update other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && x == other.x && y == other.y && z == other.z && q == other.q && w == other.w && v == other.v && s == other.s && p == other.p)
		{
			return Enumerable.SequenceEqual(a, other.a);
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
		return Equals((Message_Seagull_Update)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add(x);
		hashCode.Add(y);
		hashCode.Add(z);
		hashCode.Add(q);
		hashCode.Add(w);
		hashCode.Add(v);
		hashCode.Add(s);
		hashCode.Add(p);
		hashCode.Add(a);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_Seagull_Update left, Message_Seagull_Update right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Seagull_Update left, Message_Seagull_Update right)
	{
		return !object.Equals(left, right);
	}

	public Message_Seagull_Update()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(26))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in q, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in w, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in v, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in s, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in p, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(a, default(FastBufferWriter.ForPrimitives));
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
		reader.ReadValue(out s, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out p, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out a, default(FastBufferWriter.ForPrimitives));
	}

	public Message_Seagull_Update(Messages type, MonoBehaviour_Network behaviour, uint networkObjectIndex, Seagull seagull)
		: base(type, behaviour, networkObjectIndex)
	{
		SeagullState = seagull.currentState;
		Sitting = seagull.Sitting;
		SeagullParentEnum = seagull.GetParent();
		if (SeagullParentEnum == SeagullParentEnum.None)
		{
			Position = seagull.transform.position;
		}
		else
		{
			Position = seagull.transform.localPosition;
		}
		ForwardDirection = seagull.transform.forward;
		Anim_Sitting = seagull.anim.GetBool("Sit");
		Anim_FlyingAltitude = seagull.anim.GetBool("FlyingAltitude");
		Anim_Nest = seagull.anim.GetBool("Nest");
		Anim_Cropplot = seagull.anim.GetBool("Peck_Cropplot");
		Anim_Scarecrow = seagull.anim.GetBool("Peck_Scarecrow");
	}
}
