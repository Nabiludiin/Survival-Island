using System;
using Unity.Netcode;

[Serializable]
public class Message_ProgressQuest : Message_NetworkBehaviour
{
	public int questIndex = 1;

	public int questProgress;

	protected bool Equals(Message_ProgressQuest other)
	{
		if (Equals((Message_NetworkBehaviour)other) && questIndex == other.questIndex)
		{
			return questProgress == other.questProgress;
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
		return Equals((Message_ProgressQuest)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), questIndex, questProgress);
	}

	public static bool operator ==(Message_ProgressQuest left, Message_ProgressQuest right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ProgressQuest left, Message_ProgressQuest right)
	{
		return !object.Equals(left, right);
	}

	public Message_ProgressQuest()
	{
	}

	public Message_ProgressQuest(Messages type, MonoBehaviour_Network behaviour, int questIndex, int questProgress)
		: base(type, behaviour)
	{
		this.questIndex = questIndex;
		this.questProgress = questProgress;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in questIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in questProgress, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out questIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out questProgress, default(FastBufferWriter.ForPrimitives));
	}
}
