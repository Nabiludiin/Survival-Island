using System.Collections.Generic;
using System.Linq;

public class RecipeItem
{
	public string subCategory;

	public List<Item_Base> recipes = new List<Item_Base>();

	public int CategoryOrder
	{
		get
		{
			if (recipes != null && recipes.Count > 0)
			{
				return recipes[0].settings_recipe.categoryOrder;
			}
			return 100;
		}
	}

	public RecipeItem(string subCategory, params Item_Base[] recipes)
	{
		this.subCategory = subCategory;
		this.recipes = recipes.ToList();
	}

	public void Sort()
	{
		recipes = recipes.OrderBy((Item_Base r) => r.settings_recipe.SubCategoryOrder).ToList();
	}

	public bool HasLearned()
	{
		for (int i = 0; i < recipes.Count; i++)
		{
			if (recipes[i].settings_recipe.CanCraft)
			{
				return true;
			}
		}
		return false;
	}
}
