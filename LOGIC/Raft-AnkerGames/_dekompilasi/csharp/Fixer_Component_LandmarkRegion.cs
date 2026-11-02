using UnityEngine;

public class Fixer_Component_LandmarkRegion : Fixer_Component_ABS
{
	public override bool DoCheck(Transform target)
	{
		return HasRegion(target);
	}

	public override bool DoFix(Transform target)
	{
		return true;
	}

	private bool HasRegion(Transform target)
	{
		if (target.GetComponentInChildren<Region>(includeInactive: true) != null)
		{
			return true;
		}
		return false;
	}
}
