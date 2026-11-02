using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingUI_Costbox_Sub_Crafting : BuildingUI_CostBox_Sub, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private Button quickCraftButton;

	[SerializeField]
	private UiMagnet magnet;

	private BuildingUI_CostBox_CraftingCost costBox;

	protected override void Start()
	{
		costBox = GetComponentInParent<BuildingUI_CostBox_CraftingCost>();
	}

	private void Update()
	{
		bool flag = quickCraftButton != null && quickCraftButton.gameObject.activeInHierarchy;
		magnet.enabled = quickCraftButton != null && item.settings_recipe.CanCraft && item.settings_recipe.NewCost.Length != 0;
		magnet.UserInt = (flag ? 1 : 0);
		if (flag)
		{
			quickCraftButton.interactable = item.settings_recipe.HasEnoughResourcesToCraft(costBox.PlayerInventory);
		}
	}

	private void OnDisable()
	{
		if (quickCraftButton != null)
		{
			quickCraftButton.gameObject.SetActive(value: false);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		costBox.PointerEnterSub(this);
		if (quickCraftButton != null && item.settings_recipe.CanCraft && item.settings_recipe.NewCost.Length != 0)
		{
			quickCraftButton.gameObject.SetActive(value: true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		costBox.PointerExitSub(this);
		if (quickCraftButton != null)
		{
			quickCraftButton.gameObject.SetActive(value: false);
		}
	}

	public void OnQuickCraft()
	{
		if (item != null)
		{
			costBox.PlayerInventory.RemoveCostMultiple(item.settings_recipe.NewCost);
			costBox.PlayerInventory.AddItem(item.UniqueName, item.settings_recipe.AmountToCraft);
		}
	}
}
