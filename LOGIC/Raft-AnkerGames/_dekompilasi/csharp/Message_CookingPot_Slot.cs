using System;
using Unity.Netcode;

[Serializable]
public class Message_CookingPot_Slot : Message_NetworkBehaviour_ID_SteamID
{
	public int slotIndex;

	public RGD_ItemInstance rgdItemInstance;

	protected bool Equals(Message_CookingPot_Slot other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other) && slotIndex == other.slotIndex)
		{
			return object.Equals(rgdItemInstance, other.rgdItemInstance);
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
		return Equals((Message_CookingPot_Slot)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), slotIndex, rgdItemInstance);
	}

	public static bool operator ==(Message_CookingPot_Slot left, Message_CookingPot_Slot right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_CookingPot_Slot left, Message_CookingPot_Slot right)
	{
		return !object.Equals(left, right);
	}

	public Message_CookingPot_Slot()
	{
	}

	public Message_CookingPot_Slot(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Network_UserId steamID, int slotIndex, ItemInstance itemInstance)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		this.slotIndex = slotIndex;
		rgdItemInstance = new RGD_ItemInstance(itemInstance);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in slotIndex, default(FastBufferWriter.ForPrimitives));
		rgdItemInstance.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out slotIndex, default(FastBufferWriter.ForPrimitives));
		rgdItemInstance = new RGD_ItemInstance();
		rgdItemInstance.DeserializeFast(reader);
	}
}
