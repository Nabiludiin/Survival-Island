using System;
using Unity.Netcode;

[Serializable]
public class Message_PlaceTrophyOnBoard : Message_NetworkBehaviour_ID_SteamID
{
	public uint trophyBoardIndex;

	public int itemIndex;

	protected bool Equals(Message_PlaceTrophyOnBoard other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other) && trophyBoardIndex == other.trophyBoardIndex)
		{
			return itemIndex == other.itemIndex;
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
		return Equals((Message_PlaceTrophyOnBoard)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), trophyBoardIndex, itemIndex);
	}

	public static bool operator ==(Message_PlaceTrophyOnBoard left, Message_PlaceTrophyOnBoard right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_PlaceTrophyOnBoard left, Message_PlaceTrophyOnBoard right)
	{
		return !object.Equals(left, right);
	}

	public Message_PlaceTrophyOnBoard()
	{
	}

	public Message_PlaceTrophyOnBoard(Messages p_messageType, Network_UserId p_steamID, MonoBehaviour_Network p_behavior, uint p_trophyBoardIndex, int p_itemIndex)
		: base(p_messageType, p_behavior, p_trophyBoardIndex, p_steamID)
	{
		itemIndex = p_itemIndex;
		trophyBoardIndex = p_trophyBoardIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in trophyBoardIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in itemIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out trophyBoardIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out itemIndex, default(FastBufferWriter.ForPrimitives));
	}
}
