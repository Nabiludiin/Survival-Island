using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingCategoryOrder", menuName = "Crafting/CraftingCategoryOrder")]
public class SO_CraftingCategoryOrder : ScriptableObject
{
	public CraftingCategory craftingCategory;

	public List<Item_Base> itemsInOrder;

	public void AquireItems()
	{
		if (itemsInOrder == null)
		{
			itemsInOrder = new List<Item_Base>();
		}
		Item_Base[] allItems = Resources.LoadAll<Item_Base>("IItems");
		RefreshItems(allItems);
	}

	public void RefreshItems(Item_Base[] allItems)
	{
		foreach (Item_Base item_Base in allItems)
		{
			if (item_Base.settings_recipe.CraftingCategory == craftingCategory && item_Base.settings_recipe.SubCategoryOrder == 0 && !itemsInOrder.Contains(item_Base))
			{
				itemsInOrder.Add(item_Base);
			}
		}
	}

	public int GetOrderFromItem(Item_Base item)
	{
		for (int i = 0; i < itemsInOrder.Count; i++)
		{
			if (itemsInOrder[i] == item)
			{
				return i;
			}
		}
		return -1;
	}
}
