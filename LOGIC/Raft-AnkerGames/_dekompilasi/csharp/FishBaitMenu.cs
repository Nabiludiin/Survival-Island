using UnityEngine;
using UnityEngine.UI;

public class FishBaitMenu : MonoBehaviour
{
	private GamepadCursor cursor;

	private UiMagnet savedMagnet;

	[SerializeField]
	private UiMagnet defaultSelect;

	private static Image selectedImage;

	private void Start()
	{
		gameObject.SetActive(value: false);
		CanvasHelper.MenuPreCloseEvent += OnMenuClose;
		CanvasHelper.MenuOpenEvent += OnMenuOpen;
		cursor = ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>();
	}

	private void Update()
	{
		if (CanvasHelper.ActiveMenu != MenuType.FishingBait)
		{
			return;
		}
		UiMagnet magnetUnderCursor = cursor.GetMagnetUnderCursor();
		if (magnetUnderCursor != null && magnetUnderCursor.Layer == GamepadCursorLayer.Fishing)
		{
			Image image = magnetUnderCursor.GetComponent<UI_Cost_Interactable_FishingBait>().backgroundButton.image;
			image.sprite = ComponentManager<CraftingMenu>.Value.spriteHoover;
			if (selectedImage != null && selectedImage != image)
			{
				selectedImage.sprite = ComponentManager<CraftingMenu>.Value.spriteNormal;
			}
			selectedImage = image;
		}
	}

	private void OnMenuOpen(MenuType menuType)
	{
		if (menuType == MenuType.FishingBait)
		{
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
		if (menuType == MenuType.FishingBait)
		{
			savedMagnet = cursor.GetMagnetClosestToCursor();
		}
	}
}
