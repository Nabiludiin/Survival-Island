using System;
using Unity.Netcode;

[Serializable]
public class Message_RaftLandmark_Update : Message_NetworkBehaviour
{
	public SerializableVector3 Position;

	public SerializableVector3 Euler;

	protected bool Equals(Message_RaftLandmark_Update other)
	{
		if (Equals((Message_NetworkBehaviour)other) && object.Equals(Position, other.Position))
		{
			return object.Equals(Euler, other.Euler);
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
		return Equals((Message_RaftLandmark_Update)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Position, Euler);
	}

	public static bool operator ==(Message_RaftLandmark_Update left, Message_RaftLandmark_Update right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_RaftLandmark_Update left, Message_RaftLandmark_Update right)
	{
		return !object.Equals(left, right);
	}

	public Message_RaftLandmark_Update()
	{
	}

	public Message_RaftLandmark_Update(Messages type, MonoBehaviour_Network behaviour, RaftLandmark raftLandmark)
		: base(type, behaviour)
	{
		Position = new SerializableVector3(raftLandmark.transform.position);
		Euler = new SerializableVector3(raftLandmark.transform.eulerAngles);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(24))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in Position.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Position.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Position.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Euler.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Euler.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Euler.z, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		Position = new SerializableVector3();
		Euler = new SerializableVector3();
		reader.ReadValue(out Position.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Position.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Position.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Euler.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Euler.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Euler.z, default(FastBufferWriter.ForPrimitives));
	}
}
