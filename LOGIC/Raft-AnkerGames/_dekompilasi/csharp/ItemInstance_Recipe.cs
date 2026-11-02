using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemInstance_Recipe
{
	[SerializeField]
	private CraftingCategory craftingCategory = CraftingCategory.Nothing;

	public int categoryOrder;

	[SerializeField]
	private string subCategory;

	[SerializeField]
	private int subCatergoryOrder;

	public Item_Base baseSkinItem;

	[SerializeField]
	private Item_Base[] skins;

	[HideInInspector]
	public Item_Base selectedSkin;

	[SerializeField]
	private CostMultiple[] newCostToCraft;

	[SerializeField]
	private int amountToCraft = 1;

	[SerializeField]
	private bool learned;

	[SerializeField]
	private bool learnedFromBeginning;

	[SerializeField]
	private bool _hiddenInResearchTable;

	[SerializeField]
	private Item_Base blueprintItem;

	[SerializeField]
	private Item_Base[] extraBlueprintItems;

	[SerializeField]
	private bool learnedViaBlueprint;

	public bool HasSubCategory => !string.IsNullOrEmpty(SubCategory);

	public bool HasHiddenSubCategory
	{
		get
		{
			if (HasSubCategory)
			{
				return string.IsNullOrEmpty(LocalizedSubCategory);
			}
			return false;
		}
	}

	public string LocalizedSubCategory => Helper.GetTerm("CraftingSub/" + SubCategory);

	public CraftingCategory CraftingCategory => craftingCategory;

	public string SubCategory => subCategory;

	public int SubCategoryOrder => subCatergoryOrder;

	public bool HasSkins
	{
		get
		{
			if (skins != null)
			{
				return skins.Length != 0;
			}
			return false;
		}
	}

	public Item_Base[] Skins => skins;

	public CostMultiple[] NewCost
	{
		get
		{
			return newCostToCraft;
		}
		set
		{
			newCostToCraft = value;
		}
	}

	public int AmountToCraft => amountToCraft;

	public bool CanCraft
	{
		get
		{
			if (!Learned && !LearnedFromBeginning)
			{
				return Cheat.UnlockAllCrafting;
			}
			return true;
		}
	}

	public bool Learned
	{
		get
		{
			return learned;
		}
		set
		{
			learned = value;
		}
	}

	public bool LearnedFromBeginning => learnedFromBeginning;

	public bool HiddenInResearchTable => _hiddenInResearchTable;

	public bool IsBlueprint
	{
		get
		{
			if (!(BlueprintItem != null))
			{
				if (ExtraBlueprintItems != null)
				{
					return ExtraBlueprintItems.Length != 0;
				}
				return false;
			}
			return true;
		}
	}

	public Item_Base BlueprintItem => blueprintItem;

	public Item_Base[] ExtraBlueprintItems => extraBlueprintItems;

	public bool LearnedViaBlueprint
	{
		get
		{
			return learnedViaBlueprint;
		}
		set
		{
			learnedViaBlueprint = value;
		}
	}

	private void Button_ToggleLearnedViaBlueprint()
	{
		LearnedViaBlueprint = !LearnedViaBlueprint;
	}

	public ItemInstance_Recipe(CraftingCategory craftingCategory, bool learned, bool learnedFromBeginning, string subCategory, int subCategoryOrder)
	{
		this.craftingCategory = craftingCategory;
		this.learnedFromBeginning = learnedFromBeginning;
		this.subCategory = subCategory;
		subCatergoryOrder = subCategoryOrder;
		if (this.craftingCategory != CraftingCategory.Nothing)
		{
			this.learned = learned;
			return;
		}
		amountToCraft = 1;
		this.learned = false;
	}

	public ItemInstance_Recipe Clone()
	{
		return (ItemInstance_Recipe)MemberwiseClone();
	}

	public bool HasEnoughResourcesToCraft(PlayerInventory playerInventory)
	{
		CostMultiple[] newCost = NewCost;
		for (int i = 0; i < newCost.Length; i++)
		{
			if (!newCost[i].HasEnoughInInventory(playerInventory))
			{
				return false;
			}
		}
		return true;
	}

	public void SetSkinsBase(Item_Base baseItem)
	{
		Item_Base[] array = skins;
		foreach (Item_Base item_Base in array)
		{
			if (item_Base != null)
			{
				item_Base.settings_recipe.baseSkinItem = baseItem;
			}
			else
			{
				Debug.Log(baseItem?.ToString() + " has skins that is null");
			}
		}
	}

	public List<int> GetBlueprintItemIndexes()
	{
		if (!IsBlueprint)
		{
			return null;
		}
		List<int> list = new List<int>();
		if (BlueprintItem != null)
		{
			list.Add(BlueprintItem.UniqueIndex);
		}
		if (ExtraBlueprintItems != null)
		{
			for (int i = 0; i < ExtraBlueprintItems.Length; i++)
			{
				Item_Base item_Base = ExtraBlueprintItems[i];
				if (!(item_Base == null))
				{
					list.Add(item_Base.UniqueIndex);
				}
			}
		}
		return list;
	}
}
