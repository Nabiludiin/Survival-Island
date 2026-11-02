using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour
{
	[Header("References")]
	[SerializeField]
	private GameObject buildPanel;

	[SerializeField]
	private Image selectedBlockImage;

	[SerializeField]
	private Text selectedBlockName;

	[SerializeField]
	private Text selectedBlockDescription;

	[SerializeField]
	private CostCollection costColletionPanel;

	[SerializeField]
	private CostCollection costColletionCursor;

	[SerializeField]
	private Vector3 categoryBackgroundOffset = Vector3.zero;

	private GameObject currentSubParent;

	private BlockCreator blockCreator;

	private Item_Base currentBuildable;

	[HideInInspector]
	public Item_Base lastSelectedBuildabe;

	private UiMagnet savedMagnet;

	private GamepadCursor cursor;

	[SerializeField]
	private UiMagnet defaultSelect;

	[SerializeField]
	private Sprite spriteNormal;

	[SerializeField]
	private Sprite spriteHighlighted;

	private Image selectedImage;

	private void Awake()
	{
		ComponentManager<BuildMenu>.Value = this;
	}

	private void Start()
	{
		buildPanel.SetActive(value: false);
		CanvasHelper.MenuPreCloseEvent += OnMenuClose;
		CanvasHelper.MenuOpenEvent += OnMenuOpen;
		Object.Destroy(GetComponent<Canvas>());
		cursor = ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>();
	}

	public void Initialize(BlockCreator blockCreator)
	{
		this.blockCreator = blockCreator;
		currentBuildable = blockCreator.GetCurrentBlockType();
		DisplayBlockInfo(currentBuildable);
	}

	private void Update()
	{
		if (blockCreator == null || !blockCreator.gameObject.activeInHierarchy)
		{
			return;
		}
		currentBuildable = blockCreator.GetCurrentBlockType();
		if (currentBuildable == null)
		{
			return;
		}
		bool flag = false;
		if (currentBuildable != null && blockCreator.IsBuildableItemUpgradeItem(currentBuildable) && blockCreator.upgradePreview.ghostBlock != null)
		{
			currentBuildable = blockCreator.upgradePreview.ghostBlock.buildableItem;
			flag = true;
		}
		if (currentBuildable.settings_buildable.Placeable)
		{
			if (costColletionCursor.gameObject.activeInHierarchy)
			{
				costColletionCursor.gameObject.SetActive(value: false);
			}
			return;
		}
		if (CanvasHelper.ActiveMenu != MenuType.None)
		{
			if (costColletionCursor.gameObject.activeInHierarchy)
			{
				costColletionCursor.gameObject.SetActive(value: false);
			}
		}
		else if (CanvasHelper.ActiveMenu == MenuType.None && currentBuildable != null && !costColletionCursor.gameObject.activeInHierarchy)
		{
			costColletionCursor.gameObject.SetActive(value: true);
		}
		if (costColletionCursor.gameObject.activeInHierarchy)
		{
			CostMultiple[] newCost = currentBuildable.settings_recipe.NewCost;
			costColletionCursor.ShowCost(newCost);
		}
		if (!flag && buildPanel.activeInHierarchy && currentBuildable != null)
		{
			CostMultiple[] newCost2 = currentBuildable.settings_recipe.NewCost;
			costColletionPanel.ShowCost(newCost2);
		}
		if (CanvasHelper.ActiveMenu != MenuType.BuildMenu)
		{
			return;
		}
		UiMagnet magnetUnderCursor = cursor.GetMagnetUnderCursor();
		if (magnetUnderCursor != null && magnetUnderCursor.Layer == GamepadCursorLayer.BuildMenu)
		{
			Image component = magnetUnderCursor.GetComponent<Image>();
			component.sprite = (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad ? spriteHighlighted : spriteNormal);
			if (selectedImage != null && selectedImage != component)
			{
				selectedImage.sprite = spriteNormal;
			}
			selectedImage = component;
		}
	}

	public void OnPointerEnterSubBlock(Item_Base buildableSubItem)
	{
		SelectBlock(buildableSubItem);
	}

	public void OnHammerDeselect()
	{
		costColletionCursor.gameObject.SetActive(value: false);
	}

	public void SelectBlock(Item_Base buildableItem)
	{
		if (!(buildableItem == null))
		{
			DisplayBlockInfo(buildableItem);
			currentBuildable = buildableItem;
			lastSelectedBuildabe = buildableItem;
			blockCreator.SetBlockTypeToBuild(currentBuildable.UniqueName);
		}
	}

	private void DisplayBlockInfo(Item_Base buildableItem)
	{
		selectedBlockImage.sprite = buildableItem.settings_Inventory.Sprite;
		selectedBlockName.text = buildableItem.settings_Inventory.DisplayName;
		selectedBlockDescription.text = buildableItem.settings_Inventory.Description;
	}

	private void OnMenuOpen(MenuType menuType)
	{
		if (menuType == MenuType.BuildMenu)
		{
			cursor.SetLayer(GamepadCursorLayer.BuildMenu);
			if (savedMagnet != null)
			{
				cursor.FocusMagnet(savedMagnet);
			}
			else
			{
				cursor.FocusMagnet(defaultSelect);
			}
		}
	}

	private void OnMenuClose(MenuType menuType)
	{
		if (menuType == MenuType.BuildMenu)
		{
			savedMagnet = cursor.GetMagnetClosestToCursor();
			cursor.SetLayer(GamepadCursorLayer.Inventory);
		}
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad && menuType == MenuType.BuildMenu)
		{
			if (BuildMenuItem_SelectMainCategory.CurrentCategory != null)
			{
				BuildMenuItem_SelectMainCategory.CurrentCategory.SetHorizontalParentState(state: false);
				BuildMenuItem_SelectMainCategory.CurrentCategory = null;
			}
			if (BuildMenuItem_SelectSubCategory.CurrentSubCategory != null)
			{
				BuildMenuItem_SelectSubCategory.CurrentSubCategory.SetVerticalParentState(state: false);
				BuildMenuItem_SelectSubCategory.CurrentSubCategory = null;
			}
		}
	}
}
