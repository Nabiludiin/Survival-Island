using System;
using Unity.Netcode;

[Serializable]
public class Message_ResearchTable_ResearchOrLearn : Message_NetworkBehaviour_SteamID
{
	public int itemIndex;

	protected bool Equals(Message_ResearchTable_ResearchOrLearn other)
	{
		if (Equals((Message_NetworkBehaviour_SteamID)other))
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
		return Equals((Message_ResearchTable_ResearchOrLearn)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), itemIndex);
	}

	public static bool operator ==(Message_ResearchTable_ResearchOrLearn left, Message_ResearchTable_ResearchOrLearn right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ResearchTable_ResearchOrLearn left, Message_ResearchTable_ResearchOrLearn right)
	{
		return !object.Equals(left, right);
	}

	public Message_ResearchTable_ResearchOrLearn()
	{
	}

	public Message_ResearchTable_ResearchOrLearn(Messages type, MonoBehaviour_Network behaviour, Network_UserId researcherID, int itemIndex)
		: base(type, behaviour, researcherID)
	{
		this.itemIndex = itemIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in itemIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out itemIndex, default(FastBufferWriter.ForPrimitives));
	}
}
