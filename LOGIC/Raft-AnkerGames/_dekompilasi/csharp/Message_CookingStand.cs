using System;
using Unity.Netcode;

[Serializable]
public class Message_CookingStand : Message_NetworkBehaviour_ID_SteamID
{
	public uint cookingstandObjectIndex;

	protected bool Equals(Message_CookingStand other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
		{
			return cookingstandObjectIndex == other.cookingstandObjectIndex;
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
		return Equals((Message_CookingStand)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), cookingstandObjectIndex);
	}

	public static bool operator ==(Message_CookingStand left, Message_CookingStand right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_CookingStand left, Message_CookingStand right)
	{
		return !object.Equals(left, right);
	}

	public Message_CookingStand()
	{
	}

	public Message_CookingStand(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, Block_CookingStand stand)
		: base(type, behaviour, stand.ObjectIndex, steamID)
	{
		cookingstandObjectIndex = stand.ObjectIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in cookingstandObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out cookingstandObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
