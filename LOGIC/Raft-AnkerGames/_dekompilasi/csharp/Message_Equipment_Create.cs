using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Equipment_Create : Message_NetworkBehaviour
{
	public int[] equipedIndexes;

	protected bool Equals(Message_Equipment_Create other)
	{
		if (equipedIndexes == null && other.equipedIndexes != null)
		{
			return false;
		}
		if (equipedIndexes != null && other.equipedIndexes == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (equipedIndexes != null || other.equipedIndexes != null)
			{
				return Enumerable.SequenceEqual(equipedIndexes, other.equipedIndexes);
			}
			return true;
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
		return Equals((Message_Equipment_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), equipedIndexes);
	}

	public static bool operator ==(Message_Equipment_Create left, Message_Equipment_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Equipment_Create left, Message_Equipment_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_Equipment_Create()
	{
	}

	public Message_Equipment_Create(Messages type, MonoBehaviour_Network behaviour, PlayerEquipment playerEquipment)
		: base(type, behaviour)
	{
		equipedIndexes = playerEquipment.GetEquipedIndexes().ToArray();
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(equipedIndexes, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out equipedIndexes, default(FastBufferWriter.ForPrimitives));
	}
}
