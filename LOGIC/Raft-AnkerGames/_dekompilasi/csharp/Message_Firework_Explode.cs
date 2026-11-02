using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Firework_Explode : Message_NetworkBehaviour_ID
{
	public int x;

	public int y;

	public int z;

	public int particlePrefabIndex;

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

	protected bool Equals(Message_Firework_Explode other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && x == other.x && y == other.y && z == other.z)
		{
			return particlePrefabIndex == other.particlePrefabIndex;
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
		return Equals((Message_Firework_Explode)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), x, y, z, particlePrefabIndex);
	}

	public static bool operator ==(Message_Firework_Explode left, Message_Firework_Explode right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Firework_Explode left, Message_Firework_Explode right)
	{
		return !object.Equals(left, right);
	}

	public Message_Firework_Explode()
	{
	}

	public Message_Firework_Explode(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Vector3 position, int particlePrefabIndex)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		Position = position;
		this.particlePrefabIndex = particlePrefabIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(16))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in particlePrefabIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out particlePrefabIndex, default(FastBufferWriter.ForPrimitives));
	}
}
