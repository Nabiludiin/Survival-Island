using System;
using UnityEngine;

[Serializable]
public class CookItemConnection
{
	[HideInInspector]
	public string name;

	public Item_Base cookableItem;

	public GameObject rawItem;

	public GameObject cookedItem;

	public void SetRawState(bool state)
	{
		if (rawItem != null)
		{
			rawItem.SetActive(state);
		}
	}

	public void SetCookedState(bool state)
	{
		if (cookedItem != null)
		{
			cookedItem.SetActive(state);
		}
	}
}
