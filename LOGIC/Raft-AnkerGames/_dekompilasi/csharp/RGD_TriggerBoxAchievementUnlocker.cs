using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_TriggerBoxAchievementUnlocker : RGD_LandmarkItem
{
	public ulong triggersHitFlags;

	protected bool Equals(RGD_TriggerBoxAchievementUnlocker other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return triggersHitFlags == other.triggersHitFlags;
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
		return Equals((RGD_TriggerBoxAchievementUnlocker)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), triggersHitFlags);
	}

	public static bool operator ==(RGD_TriggerBoxAchievementUnlocker left, RGD_TriggerBoxAchievementUnlocker right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_TriggerBoxAchievementUnlocker left, RGD_TriggerBoxAchievementUnlocker right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in triggersHitFlags, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out triggersHitFlags, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_TriggerBoxAchievementUnlocker()
	{
	}

	public RGD_TriggerBoxAchievementUnlocker(TriggerBoxAchievementUnlocker landmarkItem)
		: base(landmarkItem)
	{
		triggersHitFlags = landmarkItem.triggersHitFlags;
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		if (landmarkItem is TriggerBoxAchievementUnlocker)
		{
			(landmarkItem as TriggerBoxAchievementUnlocker).Restore(this);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("TriggersHitFlags", triggersHitFlags);
	}

	public RGD_TriggerBoxAchievementUnlocker(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "TriggersHitFlags")
			{
				triggersHitFlags = Helper.ObjToValue<ulong>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		triggersHitFlags = 0uL;
	}
}
