using System;
using Unity.Netcode;

[Serializable]
public class Message_Container_ObjectIndex : Message_NetworkBehaviour_ID_SteamID
{
	public uint itemObjectIndex;

	protected bool Equals(Message_Container_ObjectIndex other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
		{
			return itemObjectIndex == other.itemObjectIndex;
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
		return Equals((Message_Container_ObjectIndex)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), itemObjectIndex);
	}

	public static bool operator ==(Message_Container_ObjectIndex left, Message_Container_ObjectIndex right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Container_ObjectIndex left, Message_Container_ObjectIndex right)
	{
		return !object.Equals(left, right);
	}

	public Message_Container_ObjectIndex()
	{
	}

	public Message_Container_ObjectIndex(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Network_UserId steamID, uint itemObjectIndex)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		this.itemObjectIndex = itemObjectIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in itemObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out itemObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
