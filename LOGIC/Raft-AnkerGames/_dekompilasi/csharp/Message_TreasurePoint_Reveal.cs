using System;
using Unity.Netcode;

[Serializable]
public class Message_TreasurePoint_Reveal : Message_NetworkBehaviour_ID
{
	public bool reveal = true;

	protected bool Equals(Message_TreasurePoint_Reveal other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return reveal == other.reveal;
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
		return Equals((Message_TreasurePoint_Reveal)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), reveal);
	}

	public static bool operator ==(Message_TreasurePoint_Reveal left, Message_TreasurePoint_Reveal right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_TreasurePoint_Reveal left, Message_TreasurePoint_Reveal right)
	{
		return !object.Equals(left, right);
	}

	public Message_TreasurePoint_Reveal()
	{
	}

	public Message_TreasurePoint_Reveal(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, bool toReveal)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		reveal = toReveal;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in reveal, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out reveal, default(FastBufferWriter.ForPrimitives));
	}
}
