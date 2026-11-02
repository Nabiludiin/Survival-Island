using System;
using Unity.Netcode;

[Serializable]
public class Message_BlockCreator_SetReinforced : Message_NetworkBehaviour_SteamID
{
	public bool fortifyValue;

	public uint blockObjectIndex;

	protected bool Equals(Message_BlockCreator_SetReinforced other)
	{
		if (Equals((Message_NetworkBehaviour_SteamID)other) && fortifyValue == other.fortifyValue)
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
		return Equals((Message_BlockCreator_SetReinforced)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), fortifyValue, blockObjectIndex);
	}

	public static bool operator ==(Message_BlockCreator_SetReinforced left, Message_BlockCreator_SetReinforced right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_BlockCreator_SetReinforced left, Message_BlockCreator_SetReinforced right)
	{
		return !object.Equals(left, right);
	}

	public Message_BlockCreator_SetReinforced(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, uint blockObjectIndex, bool fortifyValue)
		: base(type, behaviour, steamID)
	{
		this.fortifyValue = fortifyValue;
		this.blockObjectIndex = blockObjectIndex;
	}

	public Message_BlockCreator_SetReinforced()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in fortifyValue, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in blockObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out fortifyValue, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out blockObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
