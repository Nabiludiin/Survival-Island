using UnityEngine;
using UnityEngine.UI;

public class RecipeMenuSubItem : MonoBehaviour
{
	public Image item_bg;

	[SerializeField]
	private Image image_item;

	[HideInInspector]
	public Item_Base recipeItem;

	public static RecipeMenuSubItem selectedSubItem;

	public void AttachRecipe(Item_Base recipe)
	{
		recipeItem = recipe;
		image_item.sprite = recipe.settings_Inventory.Sprite;
	}

	public void OnButtonClick()
	{
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			ComponentManager<CraftingMenu>.ValueConsole.SelectRecipe(recipeItem, forceSelect: false);
		}
		else
		{
			ComponentManager<CraftingMenu>.Value.SelectRecipe(recipeItem, forceSelect: false);
		}
	}

	public void OnEnter()
	{
		if (selectedSubItem != this)
		{
			item_bg.sprite = ComponentManager<CraftingMenu>.Value.spriteHoover;
		}
	}

	public void OnExit()
	{
		if (selectedSubItem != this)
		{
			item_bg.sprite = ComponentManager<CraftingMenu>.Value.spriteNormal;
		}
	}
}
