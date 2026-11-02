using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_BiofuelExtactor : RGD_Extractor
{
	[SerializeField]
	private RGD_Tank honeyTank;

	protected bool Equals(RGD_BiofuelExtactor other)
	{
		if (Equals((RGD_Extractor)other))
		{
			return object.Equals(honeyTank, other.honeyTank);
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
		return Equals((RGD_BiofuelExtactor)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), honeyTank);
	}

	public static bool operator ==(RGD_BiofuelExtactor left, RGD_BiofuelExtactor right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_BiofuelExtactor left, RGD_BiofuelExtactor right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		honeyTank.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		honeyTank = new RGD_Tank();
		honeyTank.DeserializeFast(reader);
	}

	public RGD_BiofuelExtactor()
	{
	}

	public RGD_BiofuelExtactor(Placeable_BiofuelExtractor bioFuelCreator)
		: base(bioFuelCreator)
	{
		honeyTank = new RGD_Tank(bioFuelCreator.honeyTank);
	}

	public override void RestoreExtractor(Placeable_Extractor bioFuelCreator)
	{
		Placeable_BiofuelExtractor placeable_BiofuelExtractor = bioFuelCreator as Placeable_BiofuelExtractor;
		if (placeable_BiofuelExtractor == null)
		{
			Debug.Log("Attempted to load in bioFuelCreator, but it was null");
			return;
		}
		base.RestoreExtractor(placeable_BiofuelExtractor);
		honeyTank.RestoreTank(placeable_BiofuelExtractor.honeyTank);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("HoneyTank", honeyTank);
	}

	public RGD_BiofuelExtactor(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "HoneyTank")
			{
				honeyTank = Helper.ObjToValue<RGD_Tank>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		honeyTank = null;
	}
}
