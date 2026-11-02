using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI_CostBox : MonoBehaviour
{
	[SerializeField]
	private BuildingUI_CostBox_Sub subPrefab;

	[SerializeField]
	private List<BuildingUI_CostBox_Sub> subs;

	[SerializeField]
	private Transform subParent;

	[SerializeField]
	private Image radialImage;

	[SerializeField]
	protected Text label;

	[SerializeField]
	protected TextMeshProUGUI labelTextMeshPro;

	[SerializeField]
	protected Text displayName;

	[SerializeField]
	protected List<Item_Base> items;

	[SerializeField]
	protected int requiredAmount = 1;

	private int amount;

	private int lastAmount;

	private int lastRequiredAmount;

	private void Awake()
	{
		SetRadial(visible: false);
	}

	public virtual void Refresh()
	{
		if (items == null || items.Count == 0)
		{
			return;
		}
		if (subs == null || items.Count > subs.Count)
		{
			SetNewSubLength(items.Count);
		}
		if (displayName != null)
		{
			displayName.gameObject.SetActive(items.Count == 1);
		}
		for (int i = 0; i < subs.Count; i++)
		{
			BuildingUI_CostBox_Sub buildingUI_CostBox_Sub = subs[i];
			if (i < items.Count)
			{
				Item_Base item_Base = (buildingUI_CostBox_Sub.item = items[i]);
				if (buildingUI_CostBox_Sub.itemImage != null)
				{
					buildingUI_CostBox_Sub.itemImage.sprite = item_Base.settings_Inventory.Sprite;
				}
				if (displayName != null && displayName.gameObject.activeInHierarchy && i == 0)
				{
					displayName.text = item_Base.settings_Inventory.DisplayName;
				}
				buildingUI_CostBox_Sub.gameObject.SetActive(value: true);
			}
			else
			{
				buildingUI_CostBox_Sub.gameObject.SetActive(value: false);
			}
		}
		if (GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.unlimitedResources)
		{
			if (lastRequiredAmount != requiredAmount)
			{
				lastRequiredAmount = requiredAmount;
				SetLabel(requiredAmount + "/" + requiredAmount);
			}
		}
		else if (lastAmount != amount || lastRequiredAmount != requiredAmount)
		{
			lastAmount = amount;
			lastRequiredAmount = requiredAmount;
			SetLabel(amount + "/" + requiredAmount);
		}
	}

	public virtual void PointerEnterSub(BuildingUI_CostBox_Sub sub)
	{
	}

	public virtual void PointerExitSub(BuildingUI_CostBox_Sub sub)
	{
	}

	public bool RequiresItem(Item_Base item)
	{
		foreach (Item_Base item2 in items)
		{
			if (item2.UniqueIndex == item.UniqueIndex)
			{
				return true;
			}
		}
		return false;
	}

	public bool RequiresItem(string uniqueName)
	{
		foreach (Item_Base item in items)
		{
			if (item.UniqueName == uniqueName)
			{
				return true;
			}
		}
		return false;
	}

	public bool MeetsRequirements()
	{
		return amount >= requiredAmount;
	}

	public void SetAmount(int newAmount)
	{
		amount = newAmount;
		Refresh();
	}

	public void IncrementAmount(int amountToIncrement)
	{
		amount += amountToIncrement;
		amount = Mathf.Clamp(amount, 0, requiredAmount);
		Refresh();
	}

	public void SetAmountInInventory(PlayerInventory inventory, bool includeSecondaryInventory)
	{
		if (!GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.unlimitedResources)
		{
			int num = 0;
			for (int i = 0; i < items.Count; i++)
			{
				if (!(items[i] == null))
				{
					num += inventory.GetItemCount(items[i].UniqueName);
					if (includeSecondaryInventory && inventory.secondInventory != null)
					{
						num += inventory.secondInventory.GetItemCount(items[i].UniqueName);
					}
				}
			}
			SetAmount(num);
		}
		else
		{
			SetAmount(int.MaxValue);
		}
	}

	public void SetRequiredItem(Item_Base item)
	{
		if (items.Count == 0)
		{
			items.Add(item);
		}
		else
		{
			items[0] = item;
		}
		Refresh();
	}

	public void SetRequiredItem(Item_Base[] items)
	{
		this.items = items.ToList();
		Refresh();
	}

	public void SetRequiredItem(List<Item_Base> items)
	{
		this.items = items;
		Refresh();
	}

	public void SetRequiredAmount(int newRequiredAmount)
	{
		requiredAmount = newRequiredAmount;
		Refresh();
	}

	public int GetAmount()
	{
		return amount;
	}

	public void HideText()
	{
		if (labelTextMeshPro != null)
		{
			labelTextMeshPro.gameObject.SetActive(value: false);
		}
		else
		{
			label.gameObject.SetActive(value: false);
		}
	}

	public void ShowText()
	{
		if (labelTextMeshPro != null)
		{
			labelTextMeshPro.gameObject.SetActive(value: true);
		}
		else
		{
			label.gameObject.SetActive(value: true);
		}
	}

	public void Hide()
	{
		gameObject.SetActive(value: false);
	}

	public void Show()
	{
		gameObject.SetActive(value: true);
	}

	public void SetRadial(float normal)
	{
		if (!(radialImage == null))
		{
			if (!radialImage.gameObject.activeSelf)
			{
				SetRadial(visible: true);
			}
			radialImage.fillAmount = normal;
		}
	}

	public void SetRadial(bool visible)
	{
		if (!(radialImage == null))
		{
			radialImage.gameObject.SetActive(visible);
		}
	}

	public void SetRadial(Sprite sprite)
	{
		radialImage.sprite = sprite;
	}

	private void SetNewSubLength(int length)
	{
		if (subs == null)
		{
			subs = new List<BuildingUI_CostBox_Sub>();
		}
		while (subs.Count < length)
		{
			BuildingUI_CostBox_Sub buildingUI_CostBox_Sub = Object.Instantiate(subPrefab, subParent);
			buildingUI_CostBox_Sub.GetComponent<RectTransform>().localPosition = Vector3.zero;
			subs.Add(buildingUI_CostBox_Sub);
		}
	}

	protected void SetLabel(string text)
	{
		if (labelTextMeshPro != null)
		{
			labelTextMeshPro.text = text;
		}
		else
		{
			label.text = text;
		}
	}
}
