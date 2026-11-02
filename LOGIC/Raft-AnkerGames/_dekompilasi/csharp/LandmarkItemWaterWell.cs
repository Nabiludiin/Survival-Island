using UnityEngine;

public class LandmarkItemWaterWell : LandmarkItem
{
	public Landmark_WaterWell waterWell;

	public override RGD_LandmarkItem GetSave()
	{
		if (waterWell == null)
		{
			Debug.Log("Waterwell ref null in landmark item waterwell", gameObject);
			return null;
		}
		return new RGD_LandmarkItem_WaterWell(this, waterWell);
	}
}
