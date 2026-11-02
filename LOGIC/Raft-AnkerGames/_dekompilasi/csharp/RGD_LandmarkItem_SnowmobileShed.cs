using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_LandmarkItem_SnowmobileShed : RGD_LandmarkItem
{
	public RGD_Snowmobile rgdSnowmobile;

	protected bool Equals(RGD_LandmarkItem_SnowmobileShed other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return object.Equals(rgdSnowmobile, other.rgdSnowmobile);
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
		return Equals((RGD_LandmarkItem_SnowmobileShed)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdSnowmobile);
	}

	public static bool operator ==(RGD_LandmarkItem_SnowmobileShed left, RGD_LandmarkItem_SnowmobileShed right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_SnowmobileShed left, RGD_LandmarkItem_SnowmobileShed right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<bool>(rgdSnowmobile != null, default(FastBufferWriter.ForPrimitives));
		if (rgdSnowmobile != null)
		{
			rgdSnowmobile.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out bool value, default(FastBufferWriter.ForPrimitives));
		if (value)
		{
			rgdSnowmobile = new RGD_Snowmobile();
			rgdSnowmobile.DeserializeFast(reader);
		}
	}

	public RGD_LandmarkItem_SnowmobileShed()
	{
	}

	public RGD_LandmarkItem_SnowmobileShed(LandmarkItem landmarkItem)
		: base(landmarkItem)
	{
		SnowmobileShed component = landmarkItem.GetComponent<SnowmobileShed>();
		if (component != null && component.spawnedSnowmobile != null)
		{
			rgdSnowmobile = component.spawnedSnowmobile.GetRGD();
		}
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		SnowmobileShed component = landmarkItem.GetComponent<SnowmobileShed>();
		if (component != null)
		{
			component.Restore(rgdSnowmobile);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Snowmobile", rgdSnowmobile);
	}

	public RGD_LandmarkItem_SnowmobileShed(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Snowmobile")
			{
				rgdSnowmobile = (RGD_Snowmobile)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdSnowmobile = null;
	}
}
