using UnityEngine;
using UnityEngine.InputSystem;

public class ItemNet : PickupItem, IRaycastable
{
	[HideInInspector]
	public ItemCollector itemCollector;

	private bool displayText;

	private CanvasHelper canvas;

	private void Awake()
	{
		itemCollector = GetComponentInChildren<ItemCollector>();
	}

	public override void Start()
	{
		base.Start();
		canvas = ComponentManager<CanvasHelper>.Value;
	}

	public void OnIsRayed()
	{
		if (Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance) && itemCollector.collectedItems.Count > 0)
		{
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			LocalizationParameters.itemCount = itemCollector.collectedItems.Count;
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/CollectItemCount", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			displayText = true;
		}
		else
		{
			if (canvas != null)
			{
				canvas.displayTextManager.HideDisplayTexts();
			}
			displayText = false;
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (displayText)
		{
			canvas.displayTextManager.HideDisplayTexts();
			displayText = false;
		}
	}
}
