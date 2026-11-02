using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindObjectsOfTypeUnderPrefab : MonoBehaviour
{
	public Transform prefabTransform;

	public List<GameObject> objectArray;

	public void PopulateObjectArray()
	{
		new List<Light>();
		foreach (Light item in prefabTransform.GetComponentsInChildren<Light>().ToList())
		{
			objectArray.Add(item.gameObject);
		}
	}
}
