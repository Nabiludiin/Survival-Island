using UnityEngine;

public class Notification_ResearchBlueprint_Info
{
	public string itemName;

	public Sprite itemSprite;

	public Notification_ResearchBlueprint_Info(Item_Base newItem)
	{
		itemName = newItem.settings_Inventory.DisplayName;
		itemSprite = newItem.settings_Inventory.Sprite;
	}
}
