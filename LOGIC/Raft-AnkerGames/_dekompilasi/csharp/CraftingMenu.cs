using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{
	public RecipeMenuItem recipeMenuItemPrefab;

	public RectTransform recipeMenuItemParent;

	public RectTransform menuItemSortParent;

	public RectTransform scrollViewRect;

	public SelectedRecipeBox selectedRecipeBox;

	public ScrollRect scrollRect;

	public Image selectedCategoryImage;

	public Text selectedCategoryText;

	public GameObject creativeModeCategoryButton;

	public bool isConsole;

	[SerializeField]
	private int maxItemShown = 10;

	[SerializeField]
	private InputActionReference inputTabRight;

	[SerializeField]
	private InputActionReference inputTabLeft;

	public Sprite spriteNormal;

	public Sprite spriteHoover;

	public Sprite spriteHighlighted;

	public EventReference er_craftItem;

	private static int CategoryCount = 9;

	public CraftingCategory[] uiOrder = new CraftingCategory[9]
	{
		CraftingCategory.FoodWater,
		CraftingCategory.Other,
		CraftingCategory.Tools,
		CraftingCategory.Weapons,
		CraftingCategory.Equipment,
		CraftingCategory.Resources,
		CraftingCategory.Navigation,
		CraftingCategory.Decorations,
		CraftingCategory.CreativeMode
	};

	public Button[] uiButtons = new Button[CategoryCount];

	private CraftingCategory selectedCategory;

	private Dictionary<CraftingCategory, List<RecipeItem>> allRecipes = new Dictionary<CraftingCategory, List<RecipeItem>>();

	private Network_Player localPlayer;

	private List<RecipeMenuItem> recipeMenuItems = new List<RecipeMenuItem>();

	private float defaultHeight;

	private float spacingHeight;

	private float recipeHeight;

	public int TotalRecipeCount => allRecipes.Count;

	public List<Item_Base> AllRecipes
	{
		get
		{
			List<Item_Base> list = new List<Item_Base>();
			foreach (List<RecipeItem> value in allRecipes.Values)
			{
				foreach (RecipeItem item in value)
				{
					list.AddRange(item.recipes);
				}
			}
			return list;
		}
	}

	public List<Item_Base> RecipesLearnedBeginning
	{
		get
		{
			List<Item_Base> list = new List<Item_Base>();
			foreach (List<RecipeItem> value in allRecipes.Values)
			{
				foreach (RecipeItem item in value)
				{
					foreach (Item_Base recipe in item.recipes)
					{
						if (recipe.settings_recipe.LearnedFromBeginning)
						{
							list.Add(recipe);
						}
					}
				}
			}
			return list;
		}
	}

	private void Awake()
	{
		if (isConsole)
		{
			ComponentManager<CraftingMenu>.ValueConsole = this;
		}
		else
		{
			ComponentManager<CraftingMenu>.Value = this;
		}
		InitializeRecipes();
		InitializeMenuItems();
		SelectCraftingCategory(selectedCategory, awake: true);
	}

	private void Start()
	{
		selectedRecipeBox.gameObject.SetActive(value: false);
		defaultHeight = 85f;
		spacingHeight = recipeMenuItemParent.GetComponent<VerticalLayoutGroup>().spacing;
		recipeHeight = recipeMenuItemPrefab.GetComponent<RectTransform>().sizeDelta.y;
		SelectCraftingCategory(CraftingCategory.Tools);
		localPlayer = ComponentManager<Network_Player>.Value;
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(OnWorldRecievedLate));
	}

	private void OnWorldRecievedLate()
	{
		if (GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.unlimitedResources)
		{
			creativeModeCategoryButton.SetActive(value: true);
		}
	}

	private void OnEnable()
	{
		if (selectedCategory != CraftingCategory.Nothing)
		{
			ReselectCategory();
		}
	}

	private void Update()
	{
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			return;
		}
		int num = Array.IndexOf(uiOrder, selectedCategory);
		if ((bool)inputTabRight && inputTabRight.action.WasPressedThisFrame())
		{
			int num2 = (num + 1) % uiOrder.Length;
			if (num2 == 8 && !creativeModeCategoryButton.gameObject.activeSelf)
			{
				num2 = 0;
			}
			SelectCraftingCategory(uiOrder[num2]);
			ComponentManager<SoundManager>.Value.PlayUI_OpenMenu();
		}
		else if ((bool)inputTabLeft && inputTabLeft.action.WasPressedThisFrame())
		{
			int num3 = (num - 1 + uiOrder.Length) % uiOrder.Length;
			if (num3 == 8 && !creativeModeCategoryButton.gameObject.activeSelf)
			{
				num3 = 7;
			}
			SelectCraftingCategory(uiOrder[num3]);
			ComponentManager<SoundManager>.Value.PlayUI_OpenMenu();
		}
	}

	private void InitializeRecipes()
	{
		Item_Base[] array = ItemManager.GetAllItems().ToArray();
		SO_CraftingCategoryOrder[] array2 = Resources.LoadAll<SO_CraftingCategoryOrder>("CraftingCategoryOrderAssets");
		for (int i = 0; i < array.Length; i++)
		{
			ItemInstance_Recipe settings_recipe = array[i].settings_recipe;
			settings_recipe.Learned = false;
			if (settings_recipe.IsBlueprint)
			{
				if (settings_recipe.BlueprintItem != null)
				{
					settings_recipe.BlueprintItem.settings_recipe.LearnedViaBlueprint = true;
				}
				if (settings_recipe.ExtraBlueprintItems != null)
				{
					Item_Base[] extraBlueprintItems = settings_recipe.ExtraBlueprintItems;
					foreach (Item_Base item_Base in extraBlueprintItems)
					{
						if (item_Base != null)
						{
							item_Base.settings_recipe.LearnedViaBlueprint = true;
						}
					}
				}
			}
			if (settings_recipe.HasSkins)
			{
				settings_recipe.selectedSkin = settings_recipe.Skins[0];
				settings_recipe.SetSkinsBase(array[i]);
			}
			if (settings_recipe.CraftingCategory == CraftingCategory.Nothing)
			{
				continue;
			}
			if (allRecipes.ContainsKey(settings_recipe.CraftingCategory))
			{
				List<RecipeItem> list = allRecipes[settings_recipe.CraftingCategory];
				bool flag = false;
				if (!string.IsNullOrEmpty(settings_recipe.SubCategory))
				{
					foreach (RecipeItem item in list)
					{
						if (item.subCategory == settings_recipe.SubCategory)
						{
							item.recipes.Add(array[i]);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					list.Add(new RecipeItem(settings_recipe.SubCategory, array[i]));
				}
			}
			else
			{
				List<RecipeItem> list2 = new List<RecipeItem>();
				list2.Add(new RecipeItem(settings_recipe.SubCategory, array[i]));
				allRecipes.Add(settings_recipe.CraftingCategory, list2);
			}
			SO_CraftingCategoryOrder sO_CraftingCategoryOrder = null;
			for (int k = 0; k < array2.Length; k++)
			{
				if (array2[k].craftingCategory == settings_recipe.CraftingCategory)
				{
					sO_CraftingCategoryOrder = array2[k];
				}
			}
			if (sO_CraftingCategoryOrder != null)
			{
				settings_recipe.categoryOrder = sO_CraftingCategoryOrder.GetOrderFromItem(array[i]);
			}
			if (settings_recipe.categoryOrder < 0 || sO_CraftingCategoryOrder == null)
			{
				settings_recipe.categoryOrder = 100;
			}
		}
		for (int l = 0; l < allRecipes.Values.Count; l++)
		{
			foreach (RecipeItem item2 in allRecipes.Values.ElementAt(l))
			{
				item2.Sort();
			}
		}
	}

	private void InitializeMenuItems()
	{
		int num = 0;
		foreach (List<RecipeItem> value in allRecipes.Values)
		{
			if (value.Count > num)
			{
				num = value.Count;
			}
		}
		for (int i = 0; i < num; i++)
		{
			RecipeMenuItem recipeMenuItem = UnityEngine.Object.Instantiate(recipeMenuItemPrefab, menuItemSortParent);
			recipeMenuItem.transform.localScale = recipeMenuItemPrefab.transform.localScale;
			recipeMenuItem.viewport = menuItemSortParent.transform.parent.GetComponent<RectTransform>();
			recipeMenuItems.Add(recipeMenuItem);
		}
	}

	public void SelectCategoryTools(Image image)
	{
		SelectCraftingCategory(CraftingCategory.Tools);
		selectedCategoryImage.sprite = image.sprite;
	}

	public void SelectCategoryWeapons(Image image)
	{
		SelectCraftingCategory(CraftingCategory.Weapons);
		selectedCategoryImage.sprite = image.sprite;
	}

	public void SelectCategoryPlaceables(Image image)
	{
		SelectCraftingCategory(CraftingCategory.Other);
		selectedCategoryImage.sprite = image.sprite;
	}

	public void SelectCategoryBasics(Image image)
	{
		SelectCraftingCategory(CraftingCategory.Resources);
		selectedCategoryImage.sprite = image.sprite;
	}

	public void SelectCategoryDecorations(Image image)
	{
		SelectCraftingCategory(CraftingCategory.Decorations);
		selectedCategoryImage.sprite = image.sprite;
	}

	public void SelectCategoryEquipment(Image image)
	{
		SelectCraftingCategory(CraftingCategory.Equipment);
		selectedCategoryImage.sprite = image.sprite;
	}

	public void SelectCategoryNavigation(Image image)
	{
		SelectCraftingCategory(CraftingCategory.Navigation);
		selectedCategoryImage.sprite = image.sprite;
	}

	public void SelectCategoryFoodWater(Image image)
	{
		SelectCraftingCategory(CraftingCategory.FoodWater);
		selectedCategoryImage.sprite = image.sprite;
	}

	public void SelectCategoryCreativeMode(Image image)
	{
		SelectCraftingCategory(CraftingCategory.CreativeMode);
		selectedCategoryImage.sprite = image.sprite;
	}

	public void OnCategoryEnter(UiMagnet magnet)
	{
		Image[] componentsInChildren = magnet.GetComponentsInChildren<Image>();
		if (!(selectedCategoryImage.sprite == componentsInChildren[1].sprite))
		{
			componentsInChildren[0].sprite = spriteHoover;
		}
	}

	public void OnCategoryExit(UiMagnet magnet)
	{
		Image[] componentsInChildren = magnet.GetComponentsInChildren<Image>();
		if (!(selectedCategoryImage.sprite == componentsInChildren[1].sprite))
		{
			componentsInChildren[0].sprite = spriteNormal;
		}
	}

	public void SelectCraftingCategory(CraftingCategory category, bool awake = false)
	{
		if (category == CraftingCategory.Nothing)
		{
			recipeMenuItemParent.gameObject.SetActive(value: false);
		}
		else
		{
			if (!allRecipes.ContainsKey(category))
			{
				return;
			}
			_ = selectedCategory;
			selectedCategory = category;
			int num = Array.IndexOf(uiOrder, selectedCategory);
			UiMagnet magnetUnderCursor = ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().GetMagnetUnderCursor();
			Button button = uiButtons[num];
			Image[] componentsInChildren = button.GetComponentsInChildren<Image>();
			Button[] array = uiButtons;
			foreach (Button button2 in array)
			{
				if (!(button2 != button))
				{
					continue;
				}
				Image[] componentsInChildren2 = button2.GetComponentsInChildren<Image>();
				if (selectedCategoryImage.sprite == componentsInChildren2[1].sprite && componentsInChildren2[0].sprite != spriteHoover)
				{
					if ((bool)magnetUnderCursor)
					{
						componentsInChildren2[0].sprite = ((button2.gameObject == magnetUnderCursor.gameObject) ? spriteHoover : spriteNormal);
					}
					else
					{
						componentsInChildren2[0].sprite = spriteNormal;
					}
				}
				else
				{
					componentsInChildren2[0].sprite = spriteNormal;
				}
			}
			componentsInChildren[0].sprite = spriteHighlighted;
			selectedCategoryImage.sprite = componentsInChildren[1].sprite;
			recipeMenuItemParent.gameObject.SetActive(value: true);
			selectedCategoryText.text = Helper.GetTerm("Crafting/" + category);
			List<RecipeItem> source = allRecipes[category];
			source = source.OrderBy((RecipeItem r) => r.CategoryOrder).ToList();
			foreach (RecipeMenuItem recipeMenuItem2 in recipeMenuItems)
			{
				if (recipeMenuItem2 != null)
				{
					recipeMenuItem2.gameObject.SetActive(value: false);
					recipeMenuItem2.transform.SetSiblingIndex(100);
				}
			}
			int num2 = 0;
			int num3 = 1;
			for (int num4 = 0; num4 < source.Count; num4++)
			{
				RecipeItem recipeItem = source[num4];
				if (recipeItem == null || num4 >= source.Count || !recipeItem.HasLearned())
				{
					continue;
				}
				for (int num5 = 0; num5 < recipeMenuItems.Count; num5++)
				{
					RecipeMenuItem recipeMenuItem = recipeMenuItems[num5];
					if (!(recipeMenuItem == null) && !recipeMenuItem.gameObject.activeSelf)
					{
						num2++;
						int count = recipeItem.recipes.Count;
						if (count > num3)
						{
							num3 = count;
						}
						recipeMenuItem.gameObject.SetActive(value: true);
						recipeMenuItem.AttachRecipes(recipeItem.recipes);
						recipeMenuItem.transform.SetSiblingIndex(recipeItem.CategoryOrder);
						break;
					}
				}
			}
			int num6 = ((num2 > maxItemShown) ? maxItemShown : num2);
			Vector2 sizeDelta = recipeMenuItemParent.sizeDelta;
			sizeDelta.y = defaultHeight + recipeHeight * (float)num6 + spacingHeight * (float)num6;
			sizeDelta.x = 315f + ((num3 > 3) ? ((float)(55 * (num3 - 3))) : 0f);
			recipeMenuItemParent.sizeDelta = sizeDelta;
			scrollRect.vertical = num2 > maxItemShown;
			scrollRect.verticalScrollbar.value = 1f;
			scrollViewRect.sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y - defaultHeight);
		}
	}

	public void ReselectCategory()
	{
		SelectCraftingCategory(selectedCategory);
		SelectRecipe(selectedRecipeBox.selectedRecipeItem, forceSelect: true);
	}

	public void SelectRecipe(Item_Base recipeItem, bool forceSelect)
	{
		if (!(recipeItem == null))
		{
			Item_Base selectedRecipeItem = selectedRecipeBox.selectedRecipeItem;
			if (forceSelect || selectedRecipeItem == null || selectedRecipeItem != recipeItem)
			{
				selectedRecipeBox.DisplayRecipe(recipeItem);
			}
			else
			{
				_ = selectedRecipeItem == recipeItem;
			}
		}
	}

	public void CraftItem()
	{
		if (!(selectedRecipeBox.ItemToCraft == null))
		{
			if (localPlayer == null)
			{
				localPlayer = ComponentManager<Network_Player>.Value;
			}
			RuntimeManager.PlayOneShotAttached(er_craftItem, localPlayer.gameObject);
			Item_Base itemToCraft = selectedRecipeBox.ItemToCraft;
			if (!GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.unlimitedResources)
			{
				localPlayer.Inventory.RemoveCostMultipleIncludeSecondaryInventories(itemToCraft.settings_recipe.NewCost);
			}
			localPlayer.Inventory.AddItem(itemToCraft.UniqueName, itemToCraft.settings_recipe.AmountToCraft);
		}
	}
}
