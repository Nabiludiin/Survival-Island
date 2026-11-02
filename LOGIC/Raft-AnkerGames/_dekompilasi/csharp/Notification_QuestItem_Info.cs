using UnityEngine;

public class Notification_QuestItem_Info
{
	public string itemName;

	public Sprite itemSprite;

	public Notification_QuestItem_Info(string itemName, int amountAdded, Sprite itemSprite)
	{
		this.itemName = amountAdded + "x " + itemName;
		this.itemSprite = itemSprite;
	}
}
