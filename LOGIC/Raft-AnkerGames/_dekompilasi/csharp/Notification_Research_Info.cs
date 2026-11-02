using UnityEngine;

public class Notification_Research_Info
{
	public string itemName;

	public Sprite itemSprite;

	public Network_UserId researcher;

	public Notification_Research_Info(string itemName, Network_UserId researcher, Sprite itemSprite)
	{
		this.itemName = itemName;
		this.researcher = researcher;
		this.itemSprite = itemSprite;
	}
}
