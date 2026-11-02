using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Chicken_LayEgg : Message_NetworkBehaviour_ID
{
	public SerializableVector3 localRaftPosition;

	public uint eggObjectIndex;

	protected bool Equals(Message_Chicken_LayEgg other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && object.Equals(localRaftPosition, other.localRaftPosition))
		{
			return eggObjectIndex == other.eggObjectIndex;
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
		return Equals((Message_Chicken_LayEgg)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), localRaftPosition, eggObjectIndex);
	}

	public static bool operator ==(Message_Chicken_LayEgg left, Message_Chicken_LayEgg right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Chicken_LayEgg left, Message_Chicken_LayEgg right)
	{
		return !object.Equals(left, right);
	}

	public Message_Chicken_LayEgg()
	{
	}

	public Message_Chicken_LayEgg(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Vector3 localRaftPosition, uint eggObjectIndex)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.localRaftPosition = new SerializableVector3(localRaftPosition);
		this.eggObjectIndex = eggObjectIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(16))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in localRaftPosition.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in localRaftPosition.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in localRaftPosition.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in eggObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		localRaftPosition = new SerializableVector3();
		reader.ReadValue(out localRaftPosition.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out localRaftPosition.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out localRaftPosition.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out eggObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
