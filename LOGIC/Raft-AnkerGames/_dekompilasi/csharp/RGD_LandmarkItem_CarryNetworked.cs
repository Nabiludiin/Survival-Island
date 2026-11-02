using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_CarryNetworked : RGD_LandmarkItem
{
	public RGD_CarryNetwork rgdCarryNetwork;

	protected bool Equals(RGD_LandmarkItem_CarryNetworked other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return object.Equals(rgdCarryNetwork, other.rgdCarryNetwork);
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
		return Equals((RGD_LandmarkItem_CarryNetworked)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdCarryNetwork);
	}

	public static bool operator ==(RGD_LandmarkItem_CarryNetworked left, RGD_LandmarkItem_CarryNetworked right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_CarryNetworked left, RGD_LandmarkItem_CarryNetworked right)
	{
		return !object.Equals(left, right);
	}

	public RGD_LandmarkItem_CarryNetworked()
	{
	}

	public RGD_LandmarkItem_CarryNetworked(LandmarkItem landmarkItem, CarryNetworked carryNetworked)
		: base(landmarkItem)
	{
		rgdCarryNetwork = new RGD_CarryNetwork(carryNetworked);
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		if (landmarkItem is LandmarkItem_CarryNetworked)
		{
			LandmarkItem_CarryNetworked landmarkItem_CarryNetworked = landmarkItem as LandmarkItem_CarryNetworked;
			if (landmarkItem_CarryNetworked.carryNetworked != null && rgdCarryNetwork != null)
			{
				rgdCarryNetwork.RestoreCarryNetworked(landmarkItem_CarryNetworked.carryNetworked);
			}
		}
		else
		{
			Debug.LogError("Can't restore RGD_LandmarkItem_CarryNetworked");
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CarryNetwork", rgdCarryNetwork);
	}

	public RGD_LandmarkItem_CarryNetworked(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "CarryNetwork")
			{
				rgdCarryNetwork = (RGD_CarryNetwork)item.Value;
			}
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdCarryNetwork.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdCarryNetwork = new RGD_CarryNetwork();
		rgdCarryNetwork.DeserializeFast(reader);
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdCarryNetwork = null;
	}
}
