using System;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_ClawChallenge : RGD_LandmarkItem
{
	public RGD_ClawChallengeLiftableObject[] rgdLiftableObjects;

	public int liftedUniqueIndex;

	public int clawSlotIndexX;

	public int clawSlotIndexZ;

	protected bool Equals(RGD_LandmarkItem_ClawChallenge other)
	{
		if (Equals((RGD_LandmarkItem)other) && Enumerable.SequenceEqual(rgdLiftableObjects, other.rgdLiftableObjects) && liftedUniqueIndex == other.liftedUniqueIndex && clawSlotIndexX == other.clawSlotIndexX)
		{
			return clawSlotIndexZ == other.clawSlotIndexZ;
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
		return Equals((RGD_LandmarkItem_ClawChallenge)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdLiftableObjects, liftedUniqueIndex, clawSlotIndexX, clawSlotIndexZ);
	}

	public static bool operator ==(RGD_LandmarkItem_ClawChallenge left, RGD_LandmarkItem_ClawChallenge right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_ClawChallenge left, RGD_LandmarkItem_ClawChallenge right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(rgdLiftableObjects.Length, default(FastBufferWriter.ForPrimitives));
		RGD_ClawChallengeLiftableObject[] array = rgdLiftableObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SerializeFast(writer);
		}
		writer.WriteValueSafe(in liftedUniqueIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in clawSlotIndexX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in clawSlotIndexZ, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		rgdLiftableObjects = new RGD_ClawChallengeLiftableObject[value];
		for (int i = 0; i < value; i++)
		{
			rgdLiftableObjects[i] = new RGD_ClawChallengeLiftableObject();
			rgdLiftableObjects[i].DeserializeFast(reader);
		}
		reader.ReadValue(out liftedUniqueIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out clawSlotIndexX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out clawSlotIndexZ, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_ClawChallenge()
	{
	}

	public RGD_LandmarkItem_ClawChallenge(LandmarkItem landmarkItem)
		: base(landmarkItem)
	{
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		ClawChallenge component = landmarkItem.GetComponent<ClawChallenge>();
		if (component != null)
		{
			component.RestoreRGDSave(this);
		}
		else
		{
			Debug.LogError("Can't restore RGD_LandmarkItem_ClawChallenge, could not find script next to landmarkitem");
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Objects", rgdLiftableObjects);
		info.AddValue("LiftableIndex", liftedUniqueIndex);
		info.AddValue("ClawSlotIndexX", clawSlotIndexX);
		info.AddValue("ClawSlotIndexZ", clawSlotIndexZ);
	}

	public RGD_LandmarkItem_ClawChallenge(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Objects":
				rgdLiftableObjects = (RGD_ClawChallengeLiftableObject[])item.Value;
				break;
			case "LiftableIndex":
				liftedUniqueIndex = Helper.ObjToValue<int>(item.Value);
				break;
			case "ClawSlotIndexX":
				clawSlotIndexX = Helper.ObjToValue<int>(item.Value);
				break;
			case "ClawSlotIndexZ":
				clawSlotIndexZ = Helper.ObjToValue<int>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdLiftableObjects = null;
		liftedUniqueIndex = -1;
		clawSlotIndexX = (clawSlotIndexZ = -1);
	}
}
