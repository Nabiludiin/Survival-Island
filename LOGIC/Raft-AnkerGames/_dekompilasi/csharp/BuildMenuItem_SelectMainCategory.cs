using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildMenuItem_SelectMainCategory : Selectable, IPointerEnterHandler, IEventSystemHandler
{
	public static BuildMenuItem_SelectMainCategory CurrentCategory;

	private static BuildMenu buildMenu;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private GameObject horizontalParent;

	[SerializeField]
	private Item_Base buildableItem;

	private new void Start()
	{
		SetHorizontalParentState(state: false);
	}

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
		SetHorizontalParentState(state: true);
	}

	public void SetBuildableItem(Item_Base buildableItem)
	{
		this.buildableItem = buildableItem;
		RefreshIcon();
	}

	public void SetHorizontalParentState(bool state)
	{
		if (!state && BuildMenuItem_SelectSubCategory.CurrentSubCategory != null)
		{
			BuildMenuItem_SelectSubCategory.CurrentSubCategory.SetVerticalParentState(state: false);
			BuildMenuItem_SelectSubCategory.CurrentSubCategory = null;
		}
		if (state && CurrentCategory != this)
		{
			if (CurrentCategory != null)
			{
				CurrentCategory.SetHorizontalParentState(state: false);
			}
			CurrentCategory = this;
		}
		horizontalParent.SetActiveSafe(state);
	}

	private void RefreshIcon()
	{
		if (buildableItem != null && icon != null)
		{
			icon.sprite = buildableItem.settings_Inventory.Sprite;
		}
	}
}
