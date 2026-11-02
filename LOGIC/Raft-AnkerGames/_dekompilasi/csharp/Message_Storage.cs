using System;
using Unity.Netcode;

[Serializable]
public class Message_Storage : Message_NetworkBehaviour
{
	public uint storageObjectIndex;

	protected bool Equals(Message_Storage other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return storageObjectIndex == other.storageObjectIndex;
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
		return Equals((Message_Storage)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), storageObjectIndex);
	}

	public static bool operator ==(Message_Storage left, Message_Storage right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Storage left, Message_Storage right)
	{
		return !object.Equals(left, right);
	}

	public Message_Storage()
	{
	}

	public Message_Storage(Messages type, MonoBehaviour_Network behaviour, Storage_Small storage)
		: base(type, behaviour)
	{
		storageObjectIndex = storage.ObjectIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in storageObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out storageObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
