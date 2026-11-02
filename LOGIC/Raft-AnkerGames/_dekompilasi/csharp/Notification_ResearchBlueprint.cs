using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification_ResearchBlueprint : Notification_Animated
{
	public Queue<Notification_ResearchBlueprint_Info> infoQue = new Queue<Notification_ResearchBlueprint_Info>();

	[SerializeField]
	private Text text_itemName;

	[SerializeField]
	private Image image_itemImage;

	public override void Show()
	{
		Notification_ResearchBlueprint_Info notification_ResearchBlueprint_Info = infoQue.Dequeue();
		text_itemName.text = notification_ResearchBlueprint_Info.itemName;
		image_itemImage.sprite = notification_ResearchBlueprint_Info.itemSprite;
		base.Show();
	}
}
