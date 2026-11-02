using System.Collections.Generic;
using UnityEngine;

public class Fixer_Component_OneTimePickup : Fixer_Component_ABS
{
	public override bool DoCheck(Transform target)
	{
		bool result = true;
		Landmark component = target.GetComponent<Landmark>();
		LandmarkItem[] componentsInChildren = target.GetComponentsInChildren<LandmarkItem>(includeInactive: true);
		List<LandmarkItem> list = new List<LandmarkItem>();
		LandmarkItem[] array = componentsInChildren;
		foreach (LandmarkItem landmarkItem in array)
		{
			if (!landmarkItem.resetOnLandmarkReset)
			{
				list.Add(landmarkItem);
			}
		}
		if (component.containsNonResetableLandmarkItem != list.Count > 0)
		{
			Debug.LogError(component.transform.name + " is inconsistant with pickup results: " + GetPickupNames(list), component);
			result = false;
		}
		return result;
	}

	public override bool DoFix(Transform target)
	{
		return true;
	}

	private string GetPickupNames(List<LandmarkItem> pickups)
	{
		string text = string.Empty;
		foreach (LandmarkItem pickup in pickups)
		{
			text = text + pickup.transform.name + " ";
		}
		return text;
	}
}
