using System;
using Unity.Netcode;

[Serializable]
public class Message_Bed_Enter : Message_NetworkBehaviour
{
	public uint bedObjectIndex;

	protected bool Equals(Message_Bed_Enter other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return bedObjectIndex == other.bedObjectIndex;
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
		return Equals((Message_Bed_Enter)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), bedObjectIndex);
	}

	public static bool operator ==(Message_Bed_Enter left, Message_Bed_Enter right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Bed_Enter left, Message_Bed_Enter right)
	{
		return !object.Equals(left, right);
	}

	public Message_Bed_Enter()
	{
	}

	public Message_Bed_Enter(Messages type, MonoBehaviour_Network behaviour, uint bedObjectIndex)
		: base(type, behaviour)
	{
		this.bedObjectIndex = bedObjectIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in bedObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out bedObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
