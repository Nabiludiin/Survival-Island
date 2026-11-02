using System;
using Unity.Netcode;

[Serializable]
public class Message_QuestEventInteract : Message_NetworkBehaviour_ID_SteamID
{
	public int interactIndex;

	public bool interactSuccess;

	public bool hasInventoryRequirement;

	protected bool Equals(Message_QuestEventInteract other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other) && interactIndex == other.interactIndex && interactSuccess == other.interactSuccess)
		{
			return hasInventoryRequirement == other.hasInventoryRequirement;
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
		return Equals((Message_QuestEventInteract)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), interactIndex, interactSuccess, hasInventoryRequirement);
	}

	public static bool operator ==(Message_QuestEventInteract left, Message_QuestEventInteract right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_QuestEventInteract left, Message_QuestEventInteract right)
	{
		return !object.Equals(left, right);
	}

	public Message_QuestEventInteract()
	{
	}

	public Message_QuestEventInteract(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, int p_interactIndex, bool p_interactSuccess, bool p_hasInventoryRequirement, ulong steamID)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		interactIndex = p_interactIndex;
		interactSuccess = p_interactSuccess;
		hasInventoryRequirement = p_hasInventoryRequirement;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(6))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in interactIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in interactSuccess, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in hasInventoryRequirement, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out interactIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out interactSuccess, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out hasInventoryRequirement, default(FastBufferWriter.ForPrimitives));
	}
}
