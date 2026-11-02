using System;
using Unity.Netcode;

[Serializable]
public class Message_BlockCreator_UpdateHealth : Message_NetworkBehaviour
{
	public int newHealth;

	public int newMaxHealth;

	public uint blockObjectIndex;

	protected bool Equals(Message_BlockCreator_UpdateHealth other)
	{
		if (Equals((Message_NetworkBehaviour)other) && newHealth == other.newHealth && newMaxHealth == other.newMaxHealth)
		{
			return blockObjectIndex == other.blockObjectIndex;
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
		return Equals((Message_BlockCreator_UpdateHealth)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), newHealth, newMaxHealth, blockObjectIndex);
	}

	public static bool operator ==(Message_BlockCreator_UpdateHealth left, Message_BlockCreator_UpdateHealth right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_BlockCreator_UpdateHealth left, Message_BlockCreator_UpdateHealth right)
	{
		return !object.Equals(left, right);
	}

	public Message_BlockCreator_UpdateHealth(Messages type, MonoBehaviour_Network behaviour, uint blockObjectIndex, int newHealth, int newMaxHealth)
		: base(type, behaviour)
	{
		this.newHealth = newHealth;
		this.newMaxHealth = newMaxHealth;
		this.blockObjectIndex = blockObjectIndex;
	}

	public Message_BlockCreator_UpdateHealth()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in newHealth, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in newMaxHealth, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in blockObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out newHealth, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out newMaxHealth, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out blockObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
