using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_TreasurePoint : RGD
{
	public RGD_TreasurePoint_Networked rgdNetData;

	public RGD_TreasurePoint(RGDType type, RGDSortingOrder sortingOrder, TreasurePoint treasurePoint)
		: base(type, sortingOrder)
	{
		if (treasurePoint != null)
		{
			rgdNetData = new RGD_TreasurePoint_Networked(treasurePoint);
		}
	}

	public virtual TreasurePoint CreateAndRestoreTreasurePoint(TreasurePointManager treasurePointManager)
	{
		return rgdNetData.RestoreTreasurePoint(treasurePointManager);
	}

	public virtual TreasurePoint RestoreTreasurePoint(TreasurePoint tp)
	{
		return rgdNetData.RestoreTreasurePoint(tp);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDNetData", rgdNetData);
	}

	public RGD_TreasurePoint(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RGDNetData")
			{
				rgdNetData = Helper.ObjToValue<RGD_TreasurePoint_Networked>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdNetData = null;
	}
}
