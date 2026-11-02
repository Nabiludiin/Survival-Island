using UnityEngine;
using UnityEngine.UI;

public class SkinMenuItem : MonoBehaviour
{
	[SerializeField]
	private Image image_item;

	[SerializeField]
	private Image image_highlight;

	[HideInInspector]
	public Item_Base skinItem;

	private static SelectedRecipeBox selectedRecipeBox;

	public static SkinMenuItem HighlightedSkin;

	private void Awake()
	{
		if (selectedRecipeBox == null)
		{
			selectedRecipeBox = GetComponentInParent<SelectedRecipeBox>();
		}
	}

	public void Button_OnClick()
	{
		if (selectedRecipeBox != null)
		{
			selectedRecipeBox.DisplaySkin(skinItem);
		}
		SetAsActiveHighlight();
	}

	public void SetAsActiveHighlight()
	{
		if (HighlightedSkin != this)
		{
			if (HighlightedSkin != null)
			{
				HighlightedSkin.ShowHighlight(value: false);
			}
			HighlightedSkin = this;
			HighlightedSkin.ShowHighlight(value: true);
		}
	}

	public void ShowHighlight(bool value)
	{
		image_highlight.gameObject.SetActiveSafe(value);
	}

	public void AttachSkinItem(Item_Base skinItem)
	{
		this.skinItem = skinItem;
		image_item.sprite = skinItem.settings_Inventory.Sprite;
		ShowHighlight(value: false);
	}
}
