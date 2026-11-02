using UnityEngine;

public class LandmarkItem_TangaroaCockpitLaunch : LandmarkItem
{
	public override RGD_LandmarkItem GetSave()
	{
		TangaroaCockpitLaunch component = GetComponent<TangaroaCockpitLaunch>();
		if (component != null)
		{
			return component.GetRGDSave();
		}
		Debug.LogError("Can't save tangaroa cockpit, could not find TangaroaCockpitLaunch.cs next to landmarkitem");
		return null;
	}
}
