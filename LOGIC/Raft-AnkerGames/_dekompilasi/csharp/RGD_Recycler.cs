using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Recycler : RGD_Extractor
{
	public RGD_Battery RGDBattery;

	protected bool Equals(RGD_Recycler other)
	{
		if (Equals((RGD_Extractor)other))
		{
			return object.Equals(RGDBattery, other.RGDBattery);
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
		return Equals((RGD_Recycler)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), RGDBattery);
	}

	public static bool operator ==(RGD_Recycler left, RGD_Recycler right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Recycler left, RGD_Recycler right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<bool>(RGDBattery != null, default(FastBufferWriter.ForPrimitives));
		if (RGDBattery != null)
		{
			RGDBattery.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		bool value = false;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value)
		{
			RGDBattery = new RGD_Battery();
			RGDBattery.DeserializeFast(reader);
		}
	}

	public RGD_Recycler()
	{
	}

	public RGD_Recycler(Placeable_RecyclerExtractor recycler)
		: base(recycler)
	{
		RGDBattery = (recycler.battery.BatterySlotIsEmpty ? null : new RGD_Battery(recycler.battery));
	}

	public override void RestoreExtractor(Placeable_Extractor extractor)
	{
		Placeable_RecyclerExtractor placeable_RecyclerExtractor = extractor as Placeable_RecyclerExtractor;
		if (placeable_RecyclerExtractor == null)
		{
			Debug.Log("Attempted to load in reycler, but it was null");
			return;
		}
		base.RestoreExtractor(placeable_RecyclerExtractor);
		if (RGDBattery != null)
		{
			RGDBattery.RestoreBattery(placeable_RecyclerExtractor.battery);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDBattery", RGDBattery);
	}

	public RGD_Recycler(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RGDBattery")
			{
				RGDBattery = (RGD_Battery)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		RGDBattery = null;
	}
}
