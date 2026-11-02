using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_LandmarkItem_CharacterUnlock : RGD_LandmarkItem
{
	public bool unlocked;

	protected bool Equals(RGD_LandmarkItem_CharacterUnlock other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return unlocked == other.unlocked;
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
		return Equals((RGD_LandmarkItem_CharacterUnlock)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), unlocked);
	}

	public static bool operator ==(RGD_LandmarkItem_CharacterUnlock left, RGD_LandmarkItem_CharacterUnlock right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_CharacterUnlock left, RGD_LandmarkItem_CharacterUnlock right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in unlocked, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out unlocked, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_CharacterUnlock()
	{
	}

	public RGD_LandmarkItem_CharacterUnlock(LandmarkItem_CharacterUnlock landmarkItem, CharacterUnlock characterUnlock)
		: base(landmarkItem)
	{
		unlocked = characterUnlock.Unlocked;
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		if (landmarkItem is LandmarkItem_CharacterUnlock)
		{
			(landmarkItem as LandmarkItem_CharacterUnlock).Restore(this);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("unlocked", unlocked);
	}

	public RGD_LandmarkItem_CharacterUnlock(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "unlocked")
			{
				unlocked = Helper.ObjToValue<bool>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		unlocked = false;
	}
}
