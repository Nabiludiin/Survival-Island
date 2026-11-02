using System;
using System.Collections.Generic;
using UnityEngine;

public class Fixer_Component_TransformNegativeScale : Fixer_Component_ABS
{
	public List<Transform> failedTransforms = new List<Transform>();

	public override bool DoCheck(Transform target)
	{
		bool result = true;
		List<Transform> result2 = new List<Transform>();
		target.GetComponentsInChildrenRecursively(ref result2);
		foreach (Transform item in result2)
		{
			if (item.lossyScale.x < 0f || item.lossyScale.y < 0f || item.lossyScale.z < 0f)
			{
				Debug.LogError(item.transform.name + item.lossyScale.ToString() + " has negative scaling");
				result = false;
			}
		}
		return result;
	}

	public override bool DoFix(Transform target)
	{
		throw new NotImplementedException();
	}
}
