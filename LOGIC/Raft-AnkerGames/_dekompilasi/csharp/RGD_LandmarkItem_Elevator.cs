using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_Elevator : RGD_LandmarkItem
{
	public RGD_Elevator rgdElevator;

	protected bool Equals(RGD_LandmarkItem_Elevator other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return object.Equals(rgdElevator, other.rgdElevator);
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
		return Equals((RGD_LandmarkItem_Elevator)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdElevator);
	}

	public static bool operator ==(RGD_LandmarkItem_Elevator left, RGD_LandmarkItem_Elevator right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_Elevator left, RGD_LandmarkItem_Elevator right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdElevator.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdElevator = new RGD_Elevator();
		rgdElevator.DeserializeFast(reader);
	}

	public RGD_LandmarkItem_Elevator()
	{
	}

	public RGD_LandmarkItem_Elevator(LandmarkItem landmarkItem)
		: base(landmarkItem)
	{
		Elevator component = landmarkItem.GetComponent<Elevator>();
		if (component != null)
		{
			rgdElevator = new RGD_Elevator(component);
		}
		else
		{
			Debug.LogError("Can't save elevator, could not find Elevator script next to landmarkitem " + landmarkItem.name);
		}
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		Elevator component = landmarkItem.GetComponent<Elevator>();
		if (component != null && rgdElevator != null)
		{
			component.RestoreRGD(rgdElevator);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDElevator", rgdElevator);
	}

	public RGD_LandmarkItem_Elevator(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RGDElevator")
			{
				rgdElevator = (RGD_Elevator)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdElevator = null;
	}
}
