using System;
using Unity.Netcode;

[Serializable]
public class Message_ResearchTable_Create : Message_NetworkBehaviour_SteamID
{
	public RGD_Slot rgdSlot;

	protected bool Equals(Message_ResearchTable_Create other)
	{
		if (Equals((Message_NetworkBehaviour_SteamID)other))
		{
			return object.Equals(rgdSlot, other.rgdSlot);
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
		return Equals((Message_ResearchTable_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdSlot);
	}

	public static bool operator ==(Message_ResearchTable_Create left, Message_ResearchTable_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ResearchTable_Create left, Message_ResearchTable_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_ResearchTable_Create(Messages type, MonoBehaviour_Network behaviour, Network_UserId activePlayer, ResearchTable table)
		: base(type, behaviour, activePlayer)
	{
		rgdSlot = new RGD_Slot(table.Slot, 0);
	}

	public Message_ResearchTable_Create()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdSlot.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdSlot = new RGD_Slot();
		rgdSlot.DeserializeFast(reader);
	}
}
