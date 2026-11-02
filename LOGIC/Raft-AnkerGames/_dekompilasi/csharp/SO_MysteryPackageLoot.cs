using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MysteryPackageLoot", menuName = "Raft/MysterPackageLoot")]
public class SO_MysteryPackageLoot : ScriptableObject
{
	public List<Item_Base> possibleYield = new List<Item_Base>();

	public void PrintMissingHiddenLoot()
	{
		foreach (ResearchMenuItem menuItem in ComponentManager<Inventory_ResearchTable>.Value.GetMenuItems())
		{
			Item_Base item = menuItem.GetItem();
			if (item.settings_recipe.HiddenInResearchTable && !possibleYield.Contains(item))
			{
				Debug.Log(item.UniqueName + " Does not exist in mysterypackage");
			}
		}
	}

	public void AddMissingHiddenLoot()
	{
		foreach (ResearchMenuItem menuItem in ComponentManager<Inventory_ResearchTable>.Value.GetMenuItems())
		{
			Item_Base item = menuItem.GetItem();
			if (item.settings_recipe.HiddenInResearchTable && !possibleYield.Contains(item))
			{
				possibleYield.Add(item);
				Debug.Log(" Adding " + item.UniqueName + "to mysterypackage");
			}
		}
	}

	public List<Item_Base> GetNonLearnedItemsFromPossibles()
	{
		List<Item_Base> list = new List<Item_Base>();
		foreach (Item_Base item in possibleYield)
		{
			if (IsPossibleYieldItem(item))
			{
				list.Add(item);
			}
		}
		return list;
	}

	public static bool IsPossibleYieldItem(Item_Base item)
	{
		if (!item.settings_recipe.Learned && item.settings_recipe.HiddenInResearchTable)
		{
			if (item.settings_recipe.baseSkinItem != null)
			{
				Item_Base baseSkinItem = item.settings_recipe.baseSkinItem;
				if (!baseSkinItem.settings_recipe.Learned && baseSkinItem.UniqueIndex != item.UniqueIndex && !baseSkinItem.settings_recipe.HiddenInResearchTable && !baseSkinItem.settings_recipe.LearnedFromBeginning)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public Item_Base GetRandomItemFromPossibles()
	{
		List<Item_Base> nonLearnedItemsFromPossibles = GetNonLearnedItemsFromPossibles();
		if (nonLearnedItemsFromPossibles.Count > 0)
		{
			int index = Random.Range(0, nonLearnedItemsFromPossibles.Count);
			return nonLearnedItemsFromPossibles[index];
		}
		return null;
	}

	public bool HasUnlearnedLoot()
	{
		return GetNonLearnedItemsFromPossibles().Count > 0;
	}
}
