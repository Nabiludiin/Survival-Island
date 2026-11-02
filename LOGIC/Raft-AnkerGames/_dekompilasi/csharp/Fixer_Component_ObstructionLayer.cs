using System;
using System.Collections.Generic;
using UltimateWater;
using UnityEngine;

[Serializable]
public class Fixer_Component_ObstructionLayer : Fixer_Component_ABS
{
	public LayerMask obstructionMask;

	public override bool DoCheck(Transform target)
	{
		bool result = true;
		List<Transform> result2 = new List<Transform>();
		target.GetComponentsInChildrenRecursively(ref result2);
		List<string> list = new List<string>();
		foreach (Transform item in result2)
		{
			if (!(item.tag == "Tree") && !(item.GetComponent<ParticleSystem>() != null) && !(item.GetComponent<RagdollPart>() != null))
			{
				Renderer component = item.GetComponent<Renderer>();
				Collider component2 = item.GetComponent<Collider>();
				if ((!(component == null) || (!(item.GetComponent<RaycastInteractable>() != null) && !(item.GetComponent<RaycastInteractable_Redirect>() != null) && !(item.GetComponent<Pickup>() != null) && !(item.GetComponent<WaterVolumeSubtract>() != null) && !(component2 == null))) && ((1 << item.gameObject.layer) & (int)obstructionMask) == 0 && !list.Contains(item.name))
				{
					Debug.Log(item.name + " is not on obstructionlayer", item.gameObject);
					list.Add(item.name);
					result = false;
				}
			}
		}
		return result;
	}

	public override bool DoFix(Transform target)
	{
		return true;
	}
}
