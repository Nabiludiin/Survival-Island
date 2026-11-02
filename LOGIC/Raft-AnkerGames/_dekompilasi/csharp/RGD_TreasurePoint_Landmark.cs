using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class RGD_TreasurePoint_Landmark : RGD_TreasurePoint
{
	public RGD_TreasurePoint_Landmark(RGDType type, RGDSortingOrder sortingOrder, uint landmarkObjectIndex, TreasurePoint_Landmark treasurePoint)
		: base(type, sortingOrder, null)
	{
		rgdNetData = new RGD_TreasurePoint_Landmark_Networked(landmarkObjectIndex, treasurePoint);
	}

	public override TreasurePoint CreateAndRestoreTreasurePoint(TreasurePointManager treasurePointManager)
	{
		if (rgdNetData == null)
		{
			return null;
		}
		RGD_TreasurePoint_Landmark_Networked landmarkNetData = rgdNetData as RGD_TreasurePoint_Landmark_Networked;
		if (landmarkNetData == null)
		{
			return null;
		}
		Landmark landmark = WorldManager.AllLandmarks.Find((Landmark l) => l.uniqueLandmarkIndex == landmarkNetData.uniqueLandmarkIndex);
		if (landmark != null)
		{
			TreasurePoint treasurePoint = treasurePointManager.AddTreasurePoint(landmarkNetData.tpObjectIndex, landmarkNetData.tpPickupObjectIndex, landmark.transform, landmarkNetData.position, landmarkNetData.uniqueTreasureIndex);
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
	}

	public RGD_TreasurePoint_Landmark(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RGDNetData")
			{
				rgdNetData = Helper.ObjToValue<RGD_TreasurePoint_Landmark_Networked>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
	}
}
