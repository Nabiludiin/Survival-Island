using System;
using Unity.Netcode;

[Serializable]
public class Message_PickupTrophyOnBoard : Message_NetworkBehaviour_ID_SteamID
{
	public uint trophyBoardIndex;

	protected bool Equals(Message_PickupTrophyOnBoard other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
		{
			return trophyBoardIndex == other.trophyBoardIndex;
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
		return Equals((Message_PickupTrophyOnBoard)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), trophyBoardIndex);
	}

	public static bool operator ==(Message_PickupTrophyOnBoard left, Message_PickupTrophyOnBoard right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_PickupTrophyOnBoard left, Message_PickupTrophyOnBoard right)
	{
		return !object.Equals(left, right);
	}

	public Message_PickupTrophyOnBoard()
	{
	}

	public Message_PickupTrophyOnBoard(Messages p_messageType, Network_UserId steamID, MonoBehaviour_Network p_behavior, uint p_trophyBoardIndex)
		: base(p_messageType, p_behavior, p_trophyBoardIndex, steamID)
	{
		trophyBoardIndex = p_trophyBoardIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in trophyBoardIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out trophyBoardIndex, default(FastBufferWriter.ForPrimitives));
	}
}
