using UnityEngine;

public class Fixer_Component_PickupTooCloseToEachOther : Fixer_Component_ABS
{
	public override bool DoCheck(Transform target)
	{
		bool result = true;
		target.GetComponent<Landmark>();
		LandmarkItem[] componentsInChildren = target.GetComponentsInChildren<LandmarkItem>(includeInactive: true);
		foreach (LandmarkItem landmarkItem in componentsInChildren)
		{
			foreach (LandmarkItem landmarkItem2 in componentsInChildren)
			{
				if (landmarkItem != landmarkItem2 && Vector3.Distance(landmarkItem.transform.position, landmarkItem2.transform.position) <= 0.01f)
				{
					Debug.Log(landmarkItem.name + " is too close to " + landmarkItem2.name, landmarkItem.gameObject);
					result = false;
				}
			}
		}
		Debug.Log("Looked at " + componentsInChildren.Length + " items, Success = " + result);
		return result;
	}

	public override bool DoFix(Transform target)
	{
		return true;
	}
}
