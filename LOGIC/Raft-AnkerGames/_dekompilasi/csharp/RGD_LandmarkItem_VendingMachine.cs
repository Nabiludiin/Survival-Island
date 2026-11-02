using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_LandmarkItem_VendingMachine : RGD_LandmarkItem
{
	public int lastPurchaseButtonIndex = -1;

	protected bool Equals(RGD_LandmarkItem_VendingMachine other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return lastPurchaseButtonIndex == other.lastPurchaseButtonIndex;
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
		return Equals((RGD_LandmarkItem_VendingMachine)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), lastPurchaseButtonIndex);
	}

	public static bool operator ==(RGD_LandmarkItem_VendingMachine left, RGD_LandmarkItem_VendingMachine right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_VendingMachine left, RGD_LandmarkItem_VendingMachine right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in lastPurchaseButtonIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValueSafe(out lastPurchaseButtonIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_VendingMachine()
	{
	}

	public RGD_LandmarkItem_VendingMachine(LandmarkItem item)
		: base(item)
	{
		VendingMachine component = item.GetComponent<VendingMachine>();
		if (component != null)
		{
			lastPurchaseButtonIndex = component.LatestPurchaseButtonIndex;
		}
		else
		{
			lastPurchaseButtonIndex = -1;
		}
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		VendingMachine component = landmarkItem.GetComponent<VendingMachine>();
		if (component != null)
		{
			component.RestoreVendingMachine(this);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("LastBIndex", lastPurchaseButtonIndex);
	}

	public RGD_LandmarkItem_VendingMachine(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "LastBIndex")
			{
				lastPurchaseButtonIndex = (int)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		lastPurchaseButtonIndex = -1;
	}
}
