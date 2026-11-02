using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_WorldEvent : SerializableBehaviour
{
	public bool isActive;

	public float progress;

	public float inactivityProgress;

	public SO_WorldEvent.WorldEventType eventType;

	public uint[] spawnedEntitiesObjectIndexes;

	protected bool Equals(RGD_WorldEvent other)
	{
		if (spawnedEntitiesObjectIndexes == null && other.spawnedEntitiesObjectIndexes != null)
		{
			return false;
		}
		if (spawnedEntitiesObjectIndexes != null && other.spawnedEntitiesObjectIndexes == null)
		{
			return false;
		}
		if (isActive == other.isActive && progress.Equals(other.progress) && inactivityProgress.Equals(other.inactivityProgress) && eventType == other.eventType)
		{
			if (spawnedEntitiesObjectIndexes != null || other.spawnedEntitiesObjectIndexes != null)
			{
				return Enumerable.SequenceEqual(spawnedEntitiesObjectIndexes, other.spawnedEntitiesObjectIndexes);
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
		return Equals((RGD_WorldEvent)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(isActive, progress, inactivityProgress, (int)eventType, spawnedEntitiesObjectIndexes);
	}

	public static bool operator ==(RGD_WorldEvent left, RGD_WorldEvent right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_WorldEvent left, RGD_WorldEvent right)
	{
		return !object.Equals(left, right);
	}

	public RGD_WorldEvent()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in isActive, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in progress, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in inactivityProgress, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in eventType, default(FastBufferWriter.ForEnums));
		writer.WriteValueSafe<int>((spawnedEntitiesObjectIndexes != null) ? spawnedEntitiesObjectIndexes.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (spawnedEntitiesObjectIndexes != null)
		{
			uint[] array = spawnedEntitiesObjectIndexes;
			for (int i = 0; i < array.Length; i++)
			{
				uint value = array[i];
				writer.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out isActive, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out progress, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out inactivityProgress, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out eventType, default(FastBufferWriter.ForEnums));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			spawnedEntitiesObjectIndexes = new uint[value];
			for (int i = 0; i < value; i++)
			{
				reader.ReadValue(out spawnedEntitiesObjectIndexes[i], default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public RGD_WorldEvent(WorldEvent we)
	{
		isActive = we.isActive;
		progress = we.progress;
		inactivityProgress = we.inactivityProgress;
		eventType = we.eventData.worldEventType;
		List<uint> list = new List<uint>();
		foreach (MonoBehaviour_Network spawnedEntite in we.spawnedEntites)
		{
			list.Add(spawnedEntite.ObjectIndex);
		}
		spawnedEntitiesObjectIndexes = ((list.Count > 0) ? list.ToArray() : null);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("IsActive", isActive);
		info.AddValue("Progress", progress);
		info.AddValue("InactivityProgress", inactivityProgress);
		info.AddValue("EventType", eventType);
	}

	public RGD_WorldEvent(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "IsActive":
				isActive = Helper.ObjToValue<bool>(item.Value);
				break;
			case "Progress":
				progress = Helper.ObjToValue<float>(item.Value);
				break;
			case "InactivityProgress":
				inactivityProgress = Helper.ObjToValue<float>(item.Value);
				break;
			case "EventType":
				eventType = Helper.ObjToValue<SO_WorldEvent.WorldEventType>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		isActive = false;
		progress = 0f;
		inactivityProgress = 0f;
		eventType = SO_WorldEvent.WorldEventType.Undefined;
	}
}
