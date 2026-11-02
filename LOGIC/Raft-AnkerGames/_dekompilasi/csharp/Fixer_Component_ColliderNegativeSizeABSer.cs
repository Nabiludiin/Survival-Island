using System.Collections.Generic;
using UnityEngine;

public class Fixer_Component_ColliderNegativeSizeABSer : Fixer_Component_ABS
{
	public override bool DoCheck(Transform target)
	{
		bool flag = true;
		List<Collider> result = new List<Collider>();
		target.GetComponentsInChildrenRecursively(ref result);
		List<Collider> list = new List<Collider>();
		foreach (Collider item in result)
		{
			if (item != null && (item.bounds.size.x < 0f || item.bounds.size.y < 0f || item.bounds.size.z < 0f))
			{
				list.Add(item);
				flag = false;
			}
		}
		if (!flag)
		{
			Debug.LogError("Collis are Negative and invalid for " + list.Count + " Colliders!");
			foreach (Collider item2 in list)
			{
				Debug.LogError(item2.transform.name + " Has invalid collider size! " + item2.bounds.size.ToString());
			}
		}
		return flag;
	}

	public override bool DoFix(Transform target)
	{
		return true;
	}
}
