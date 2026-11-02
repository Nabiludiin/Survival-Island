using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_TangaroaCockpitLaunch : RGD_LandmarkItem
{
	[SerializeField]
	private int x;

	[SerializeField]
	private int z;

	public float TargetLocalX
	{
		get
		{
			return (float)x / 100f;
		}
		set
		{
			x = (int)(value * 100f);
		}
	}

	public float TargetLocalZ
	{
		get
		{
			return (float)z / 100f;
		}
		set
		{
			z = (int)(value * 100f);
		}
	}

	protected bool Equals(RGD_LandmarkItem_TangaroaCockpitLaunch other)
	{
		if (Equals((RGD_LandmarkItem)other) && x == other.x)
		{
			return z == other.z;
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
		return Equals((RGD_LandmarkItem_TangaroaCockpitLaunch)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), x, z);
	}

	public static bool operator ==(RGD_LandmarkItem_TangaroaCockpitLaunch left, RGD_LandmarkItem_TangaroaCockpitLaunch right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_TangaroaCockpitLaunch left, RGD_LandmarkItem_TangaroaCockpitLaunch right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in z, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_TangaroaCockpitLaunch()
	{
	}

	public RGD_LandmarkItem_TangaroaCockpitLaunch(LandmarkItem landmarkItem, float targetLocalPositionX, float targetLocalPositionZ)
		: base(landmarkItem)
	{
		TargetLocalX = targetLocalPositionX;
		TargetLocalZ = targetLocalPositionZ;
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		TangaroaCockpitLaunch component = landmarkItem.GetComponent<TangaroaCockpitLaunch>();
		if (component != null)
		{
			component.RestoreRGDSave(this);
		}
		else
		{
			Debug.LogError("Can't restore RGD_LandmarkItem_TangaroaCockpitLaunch, could not find script next to landmarkitem");
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("TargetLocalX", x);
		info.AddValue("TargetLocalZ", z);
	}

	public RGD_LandmarkItem_TangaroaCockpitLaunch(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "TargetLocalX"))
			{
				if (name == "TargetLocalZ")
				{
					z = Helper.ObjToValue<int>(item.Value);
				}
			}
			else
			{
				x = Helper.ObjToValue<int>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		x = (z = 0);
	}
}
