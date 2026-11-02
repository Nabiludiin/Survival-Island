using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_RestoreQuests : Message_NetworkBehaviour
{
	public int[] unlockedQuestIndexes;

	protected bool Equals(Message_RestoreQuests other)
	{
		if (unlockedQuestIndexes == null && other.unlockedQuestIndexes != null)
		{
			return false;
		}
		if (unlockedQuestIndexes != null && other.unlockedQuestIndexes == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (unlockedQuestIndexes != null || other.unlockedQuestIndexes != null)
			{
				return Enumerable.SequenceEqual(unlockedQuestIndexes, other.unlockedQuestIndexes);
			}
			return true;
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
		return Equals((Message_RestoreQuests)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), unlockedQuestIndexes);
	}

	public static bool operator ==(Message_RestoreQuests left, Message_RestoreQuests right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_RestoreQuests left, Message_RestoreQuests right)
	{
		return !object.Equals(left, right);
	}

	public Message_RestoreQuests()
	{
	}

	public Message_RestoreQuests(Messages type, MonoBehaviour_Network behaviour, int[] unlockedQuestIndexes)
		: base(type, behaviour)
	{
		this.unlockedQuestIndexes = unlockedQuestIndexes;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(unlockedQuestIndexes, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out unlockedQuestIndexes, default(FastBufferWriter.ForPrimitives));
	}
}
