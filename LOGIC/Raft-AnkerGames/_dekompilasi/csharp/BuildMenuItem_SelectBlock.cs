using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildMenuItem_SelectBlock : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
	[SerializeField]
	private Item_Base buildableItem;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private BuildMenuItem_SelectSubCategory subCategory;

	private static BuildMenu buildMenu;

	private void OnValidate()
	{
		RefreshIcon();
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		OnEnter();
	}

	public void OnEnter()
	{
		if (buildMenu == null)
		{
			buildMenu = ComponentManager<BuildMenu>.Value;
		}
		if (buildMenu != null)
		{
			buildMenu.OnPointerEnterSubBlock(buildableItem);
		}
		if (subCategory != null)
		{
			subCategory.SetBuildableItem(buildableItem);
		}
	}

	private void RefreshIcon()
	{
		if (buildableItem != null && icon != null)
		{
			icon.sprite = buildableItem.settings_Inventory.Sprite;
		}
	}
}
