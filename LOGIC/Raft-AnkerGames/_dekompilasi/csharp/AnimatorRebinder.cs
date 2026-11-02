using System.Collections.Generic;
using UnityEngine;

public class AnimatorRebinder : MonoBehaviour
{
	public GameObject target;

	public void Rebind()
	{
		Animator component = GetComponent<Animator>();
		if (component == null)
		{
			Debug.Log("Can't rebind, missing animator on " + transform.name);
		}
		else
		{
			component.Rebind();
		}
	}

	public void RebinBonesToSkin()
	{
		SkinnedMeshRenderer component = target.GetComponent<SkinnedMeshRenderer>();
		Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
		Transform[] bones = component.bones;
		foreach (Transform transform in bones)
		{
			dictionary[transform.gameObject.name] = transform;
		}
		SkinnedMeshRenderer component2 = base.gameObject.GetComponent<SkinnedMeshRenderer>();
		Transform[] array = new Transform[component2.bones.Length];
		for (int j = 0; j < component2.bones.Length; j++)
		{
			GameObject gameObject = component2.bones[j].gameObject;
			if (!dictionary.TryGetValue(gameObject.name, out array[j]))
			{
				Debug.Log("Unable to map bone \"" + gameObject.name + "\" to target skeleton.");
				break;
			}
		}
		component2.bones = array;
	}
}
