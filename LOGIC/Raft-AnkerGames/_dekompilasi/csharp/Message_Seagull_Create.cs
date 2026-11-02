using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Seagull_Create : Message_NetworkBehaviour_ID
{
	public uint seagullObjectIndex;

	public uint seagullBehaviourIndex;

	public uint seagullEntityObjectIndex;

	[SerializeField]
	private int x;

	[SerializeField]
	private int y;

	[SerializeField]
	private int z;

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

	protected bool Equals(Message_Seagull_Create other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && seagullObjectIndex == other.seagullObjectIndex && seagullBehaviourIndex == other.seagullBehaviourIndex && seagullEntityObjectIndex == other.seagullEntityObjectIndex && x == other.x && y == other.y)
		{
			return z == other.z;
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
		return Equals((Message_Seagull_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), seagullObjectIndex, seagullBehaviourIndex, seagullEntityObjectIndex, x, y, z);
	}

	public static bool operator ==(Message_Seagull_Create left, Message_Seagull_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Seagull_Create left, Message_Seagull_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_Seagull_Create()
	{
	}

	public Message_Seagull_Create(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Seagull seagull)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		seagullObjectIndex = seagull.ObjectIndex;
		seagullBehaviourIndex = seagull.BehaviourIndex;
		seagullEntityObjectIndex = seagull.entityComponent.ObjectIndex;
		Position = seagull.transform.position;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(24))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in seagullObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in seagullBehaviourIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in seagullEntityObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out seagullObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out seagullBehaviourIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out seagullEntityObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
	}
}
