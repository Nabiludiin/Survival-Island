using System;
using Unity.Netcode;

[Serializable]
public class Message_Raft : Message_NetworkBehaviour
{
	public int x;

	public int z;

	public int y;

	public int v;

	public float PositionX
	{
		get
		{
			return (float)x / 100f;
		}
		set
		{
			x = (int)(value * 100f);
		}
	}

	public float PositionZ
	{
		get
		{
			return (float)z / 100f;
		}
		set
		{
			z = (int)(value * 100f);
		}
	}

	public float RotationY
	{
		get
		{
			return (float)y / 100f;
		}
		set
		{
			y = (int)(value * 100f);
		}
	}

	public float Velocity
	{
		get
		{
			return (float)v / 100f;
		}
		set
		{
			v = (int)(value * 100f);
		}
	}

	protected bool Equals(Message_Raft other)
	{
		if (Equals((Message_NetworkBehaviour)other) && x == other.x && z == other.z && y == other.y)
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
		return Equals((Message_Raft)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), x, z, y, v);
	}

	public static bool operator ==(Message_Raft left, Message_Raft right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Raft left, Message_Raft right)
	{
		return !object.Equals(left, right);
	}

	public Message_Raft(Messages type, MonoBehaviour_Network behaviour, Raft raft, RaftCollisionManager raftCollisionManager)
		: base(type, behaviour)
	{
		PositionX = raftCollisionManager.transform.position.x;
		PositionZ = raftCollisionManager.transform.position.z;
		RotationY = raftCollisionManager.transform.eulerAngles.y;
		Velocity = raft.Velocity.sqrMagnitude;
	}

	public Message_Raft()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(16))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in v, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out v, default(FastBufferWriter.ForPrimitives));
	}
}
