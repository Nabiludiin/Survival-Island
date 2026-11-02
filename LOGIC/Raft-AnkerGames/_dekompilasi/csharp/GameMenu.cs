using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameMenu
{
	public MenuType menuType;

	public bool recieveEventMessages;

	public GameObject messageReciever;

	public GraphicRaycaster canvasRaycaster;

	public List<GameObject> menuObjects = new List<GameObject>();

	private bool isOpen;

	public bool IsOpen
	{
		get
		{
			return isOpen;
		}
		private set
		{
			isOpen = value;
		}
	}

	public void Initialize()
	{
		IsOpen = false;
		if (canvasRaycaster != null)
		{
			canvasRaycaster.enabled = false;
		}
	}

	public void Open()
	{
		if (IsOpen)
		{
			return;
		}
		IsOpen = true;
		foreach (GameObject menuObject in menuObjects)
		{
			if (menuObject.name == "CraftingConsole")
			{
				menuObject.SetActive(CanvasHelper.Gamepad);
			}
			else if (menuObject.name == "Crafting")
			{
				menuObject.SetActive(!CanvasHelper.Gamepad);
			}
			else if (!(menuObject.transform.parent.name == "_CanvasTradingPost"))
			{
				menuObject.SetActive(value: true);
			}
		}
		if (canvasRaycaster != null)
		{
			canvasRaycaster.enabled = true;
		}
		if (recieveEventMessages && messageReciever != null)
		{
			messageReciever.SendMessage("MenuOpen", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void Close()
	{
		if (!IsOpen)
		{
			return;
		}
		IsOpen = false;
		foreach (GameObject menuObject in menuObjects)
		{
			if (menuObject != null)
			{
				menuObject.SetActive(value: false);
			}
		}
		if (canvasRaycaster != null)
		{
			canvasRaycaster.enabled = false;
		}
		if (recieveEventMessages && messageReciever != null)
		{
			messageReciever.SendMessage("MenuClose", SendMessageOptions.DontRequireReceiver);
		}
	}
}
