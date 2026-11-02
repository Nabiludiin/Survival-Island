using UnityEngine;
using UnityEngine.UI;

public class SelectedRecipeBox : MonoBehaviour
{
	public Text recipeLabel;

	public Text recipeDescription;

	public Text craftAmountText;

	public Image recipeImage;

	public Button craftButton;

	public GameObject consoleHelpText;

	[SerializeField]
	private RectTransform itemDescriptionRect;

	[SerializeField]
	private RectTransform costBoxParent;

	[SerializeField]
	private RectTransform skinMenuParent;

	[SerializeField]
	private RectTransform skinMenuItemPrefab;

	[SerializeField]
	private CostCollection costCollection;

	[SerializeField]
	private ContentSizeFitter contentSizeFitter;

	[SerializeField]
	private RectTransform layoutGroupParent;

	[SerializeField]
	private float backgroundOffset = 110f;

	[HideInInspector]
	public Item_Base selectedRecipeItem;

	private PlayerInventory playerInventory;

	private RectTransform rect;

	private float startRectHeight;

	private float costboxHeight;

	public bool initialized;

	public Item_Base ItemToCraft
	{
		get
		{
			if (selectedRecipeItem == null)
			{
				return null;
			}
			if (selectedRecipeItem.settings_recipe.HasSkins && selectedRecipeItem.settings_recipe.selectedSkin != null)
			{
				return selectedRecipeItem.settings_recipe.selectedSkin;
			}
			return selectedRecipeItem;
		}
	}

	private void Initialize()
	{
		initialized = true;
		playerInventory = ComponentManager<PlayerInventory>.Value;
		rect = GetComponent<RectTransform>();
		startRectHeight = rect.sizeDelta.y;
		costboxHeight = costCollection.prefabSizeDelta.y + GetComponentInChildren<VerticalLayoutGroup>().spacing;
		CanvasHelper.MenuOpenEvent += OnMenuOpen;
	}

	private void OnDestroy()
	{
		initialized = false;
	}

