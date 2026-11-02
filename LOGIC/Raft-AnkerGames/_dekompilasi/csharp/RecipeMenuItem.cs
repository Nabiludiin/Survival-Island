using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RecipeMenuItem : MonoBehaviour
{
	[SerializeField]
	private Button textTransform;

	[SerializeField]
	private Text text_subCategory;

	[SerializeField]
	private RecipeMenuSubItem subItemPrefab;

	[SerializeField]
	private RectTransform subItemParent;

	public RectTransform viewport;

	private List<RecipeMenuSubItem> subItems = new List<RecipeMenuSubItem>();

	private float subItemDeltaSize;

	private int m_subItemCount;

	private Item_Base m_firstItem;

	public int SubItemCount
	{
		get
		{
			if (subItems.ContainsItems())
			{
				return subItems.Count;
			}
			return 1;
		}
	}

	private void Awake()
	{
		subItems = GetComponentsInChildren<RecipeMenuSubItem>().ToList();
		subItemDeltaSize = subItemPrefab.GetComponent<RectTransform>().sizeDelta.x;
	}

	private void OnEnable()
	{
		textTransform.interactable = true;
	}

	public void AttachRecipes(List<Item_Base> recipes)
	{
		if (recipes.Count == 0)
		{
			return;
		}
		while (subItems.Count < recipes.Count)
		{
			RecipeMenuSubItem recipeMenuSubItem = Object.Instantiate(subItemPrefab, subItemParent);
			recipeMenuSubItem.GetComponentInChildren<UiMagnet>().Viewport = viewport;
			recipeMenuSubItem.transform.localScale = subItemPrefab.transform.localScale;
			subItems.Add(recipeMenuSubItem);
			if (subItems.Count == 1)
			{
				textTransform.targetGraphic = subItems[0].GetComponent<Button>().targetGraphic;
			}
		}
		int num = 0;
		bool flag = false;
		Item_Base item_Base = null;
		for (int i = 0; i < subItems.Count; i++)
		{
			RecipeMenuSubItem recipeMenuSubItem2 = subItems[i];
			if (i < recipes.Count && recipes[i].settings_recipe.CanCraft)
			{
				if (item_Base == null)
				{
					item_Base = recipes[i];
				}
				num++;
				recipeMenuSubItem2.gameObject.SetActive(value: true);
				recipeMenuSubItem2.AttachRecipe(recipes[i]);
				if (recipeMenuSubItem2.recipeItem.settings_recipe.HasHiddenSubCategory)
				{
					flag = true;
				}
			}
			else
			{
				recipeMenuSubItem2.gameObject.SetActive(value: false);
			}
		}
		if (flag)
		{
			textTransform.gameObject.SetActive(value: false);
		}
		else
		{
			textTransform.gameObject.SetActive(value: true);
		}
		if (item_Base == null)
		{
			item_Base = recipes[0];
		}
		if (string.IsNullOrEmpty(item_Base.settings_recipe.SubCategory) || num < 2)
		{
			text_subCategory.text = item_Base.settings_Inventory.DisplayName;
		}
		else
		{
			text_subCategory.text = Helper.GetTerm("CraftingSub/" + item_Base.settings_recipe.SubCategory);
		}
		textTransform.transform.SetAsLastSibling();
		m_subItemCount = num;
		m_firstItem = item_Base;
	}

	public void OnClickItemText()
	{
		if (subItems != null && subItems[0] != null)
		{
			ComponentManager<CraftingMenu>.Value.SelectRecipe(subItems[0].recipeItem, forceSelect: false);
		}
	}
}
