using UnityEngine;
using UnityEngine.UI;

public class UI_Cost : MonoBehaviour
{
	public Button backgroundButton;

	public Image image;

	public new Text name;

	public Text description;

	public Text amount;

	[SerializeField]
	private bool showXBeforeAmount;

	[HideInInspector]
	public Cost cost;

	[SerializeField]
	private CanvasGroup imageAlphaGroup;

	protected bool isValidForInteract = true;

	public void Refresh(Cost newCost)
	{
		cost.item = newCost.item;
		cost.amount = newCost.amount;
		if (image != null && cost.item != null)
		{
			image.sprite = cost.item.settings_Inventory.Sprite;
		}
		if (name != null && cost.item != null)
		{
			name.text = cost.item.settings_Inventory.DisplayName;
		}
		if (amount != null)
		{
			amount.gameObject.SetActiveSafe(cost.amount >= 1);
			if (showXBeforeAmount)
			{
				amount.text = "x" + cost.amount;
			}
			else
			{
				amount.text = cost.amount.ToString();
			}
		}
		if (description != null && cost.item != null)
		{
			description.text = cost.item.settings_Inventory.Description;
		}
	}

	public void Refresh(Cost newCost, int inventoryAmount)
	{
		Refresh(newCost);
		if (imageAlphaGroup != null)
		{
			if (inventoryAmount >= newCost.amount)
			{
				imageAlphaGroup.alpha = 1f;
			}
			else
			{
				imageAlphaGroup.alpha = 0.5f;
			}
		}
		isValidForInteract = inventoryAmount >= newCost.amount;
		if (backgroundButton != null)
		{
			backgroundButton.interactable = inventoryAmount >= newCost.amount;
		}
		if (amount != null)
		{
			if (showXBeforeAmount)
			{
				amount.text = "x" + inventoryAmount + "/" + newCost.amount;
			}
			else
			{
				amount.text = inventoryAmount + "/" + newCost.amount;
			}
		}
	}

	public void RefreshDisplayInventoryCount(Cost newCost, int inventoryAmount, bool displayCost = false)
	{
		if (imageAlphaGroup != null)
		{
			if (inventoryAmount >= newCost.amount)
			{
				imageAlphaGroup.alpha = 1f;
			}
			else
			{
				imageAlphaGroup.alpha = 0.5f;
			}
		}
		isValidForInteract = inventoryAmount >= newCost.amount;
		if (backgroundButton != null)
		{
			backgroundButton.interactable = inventoryAmount >= newCost.amount;
		}
		if (amount != null)
		{
			string empty = string.Empty;
			empty = ((!showXBeforeAmount) ? inventoryAmount.ToString() : ("x" + inventoryAmount));
			if (displayCost)
			{
				empty = empty + "/" + newCost.amount;
			}
			amount.text = empty;
		}
	}
}
