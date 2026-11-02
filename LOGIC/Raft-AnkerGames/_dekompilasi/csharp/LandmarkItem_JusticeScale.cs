using UnityEngine;

public class LandmarkItem_JusticeScale : LandmarkItem
{
	[SerializeField]
	private JusticeScale justiceScale;

	public override RGD_LandmarkItem GetSave()
	{
		if (justiceScale != null)
		{
			return justiceScale.GetRGDSave();
		}
		Debug.LogError("Can't save Landmarkitem_JusticeScale, justiceScale reference is null");
		return null;
	}
}
