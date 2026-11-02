using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_PaintBlock : Message_NetworkBehaviour
{
	public int x;

	public int y;

	public int z;

	public uint boi;

	public uint ci;

	public uint pci;

	public int ps;

	public uint pi;

	public Vector3 LocalLockedPivotPosition
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

	protected bool Equals(Message_PaintBlock other)
	{
		if (Equals((Message_NetworkBehaviour)other) && x == other.x && y == other.y && z == other.z && boi == other.boi && ci == other.ci && pci == other.pci && ps == other.ps)
		{
			return pi == other.pi;
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
		return Equals((Message_PaintBlock)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add(x);
		hashCode.Add(y);
		hashCode.Add(z);
		hashCode.Add(boi);
		hashCode.Add(ci);
		hashCode.Add(pci);
		hashCode.Add(ps);
		hashCode.Add(pi);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_PaintBlock left, Message_PaintBlock right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_PaintBlock left, Message_PaintBlock right)
	{
		return !object.Equals(left, right);
	}

	public Message_PaintBlock()
	{
	}

	public Message_PaintBlock(Messages type, MonoBehaviour_Network behaviour, uint blockObjectIndex, Vector3 position, SO_ColorValue color, SO_ColorValue patternColor, int paintSide, uint patternIndex)
		: base(type, behaviour)
	{
		LocalLockedPivotPosition = position;
		boi = blockObjectIndex;
		ci = color.uniqueColorIndex;
		pci = patternColor.uniqueColorIndex;
		ps = paintSide;
		pi = patternIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(32))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in boi, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in ci, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in pci, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in ps, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in pi, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out boi, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out ci, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out pci, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out ps, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out pi, default(FastBufferWriter.ForPrimitives));
	}
}
