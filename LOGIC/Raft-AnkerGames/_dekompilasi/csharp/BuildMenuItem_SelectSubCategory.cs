using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildMenuItem_SelectSubCategory : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
	public static BuildMenuItem_SelectSubCategory CurrentSubCategory;

	private static BuildMenu buildMenu;

	[SerializeField]
	private BuildMenuItem_SelectMainCategory mainCategory;

	[SerializeField]
	private Item_Base buildableItem;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private GameObject verticalParent;

	private void OnValidate()
	{
		RefreshIcon();
	}

	private void Start()
	{
		SetVerticalParentState(state: false);
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
		SetVerticalParentState(state: true);
		if (mainCategory != null)
		{
			mainCategory.SetBuildableItem(buildableItem);
		}
	}

	public void SetBuildableItem(Item_Base buildableItem)
	{
		this.buildableItem = buildableItem;
		RefreshIcon();
		if (mainCategory != null)
		{
			mainCategory.SetBuildableItem(buildableItem);
		}
	}

	public void SetVerticalParentState(bool state)
	{
		if (state && CurrentSubCategory != this)
		{
			if (CurrentSubCategory != null)
			{
				CurrentSubCategory.SetVerticalParentState(state: false);
			}
			CurrentSubCategory = this;
		}
		verticalParent.SetActiveSafe(state);
	}

	private void RefreshIcon()
	{
		if (buildableItem != null && icon != null)
		{
			icon.sprite = buildableItem.settings_Inventory.Sprite;
		}
	}
}
