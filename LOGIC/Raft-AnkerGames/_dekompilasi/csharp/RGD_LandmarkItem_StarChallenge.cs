using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_StarChallenge : RGD_LandmarkItem
{
	public int xIndex;

	public int yIndex;

	protected bool Equals(RGD_LandmarkItem_StarChallenge other)
	{
		if (Equals((RGD_LandmarkItem)other) && xIndex == other.xIndex)
		{
			return yIndex == other.yIndex;
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
		return Equals((RGD_LandmarkItem_StarChallenge)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), xIndex, yIndex);
	}

	public static bool operator ==(RGD_LandmarkItem_StarChallenge left, RGD_LandmarkItem_StarChallenge right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_StarChallenge left, RGD_LandmarkItem_StarChallenge right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in xIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in yIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out xIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_StarChallenge()
	{
	}

	public RGD_LandmarkItem_StarChallenge(LandmarkItem landmarkItem)
		: base(landmarkItem)
	{
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		StarChallenge component = landmarkItem.GetComponent<StarChallenge>();
		if (component != null)
		{
			component.RestoreRGDSave(this);
		}
		else
		{
			Debug.LogError("Can't restore RGD_LandmarkItem_StarChallenge, could not find script next to landmarkitem");
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("XIndex", xIndex);
		info.AddValue("YIndex", yIndex);
	}

	public RGD_LandmarkItem_StarChallenge(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "XIndex"))
			{
				if (name == "YIndex")
				{
					yIndex = Helper.ObjToValue<int>(item.Value);
				}
			}
			else
			{
				xIndex = Helper.ObjToValue<int>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		xIndex = 0;
		yIndex = 0;
	}
}
