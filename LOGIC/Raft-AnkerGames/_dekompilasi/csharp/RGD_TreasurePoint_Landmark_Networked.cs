using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_TreasurePoint_Landmark_Networked : RGD_TreasurePoint_Networked
{
	public uint uniqueLandmarkIndex;

	protected bool Equals(RGD_TreasurePoint_Landmark_Networked other)
	{
		if (Equals((RGD_TreasurePoint_Networked)other))
		{
			return uniqueLandmarkIndex == other.uniqueLandmarkIndex;
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
		return Equals((RGD_TreasurePoint_Landmark_Networked)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), uniqueLandmarkIndex);
	}

	public static bool operator ==(RGD_TreasurePoint_Landmark_Networked left, RGD_TreasurePoint_Landmark_Networked right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_TreasurePoint_Landmark_Networked left, RGD_TreasurePoint_Landmark_Networked right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in uniqueLandmarkIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out uniqueLandmarkIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_TreasurePoint_Landmark_Networked()
	{
	}

	public RGD_TreasurePoint_Landmark_Networked(uint landmarkObjectIndex, TreasurePoint_Landmark treasurePoint)
		: base(treasurePoint)
	{
		uniqueLandmarkIndex = landmarkObjectIndex;
	}

	public override TreasurePoint RestoreTreasurePoint(TreasurePointManager treasurePointManager)
	{
		Landmark landmark = WorldManager.AllLandmarks.Find((Landmark l) => l.uniqueLandmarkIndex == uniqueLandmarkIndex);
		if (landmark != null)
		{
			TreasurePoint treasurePoint = treasurePointManager.AddTreasurePoint(tpObjectIndex, tpPickupObjectIndex, landmark.transform, position, uniqueTreasureIndex);
			RestoreTreasurePoint(treasurePoint);
			TreasurePoint_Landmark treasurePoint_Landmark = treasurePoint as TreasurePoint_Landmark;
			if (treasurePoint_Landmark != null)
			{
				treasurePoint_Landmark.connectedLandmark = landmark;
				return treasurePoint_Landmark;
			}
			Debug.Log("was unable to successfully cast treasurepoint to treasurepoint landmark in restore");
		}
		else
		{
			Debug.Log("Landmark was null in restoreTreasurePointLandmark");
		}
		return null;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("UniqueLandmarkIndex", uniqueLandmarkIndex);
	}

	public RGD_TreasurePoint_Landmark_Networked(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "UniqueLandmarkIndex")
			{
				uniqueLandmarkIndex = (uint)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		uniqueLandmarkIndex = 999u;
	}
}
