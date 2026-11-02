using UnityEngine;

public class TreasurePoint_Landmark : TreasurePoint
{
	public Landmark connectedLandmark;

	public override RGD Serialize_Save()
	{
		if (connectedLandmark == null)
		{
			Debug.Log("Connected landmark is null in treasurepoint landmark, save");
			return null;
		}
		return new RGD_TreasurePoint_Landmark(RGDType.TreasurePoint_Landmark, RGDSortingOrder.TreasurePoints, connectedLandmark.uniqueLandmarkIndex, this);
	}

	public override void Initialize(uint objectIndex, uint objectIndexPickup, Transform parent)
	{
		base.Initialize(objectIndex, objectIndexPickup, parent);
		if (parent != null)
		{
			connectedLandmark = parent.GetComponent<Landmark>();
		}
	}

	public override int GetParentObjectIndex()
	{
		if (connectedLandmark != null)
		{
			return (int)connectedLandmark.uniqueLandmarkIndex;
		}
		Debug.LogError("TreasurePoint Landmark has no connectedLandmark");
		return base.GetParentObjectIndex();
	}

	public override RGD_TreasurePoint_Networked GetSave()
	{
		if (connectedLandmark == null)
		{
			Debug.Log("connected landmark null in getsave");
			return null;
		}
		return new RGD_TreasurePoint_Landmark_Networked(connectedLandmark.uniqueLandmarkIndex, this);
	}
}
