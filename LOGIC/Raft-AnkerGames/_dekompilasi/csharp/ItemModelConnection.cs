using System;
using UnityEngine;

[Serializable]
public class ItemModelConnection
{
	public Item_Base item;

	public GameObject model;

	public void SetActive(bool value)
	{
		if (model.activeSelf != value)
		{
			model.SetActive(value);
		}
	}
}
