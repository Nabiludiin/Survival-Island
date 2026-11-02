using UnityEngine;

public class CanvasDisabler : MonoBehaviour
{
	private void Start()
	{
		CanvasHelper.MenuOpenEvent += CanvasHelper_MenuOpenEvent;
		CanvasHelper.MenuCloseEvent += CanvasHelper_MenuCloseEvent;
	}

	private void CanvasHelper_MenuOpenEvent(MenuType menuType)
	{
	}

	private void CanvasHelper_MenuCloseEvent(MenuType menuType)
	{
	}
}
