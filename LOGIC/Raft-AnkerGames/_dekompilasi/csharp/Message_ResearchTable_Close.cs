using System;
using Unity.Netcode;

[Serializable]
public class Message_ResearchTable_Close : Message_NetworkBehaviour
{
	public RGD_Slot rgdSlot;

	protected bool Equals(Message_ResearchTable_Close other)
	{
		if (Equals((Message_NetworkBehaviour)other))
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
		return Equals((Message_ResearchTable_Close)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdSlot);
	}

	public static bool operator ==(Message_ResearchTable_Close left, Message_ResearchTable_Close right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ResearchTable_Close left, Message_ResearchTable_Close right)
	{
		return !object.Equals(left, right);
	}

	public Message_ResearchTable_Close()
	{
	}

	public Message_ResearchTable_Close(Messages type, MonoBehaviour_Network behaviour, Slot tableSlot)
		: base(type, behaviour)
	{
		rgdSlot = new RGD_Slot(tableSlot, 0);
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
