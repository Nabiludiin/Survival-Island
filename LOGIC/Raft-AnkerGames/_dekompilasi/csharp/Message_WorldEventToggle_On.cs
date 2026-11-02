using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_WorldEventToggle_On : Message_NetworkBehaviour_ID
{
	public int worldEventTypeIndex;

	public List<uint> spawnedEntitiesIndexes;

	protected bool Equals(Message_WorldEventToggle_On other)
	{
		if (spawnedEntitiesIndexes == null && other.spawnedEntitiesIndexes != null)
		{
			return false;
		}
		if (spawnedEntitiesIndexes != null && other.spawnedEntitiesIndexes == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour_ID)other) && worldEventTypeIndex == other.worldEventTypeIndex)
		{
			if (spawnedEntitiesIndexes != null || other.spawnedEntitiesIndexes != null)
			{
				return spawnedEntitiesIndexes.SequenceEqual(other.spawnedEntitiesIndexes);
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
		return Equals((Message_WorldEventToggle_On)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), worldEventTypeIndex, spawnedEntitiesIndexes);
	}

	public static bool operator ==(Message_WorldEventToggle_On left, Message_WorldEventToggle_On right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_WorldEventToggle_On left, Message_WorldEventToggle_On right)
	{
		return !object.Equals(left, right);
	}

	public Message_WorldEventToggle_On()
	{
	}

	public Message_WorldEventToggle_On(Messages type, MonoBehaviour_Network behaviour, uint networkID, List<uint> spawnedEntities_ObjectIndexes, SO_WorldEvent.WorldEventType weType)
		: base(type, behaviour, networkID)
	{
		spawnedEntitiesIndexes = spawnedEntities_ObjectIndexes;
		worldEventTypeIndex = (int)weType;
	}

	public Message_WorldEventToggle_On(Messages type, MonoBehaviour_Network behaviour, uint networkID, bool value, List<MonoBehaviour_Network> spawnedEntities, SO_WorldEvent.WorldEventType weType)
		: base(type, behaviour, networkID)
	{
		spawnedEntitiesIndexes = new List<uint>();
		foreach (AI_NetworkBehaviour spawnedEntity in spawnedEntities)
		{
			spawnedEntitiesIndexes.Add(spawnedEntity.ObjectIndex);
		}
		worldEventTypeIndex = (int)weType;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in worldEventTypeIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<int>(spawnedEntitiesIndexes.Count, default(FastBufferWriter.ForPrimitives));
		foreach (uint spawnedEntitiesIndex in spawnedEntitiesIndexes)
		{
			writer.WriteValueSafe<uint>(spawnedEntitiesIndex, default(FastBufferWriter.ForPrimitives));
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out worldEventTypeIndex, default(FastBufferWriter.ForPrimitives));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		spawnedEntitiesIndexes = new List<uint>(value);
		for (int i = 0; i < value; i++)
		{
			reader.ReadValue(out uint value2, default(FastBufferWriter.ForPrimitives));
			spawnedEntitiesIndexes.Add(value2);
		}
	}
}
