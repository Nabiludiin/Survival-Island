using System;
using System.Collections.Generic;
using UnityEngine;

public class Fixer_Component_TransformByNameExists : Fixer_Component_ABS
{
	private string name;

	public Fixer_Component_TransformByNameExists(string name)
	{
		this.name = name;
	}

	public override bool DoCheck(Transform target)
	{
		bool result = true;
		List<Transform> result2 = new List<Transform>();
		target.GetComponentsInChildrenRecursively(ref result2);
		foreach (Transform item in result2)
		{
			if (item.name.Contains(name, StringComparison.OrdinalIgnoreCase))
			{
				result = false;
				Debug.Log(item?.ToString() + " " + item.name + " has " + name + " in it on landmark " + target.transform.name);
			}
		}
		return result;
	}

	public override bool DoFix(Transform target)
	{
		throw new NotImplementedException();
	}
}
