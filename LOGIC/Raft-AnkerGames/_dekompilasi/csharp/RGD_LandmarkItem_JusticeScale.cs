using System;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_JusticeScale : RGD_LandmarkItem
{
	public int[] weightSlots;

	protected bool Equals(RGD_LandmarkItem_JusticeScale other)
	{
		if (weightSlots == null && other.weightSlots != null)
		{
			return false;
		}
		if (weightSlots != null && other.weightSlots == null)
		{
			return false;
		}
		if (Equals((RGD_LandmarkItem)other))
		{
			if (weightSlots != null || other.weightSlots != null)
			{
				return Enumerable.SequenceEqual(weightSlots, other.weightSlots);
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
		return Equals((RGD_LandmarkItem_JusticeScale)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), weightSlots);
	}

	public static bool operator ==(RGD_LandmarkItem_JusticeScale left, RGD_LandmarkItem_JusticeScale right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_JusticeScale left, RGD_LandmarkItem_JusticeScale right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((weightSlots != null) ? weightSlots.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (weightSlots != null)
		{
			int[] array = weightSlots;
			for (int i = 0; i < array.Length; i++)
			{
				int value = array[i];
				writer.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			weightSlots = new int[value];
			for (int i = 0; i < value; i++)
			{
				reader.ReadValue(out weightSlots[i], default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public RGD_LandmarkItem_JusticeScale()
	{
	}

	public RGD_LandmarkItem_JusticeScale(LandmarkItem landmarkItem)
		: base(landmarkItem)
	{
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		JusticeScale component = landmarkItem.GetComponent<JusticeScale>();
		if (component != null)
		{
			component.RestoreRGDSave(this);
		}
		else
		{
			Debug.LogError("Can't restore RGD_LandmarkItem_JusticeScale, could not find script next to landmarkitem");
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("WeightSlots", weightSlots);
	}

	public RGD_LandmarkItem_JusticeScale(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "WeightSlots")
			{
				weightSlots = (int[])item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		weightSlots = null;
	}
}
