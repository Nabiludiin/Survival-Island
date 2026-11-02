using System;
using Unity.Netcode;

[Serializable]
public class Message_Brick : Message_NetworkBehaviour
{
	public uint brickObjectIndex;

	protected bool Equals(Message_Brick other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return brickObjectIndex == other.brickObjectIndex;
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
		return Equals((Message_Brick)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), brickObjectIndex);
	}

	public static bool operator ==(Message_Brick left, Message_Brick right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Brick left, Message_Brick right)
	{
		return !object.Equals(left, right);
	}

	public Message_Brick()
	{
	}

	public Message_Brick(Messages type, MonoBehaviour_Network behaviour, uint brickObjectIndex)
		: base(type, behaviour)
	{
		this.brickObjectIndex = brickObjectIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in brickObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out brickObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
