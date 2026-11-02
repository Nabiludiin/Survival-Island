using UnityEngine;

public class Fixer_Component_SceneRootIsOne : Fixer_Component_ABS
{
	public override bool DoCheck(Transform target)
	{
		if (target.gameObject.scene.rootCount == 1)
		{
			return true;
		}
		Debug.LogError(target.gameObject.scene.name + " has more then 1 rootObjects...");
		return false;
	}

	public override bool DoFix(Transform target)
	{
		return true;
	}
}
