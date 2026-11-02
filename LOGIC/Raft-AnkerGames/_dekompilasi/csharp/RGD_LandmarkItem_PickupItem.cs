using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_LandmarkItem_PickupItem : RGD_LandmarkItem
{
	public int yieldCount;

	protected bool Equals(RGD_LandmarkItem_PickupItem other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return yieldCount == other.yieldCount;
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
		return Equals((RGD_LandmarkItem_PickupItem)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), yieldCount);
	}

	public static bool operator ==(RGD_LandmarkItem_PickupItem left, RGD_LandmarkItem_PickupItem right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_PickupItem left, RGD_LandmarkItem_PickupItem right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in yieldCount, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out yieldCount, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_PickupItem()
	{
	}

	public RGD_LandmarkItem_PickupItem(LandmarkItem_PickupItem landmarkItem)
		: base(landmarkItem)
	{
		PickupItem pickupItem = landmarkItem.pickupItem;
		if (pickupItem == null)
		{
			pickupItem = landmarkItem.connectedBehaviourID.GetComponent<PickupItem>();
		}
		if (pickupItem.yieldHandler != null)
		{
			yieldCount = pickupItem.yieldHandler.Yield.Count;
		}
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		PickupItem component = landmarkItem.connectedBehaviourID.GetComponent<PickupItem>();
		if (!(component != null))
		{
			return;
		}
		if (component.networkID != null)
		{
			component.networkID.OnRestored();
		}
		if (yieldCount > 0 && component.yieldHandler != null)
		{
			while (component.yieldHandler.Yield.Count > yieldCount)
			{
				component.yieldHandler.Yield.RemoveAt(component.yieldHandler.Yield.Count - 1);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("YieldCount", yieldCount);
	}

	public RGD_LandmarkItem_PickupItem(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "YieldCount")
			{
				yieldCount = Helper.ObjToValue<int>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		yieldCount = -1;
	}
}
