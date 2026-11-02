using System.Collections.Generic;
using UnityEngine;

public class CostCollection : MonoBehaviour
{
	[SerializeField]
	private bool useInventoryAsAmount = true;

	[SerializeField]
	private bool includeSecondaryInventory;

	public BuildingUI_CostBox costBoxPrefab;

	public Vector2 prefabSizeDelta = Vector2.one;

	[HideInInspector]
	public List<BuildingUI_CostBox> costBoxes = new List<BuildingUI_CostBox>();

	private PlayerInventory inventory;

	private void Start()
	{
		inventory = ComponentManager<PlayerInventory>.Value;
	}

	private void OnDestroy()
	{
		inventory = null;
	}

	public bool MeetsRequirements()
	{
		foreach (BuildingUI_CostBox costBox in costBoxes)
		{
			if (costBox.gameObject.activeInHierarchy && !costBox.MeetsRequirements())
			{
				return false;
			}
		}
		return true;
	}

	public bool RequiresItem(Item_Base item)
	{
		if (item == null)
		{
			return false;
		}
		foreach (BuildingUI_CostBox costBox in costBoxes)
		{
			if (costBox.RequiresItem(item))
			{
				return !costBox.MeetsRequirements();
			}
		}
		return false;
	}

	public void ShowCost(Cost cost)
	{
		for (int i = 0; i < costBoxes.Count; i++)
		{
			if (i < 1)
			{
				BuildingUI_CostBox buildingUI_CostBox = costBoxes[i];
				buildingUI_CostBox.gameObject.SetActive(value: true);
				buildingUI_CostBox.SetRequiredItem(cost.item);
				buildingUI_CostBox.SetRequiredAmount(cost.amount);
				if (useInventoryAsAmount)
				{
					if (inventory == null)
					{
						inventory = ComponentManager<Network_Player>.Value.Inventory;
					}
					buildingUI_CostBox.SetAmountInInventory(inventory, useInventoryAsAmount);
				}
			}
			else
			{
				costBoxes[i].gameObject.SetActive(value: false);
			}
		}
	}

	public void ShowCost(List<Cost> cost)
	{
		if (cost.Count > costBoxes.Count)
		{
			SetNewLength(cost.Count);
		}
		for (int i = 0; i < costBoxes.Count; i++)
		{
			if (i < cost.Count)
			{
				BuildingUI_CostBox buildingUI_CostBox = costBoxes[i];
				buildingUI_CostBox.gameObject.SetActive(value: true);
				buildingUI_CostBox.SetRequiredItem(cost[i].item);
				buildingUI_CostBox.SetRequiredAmount(cost[i].amount);
				if (useInventoryAsAmount)
				{
					if (inventory == null)
					{
						inventory = ComponentManager<Network_Player>.Value.Inventory;
					}
					buildingUI_CostBox.SetAmountInInventory(inventory, useInventoryAsAmount);
				}
			}
			else
			{
				costBoxes[i].gameObject.SetActive(value: false);
			}
		}
	}

	public void ShowCost(CostMultiple[] cost)
	{
		if (cost.Length > costBoxes.Count)
		{
			SetNewLength(cost.Length);
		}
		for (int i = 0; i < costBoxes.Count; i++)
		{
			if (i < cost.Length)
			{
				BuildingUI_CostBox buildingUI_CostBox = costBoxes[i];
				buildingUI_CostBox.gameObject.SetActive(value: true);
				buildingUI_CostBox.SetRequiredItem(cost[i].items);
				buildingUI_CostBox.SetRequiredAmount(cost[i].amount);
				if (useInventoryAsAmount)
				{
					if (inventory == null)
					{
						inventory = ComponentManager<Network_Player>.Value.Inventory;
					}
					buildingUI_CostBox.SetAmountInInventory(inventory, useInventoryAsAmount);
				}
			}
			else
			{
				costBoxes[i].gameObject.SetActive(value: false);
			}
		}
	}

	public void IncrementItem(string uniqueName, int amount)
	{
		foreach (BuildingUI_CostBox costBox in costBoxes)
		{
			if (costBox.RequiresItem(uniqueName))
			{
				costBox.IncrementAmount(amount);
			}
		}
	}

	public void SetAmount(string uniqueName, int amount)
	{
		foreach (BuildingUI_CostBox costBox in costBoxes)
		{
			if (costBox.RequiresItem(uniqueName))
			{
				costBox.SetAmount(amount);
			}
		}
	}

	public void SetRequiredAmount(string uniqueName, int requiredAmount)
	{
		foreach (BuildingUI_CostBox costBox in costBoxes)
		{
			if (costBox.RequiresItem(uniqueName))
			{
				costBox.SetRequiredAmount(requiredAmount);
			}
		}
	}

	public void SetRadial(string uniqueName, float normal)
	{
		foreach (BuildingUI_CostBox costBox in costBoxes)
		{
			if (costBox.RequiresItem(uniqueName))
			{
				costBox.SetRadial(normal);
			}
		}
	}

	public void SetRadial(string uniqueName, bool state)
	{
		foreach (BuildingUI_CostBox costBox in costBoxes)
		{
			if (costBox.RequiresItem(uniqueName))
			{
				costBox.SetRadial(state);
			}
		}
	}

	private void SetNewLength(int newLength)
	{
		while (costBoxes.Count != newLength)
		{
			BuildingUI_CostBox buildingUI_CostBox = Object.Instantiate(costBoxPrefab, transform);
			buildingUI_CostBox.transform.GetComponent<RectTransform>().sizeDelta = prefabSizeDelta;
			costBoxes.Add(buildingUI_CostBox);
		}
	}
}
