using UnityEngine;

public class Fixer_Component_LandmarkTerrain : Fixer_Component_ABS
{
	public override bool DoCheck(Transform target)
	{
		Terrain componentInChildren = target.GetComponentInChildren<Terrain>(includeInactive: true);
		if (componentInChildren != null)
		{
			if (!componentInChildren.drawInstanced)
			{
				Debug.LogError("Terrain is not set to draw instanced" + target.transform.name + componentInChildren.transform.name);
			}
			return componentInChildren.drawInstanced;
		}
		return true;
	}

	public override bool DoFix(Transform target)
	{
		return true;
	}
}
