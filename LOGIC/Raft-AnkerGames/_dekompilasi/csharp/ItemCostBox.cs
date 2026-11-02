using UnityEngine;
using UnityEngine.UI;

public class ItemCostBox : MonoBehaviour, SceneEventInterface
{
	public Text costLabel;

	public Text displayName;

	public Image itemImage;

	public GameObject hoverImage;

	public Cost cost;

	private static PlayerInventory playerInventory;

	private void Start()
	{
		playerInventory = ComponentManager<Network_Player>.Value.Inventory;
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			playerInventory = null;
		}
	}

	private void Update()
	{
		if (gameObject.activeInHierarchy)
		{
			Refresh();
		}
	}

	public void Refresh()
	{
		if (cost != null && !(cost.item == null))
		{
			RefreshCostAmount();
		}
	}

	public void SetNewItemCost(Cost cost)
	{
		if (cost != null && !(cost.item == null))
		{
			this.cost = cost;
			itemImage.sprite = cost.item.settings_Inventory.Sprite;
			if (displayName != null)
			{
				displayName.text = cost.item.settings_Inventory.DisplayName;
			}
			RefreshCostAmount();
		}
	}

	public void OnPointerEnter()
	{
		playerInventory.SetItemDescription(cost.item);
		hoverImage.SetActive(value: true);
	}

	public void OnPointerExit()
	{
		playerInventory.SetItemDescription(null);
		hoverImage.SetActive(value: false);
	}

	private void RefreshCostAmount()
	{
		if (playerInventory == null)
		{
			playerInventory = ComponentManager<Network_Player>.Value.Inventory;
		}
		if (playerInventory != null)
		{
			costLabel.text = playerInventory.GetItemCount(cost.item.UniqueName) + "/" + cost.amount;
		}
	}
}
