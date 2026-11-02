using System;
using Unity.Netcode;

[Serializable]
public class Message_FinishQuest : Message_NetworkBehaviour
{
	public int questIndex = -1;

	protected bool Equals(Message_FinishQuest other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return questIndex == other.questIndex;
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
		return Equals((Message_FinishQuest)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), questIndex);
	}

	public static bool operator ==(Message_FinishQuest left, Message_FinishQuest right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_FinishQuest left, Message_FinishQuest right)
	{
		return !object.Equals(left, right);
	}

	public Message_FinishQuest()
	{
	}

	public Message_FinishQuest(Messages type, MonoBehaviour_Network behaviour, int questIndex)
		: base(type, behaviour)
	{
		this.questIndex = questIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in questIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out questIndex, default(FastBufferWriter.ForPrimitives));
	}
}
