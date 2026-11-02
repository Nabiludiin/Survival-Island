using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Fixer_Component_GPUInstancing : Fixer_Component_ABS
{
	public bool includeParticleSystems;

	public bool includeTextMeshPro;

	public override bool DoCheck(Transform target)
	{
		bool result = true;
		List<Transform> result2 = new List<Transform>();
		target.GetComponentsInChildrenRecursively(ref result2);
		foreach (Transform item in result2)
		{
			Renderer component = item.GetComponent<Renderer>();
			if (!(component != null) || (!includeParticleSystems && item.GetComponent<ParticleSystem>() != null) || (!includeTextMeshPro && item.GetComponent<TextMeshPro>() != null))
			{
				continue;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i] == null)
				{
					Debug.Log(component.transform.name + " contains a NULL material", item.gameObject);
				}
				else if (!sharedMaterials[i].enableInstancing)
				{
					Debug.Log(component.transform.name + " contains non instanced materials", item.gameObject);
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
