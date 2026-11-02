using UnityEngine;
using UnityEngine.UI;

public class AvaialableResearchItem : MonoBehaviour
{
	[SerializeField]
	private Image image;

	[SerializeField]
	private CanvasGroup canvasGroup;

	private Item_Base item;

	private bool researched;

	private PlayerInventory playerInventory;

	public bool Researched => researched;

	public Item_Base Item => item;

	private void OnDestroy()
	{
		playerInventory = null;
	}

	public void SetItem(Item_Base item)
	{
		this.item = item;
		playerInventory = ComponentManager<PlayerInventory>.Value;
		image.sprite = item.settings_Inventory.Sprite;
	}

	public void SetResearchedState(bool researched)
	{
		this.researched = researched;
		if (researched)
		{
			canvasGroup.alpha = 0.33f;
		}
		else
		{
			canvasGroup.alpha = 1f;
		}
	}

	public void PointerEnter()
	{
		if (playerInventory != null && item != null)
		{
			playerInventory.SetItemDescription(item);
		}
	}

	public void PointerExit()
	{
		if (playerInventory != null)
		{
			playerInventory.SetItemDescription(null);
		}
	}
}
