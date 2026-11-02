using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingPot_Recipe", menuName = "Raft/Item/CookingPot_Recipe")]
public class SO_CookingTable_Recipe : ScriptableObject
{
	[SerializeField]
	private CookingRecipeType recipeType;

	[SerializeField]
	private uint recipeIndex;

	[SerializeField]
	private Item_Base result;

	[SerializeField]
	private bool isBuff;

	[SerializeField]
	private uint portions = 1u;

	[SerializeField]
	private float cookTime = 1f;

	[SerializeField]
	private CostMultiple[] recipeCost;

	public bool IsValid
	{
		get
		{
			if (recipeCost != null && recipeCost.Length != 0)
			{
				return result != null;
			}
			return false;
		}
	}

	public bool IsBuff => isBuff;

	public uint RecipeIndex => recipeIndex;

	public Item_Base Result => result;

	public uint Portions => portions;

	public float CookTime => cookTime;

	public CostMultiple[] RecipeCost => recipeCost;

	public CookingRecipeType RecipeType => recipeType;

	public string GetRecipeTypeLabel()
	{
		string text = string.Empty;
		switch (recipeType)
		{
		case CookingRecipeType.CookingPot:
			text = Helper.GetTerm("Block/CookingPot");
			break;
		case CookingRecipeType.Juicer:
			text = Helper.GetTerm("Block/Juicer");
			break;
		}
		return text.Split('@').First();
	}

	public bool DoesIngredientsCompleteRecipe(Item_Base[] ingredientsToCheck)
	{
		if (ingredientsToCheck == null || ingredientsToCheck.Length == 0)
		{
			return false;
		}
		List<Item_Base> list = new List<Item_Base>(ingredientsToCheck);
		CostMultiple[] array = recipeCost;
		foreach (CostMultiple cost in array)
		{
			if (!CheckCost(cost, list))
			{
				return false;
			}
		}
		return list.Count == 0;
	}

	private bool CheckCost(CostMultiple cost, List<Item_Base> itemsToCheck)
	{
		int num = cost.amount;
		for (int i = 0; i < itemsToCheck.Count; i++)
		{
			if (num <= 0)
			{
				break;
			}
			Item_Base item_Base = itemsToCheck[i];
			for (int j = 0; j < cost.items.Length; j++)
			{
				Item_Base item_Base2 = cost.items[j];
				if (item_Base.UniqueIndex == item_Base2.UniqueIndex)
				{
					num--;
					itemsToCheck.RemoveAt(i);
					i--;
					break;
				}
			}
		}
		return num <= 0;
	}
}
