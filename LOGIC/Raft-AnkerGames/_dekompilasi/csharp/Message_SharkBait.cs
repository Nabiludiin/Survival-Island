using System;
using Unity.Netcode;

[Serializable]
public class Message_SharkBait : Message_NetworkBehaviour
{
	public uint sharkBaitObjectIndex;

	protected bool Equals(Message_SharkBait other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return sharkBaitObjectIndex == other.sharkBaitObjectIndex;
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
		return Equals((Message_SharkBait)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), sharkBaitObjectIndex);
	}

	public static bool operator ==(Message_SharkBait left, Message_SharkBait right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_SharkBait left, Message_SharkBait right)
	{
		return !object.Equals(left, right);
	}

	public Message_SharkBait(Messages type, MonoBehaviour_Network behaviour)
		: base(type, behaviour)
	{
	}

	public Message_SharkBait()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in sharkBaitObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out sharkBaitObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
