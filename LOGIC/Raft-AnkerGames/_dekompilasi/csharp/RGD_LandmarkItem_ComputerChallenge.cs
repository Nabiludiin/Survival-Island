using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_ComputerChallenge : RGD_LandmarkItem
{
	public bool complete;

	protected bool Equals(RGD_LandmarkItem_ComputerChallenge other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return complete == other.complete;
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
		return Equals((RGD_LandmarkItem_ComputerChallenge)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), complete);
	}

	public static bool operator ==(RGD_LandmarkItem_ComputerChallenge left, RGD_LandmarkItem_ComputerChallenge right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_ComputerChallenge left, RGD_LandmarkItem_ComputerChallenge right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in complete, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out complete, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_ComputerChallenge()
	{
	}

	public RGD_LandmarkItem_ComputerChallenge(LandmarkItem landmarkItem)
		: base(landmarkItem)
	{
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		ComputerChallenge component = landmarkItem.GetComponent<ComputerChallenge>();
		if (component != null)
		{
			component.RestoreRGDSave(this);
		}
		else
		{
			Debug.LogError("Can't restore RGD_LandmarkItem_ComputerChallenge, could not find script next to landmarkitem");
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Complete", complete);
	}

	public RGD_LandmarkItem_ComputerChallenge(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Complete")
			{
				complete = Helper.ObjToValue<bool>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		complete = false;
	}
}
