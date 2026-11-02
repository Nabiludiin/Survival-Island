using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemConnection
{
	[HideInInspector]
	public string name = "ItemConnection";

	public Item_Base inventoryItem;

	public GameObject obj;

	public List<GameObject> objs = new List<GameObject>();

	public void SetActive(bool active)
	{
		obj.SetActiveSafe(active);
		foreach (GameObject obj in objs)
		{
			obj.SetActiveSafe(active);
		}
	}
}
