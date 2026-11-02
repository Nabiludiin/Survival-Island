using UnityEngine;

public class ItemObjectEnabler : MonoBehaviour
{
	[SerializeField]
	protected ItemModelConnection[] itemConnections;

	public void ActivateAllModels()
	{
		if (itemConnections != null)
		{
			for (int i = 0; i < itemConnections.Length; i++)
			{
				itemConnections[i]?.SetActive(value: true);
			}
		}
	}

	public void DeActivateAllModels()
	{
		if (itemConnections != null)
		{
			for (int i = 0; i < itemConnections.Length; i++)
			{
				itemConnections[i]?.SetActive(value: false);
			}
		}
	}

	public void ActivateModel(int modelIndex)
	{
		if (itemConnections != null)
		{
			for (int i = 0; i < itemConnections.Length; i++)
			{
				itemConnections[i]?.SetActive(i == modelIndex);
			}
		}
	}

	public void ActivateModel(Item_Base item)
	{
		if (itemConnections != null && !(item == null))
		{
			for (int i = 0; i < itemConnections.Length; i++)
			{
				ItemModelConnection itemModelConnection = itemConnections[i];
				itemModelConnection?.SetActive(itemModelConnection.item.UniqueIndex == item.UniqueIndex);
			}
		}
	}

	public void DeActivateModel(Item_Base item)
	{
		if (itemConnections == null || item == null)
		{
			return;
		}
		for (int i = 0; i < itemConnections.Length; i++)
		{
			ItemModelConnection itemModelConnection = itemConnections[i];
			if (itemModelConnection != null && itemModelConnection.item.UniqueIndex == item.UniqueIndex)
			{
				itemModelConnection.SetActive(value: false);
				break;
			}
		}
	}

	public bool IsItemActive(Item_Base item)
	{
		if (itemConnections == null)
		{
			return false;
		}
		ItemModelConnection[] array = itemConnections;
		foreach (ItemModelConnection itemModelConnection in array)
		{
			if (itemModelConnection != null && itemModelConnection.item.UniqueIndex == item.UniqueIndex && itemModelConnection.model.activeInHierarchy)
			{
				return true;
			}
		}
		return false;
	}

	public bool DoesAcceptItem(Item_Base item)
	{
		for (int i = 0; i < itemConnections.Length; i++)
		{
			ItemModelConnection itemModelConnection = itemConnections[i];
			if (itemModelConnection != null && itemModelConnection.item.UniqueIndex == item.UniqueIndex)
			{
				return true;
			}
		}
		return false;
	}

	public ItemModelConnection[] GetObjectConnections()
	{
		return itemConnections;
	}

	public GameObject GetModelFromItem(Item_Base item)
	{
		if (itemConnections == null || item == null)
		{
			return null;
		}
		ItemModelConnection[] array = itemConnections;
		foreach (ItemModelConnection itemModelConnection in array)
		{
			if (itemModelConnection != null && itemModelConnection.item.UniqueIndex == item.UniqueIndex)
			{
				return itemModelConnection.model;
			}
		}
		return null;
	}
}
