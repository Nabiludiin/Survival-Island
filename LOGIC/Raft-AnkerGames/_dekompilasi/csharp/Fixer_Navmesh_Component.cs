using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class Fixer_Navmesh_Component : Fixer_Component_ABS
{
	public override bool DoFix(Transform target)
	{
		return true;
	}

	public override bool DoCheck(Transform target)
	{
		bool flag = false;
		NavMeshSurface componentInChildren = target.GetComponentInChildren<NavMeshSurface>(includeInactive: true);
		if (componentInChildren != null && componentInChildren.navMeshData != null)
		{
			flag = true;
		}
		if (!flag)
		{
			Debug.LogError("Navmesh is null or navmeshData on target is missing" + target.transform.name, target.gameObject);
		}
		return flag;
	}
}