	private void Update()
	{
		if (!(ItemToCraft == null) && CanvasHelper.ActiveMenu == MenuType.Inventory)
		{
			CostMultiple[] newCost = ItemToCraft.settings_recipe.NewCost;
			costCollection.ShowCost(newCost);
			craftButton.interactable = costCollection.MeetsRequirements();
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C) && Cheat.AllowCheatsForLocalPlayer())
			{
				playerInventory.AddItem(ItemToCraft.UniqueName, 1);
			}
		}
	}

	private void OnMenuOpen(MenuType menuType)
	{
		if (selectedRecipeItem != null)
		{
			DisplayRecipe(selectedRecipeItem);
		}
	}

	public void DisplayRecipe(Item_Base recipeItem)
	{
		if (!initialized)
		{
			Initialize();
		}
		bool flag = selectedRecipeItem != recipeItem;
		if (recipeItem == null)
		{
			selectedRecipeItem = null;
			if (gameObject.activeSelf)
			{
				gameObject.SetActive(value: false);
			}
			return;
		}
		UpdateIconNameDescription(recipeItem);
		craftButton.interactable = false;
		if (consoleHelpText != null)
		{
			consoleHelpText.SetActive(value: false);
		}
		CostMultiple[] newCost = recipeItem.settings_recipe.NewCost;
		if (flag)
		{
			selectedRecipeItem = recipeItem;
			if (selectedRecipeItem == null)
			{
				if (gameObject.activeSelf)
				{
					gameObject.SetActive(value: false);
				}
				return;
			}
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(value: true);
			}
			UpdateCraftAmountText(recipeItem);
			costCollection.ShowCost(newCost);
			skinMenuParent.gameObject.SetActiveSafe(value: false);
			if (selectedRecipeItem.settings_recipe.HasSkins)
			{
				int num = 0;
				if (selectedRecipeItem.settings_recipe.HasSkins)
				{
					Item_Base[] skins = selectedRecipeItem.settings_recipe.Skins;
					foreach (Item_Base item_Base in skins)
					{
						if (item_Base.settings_recipe.Learned || item_Base.settings_recipe.LearnedFromBeginning)
						{
							num++;
						}
					}
				}
				Item_Base[] skins2 = selectedRecipeItem.settings_recipe.Skins;
				if (skinMenuParent.childCount < skins2.Length)
				{
					int num2 = skins2.Length - skinMenuParent.childCount;
					for (int j = 0; j < num2; j++)
					{
						Object.Instantiate(skinMenuItemPrefab, skinMenuParent);
					}
				}
				for (int k = 0; k < skinMenuParent.childCount; k++)
				{
					Transform child = skinMenuParent.GetChild(k);
					if (child == null)
					{
						continue;
					}
					if (k >= skins2.Length)
					{
						child.gameObject.SetActiveSafe(value: false);
						continue;
					}
					Item_Base item_Base2 = skins2[k];
					if (num > 1 && (item_Base2.settings_recipe.Learned || item_Base2.settings_recipe.LearnedFromBeginning))
					{
						child.gameObject.SetActiveSafe(value: true);
						SkinMenuItem component = child.GetComponent<SkinMenuItem>();
						component.AttachSkinItem(item_Base2);
						if (item_Base2 != null && item_Base2 == selectedRecipeItem.settings_recipe.selectedSkin)
						{
							component.ShowHighlight(value: true);
							component.SetAsActiveHighlight();
						}
					}
					else
					{
						child.gameObject.SetActiveSafe(value: false);
					}
				}
				DisplaySkin(selectedRecipeItem.settings_recipe.selectedSkin);
				if (num > 1)
				{
					skinMenuParent.gameObject.SetActiveSafe(value: true);
				}
				LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroupParent);
			}
		}
		RefreshLayout(newCost.Length);
	}

	public void DisplaySkin(Item_Base skinItem)
	{
		if (!(skinItem == null))
		{
			selectedRecipeItem.settings_recipe.selectedSkin = skinItem;
			UpdateIconNameDescription(skinItem);
			UpdateCraftAmountText(skinItem);
			CostMultiple[] newCost = skinItem.settings_recipe.NewCost;
			costCollection.ShowCost(newCost);
			RefreshLayout(newCost.Length);
		}
	}

	private void UpdateIconNameDescription(Item_Base item)
	{
		recipeImage.sprite = item.settings_Inventory.Sprite;
		recipeLabel.text = item.settings_Inventory.DisplayName;
		recipeDescription.text = item.settings_Inventory.Description;
	}

	private void UpdateCraftAmountText(Item_Base item)
	{
		bool flag = item.settings_recipe.AmountToCraft > 1;
		craftAmountText.gameObject.SetActive(flag);
		if (flag)
		{
			craftAmountText.text = "x" + item.settings_recipe.AmountToCraft;
		}
	}

	private void RefreshLayout(int recipeLength)
	{
		contentSizeFitter.SetLayoutVertical();
		int num = recipeLength - 1;
		int num2 = 0;
		if (selectedRecipeItem.settings_recipe.HasSkins)
		{
			Item_Base[] skins = selectedRecipeItem.settings_recipe.Skins;
			foreach (Item_Base item_Base in skins)
			{
				if (item_Base.settings_recipe.Learned || item_Base.settings_recipe.LearnedFromBeginning)
				{
					num2++;
				}
			}
		}
		int num3 = (selectedRecipeItem.settings_recipe.HasSkins ? (1 + Mathf.FloorToInt((float)num2 / 5f)) : 0);
		float num4 = backgroundOffset - (float)(num * 10);
		rect.sizeDelta = new Vector2(rect.sizeDelta.x, startRectHeight + num4 + (float)num * costboxHeight + itemDescriptionRect.sizeDelta.y + (float)num3 * costboxHeight);
		costBoxParent.sizeDelta = new Vector2(costBoxParent.sizeDelta.x, costboxHeight + (float)num * costboxHeight);
	}
}
