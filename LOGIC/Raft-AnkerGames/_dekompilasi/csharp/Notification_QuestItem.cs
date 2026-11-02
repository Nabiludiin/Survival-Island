using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification_QuestItem : Notification_Animated
{
	public Queue<Notification_QuestItem_Info> infoQue = new Queue<Notification_QuestItem_Info>();

	[SerializeField]
	private Text text_itemName;

	[SerializeField]
	private Image image_itemImage;

	public override void Show()
	{
		Notification_QuestItem_Info notification_QuestItem_Info = infoQue.Dequeue();
		text_itemName.text = notification_QuestItem_Info.itemName;
		image_itemImage.sprite = notification_QuestItem_Info.itemSprite;
		base.Show();
	}
}
